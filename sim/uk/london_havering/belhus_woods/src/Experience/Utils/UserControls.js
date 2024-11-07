import * as THREE from "three"

import EventEmitter from "./EventEmitter.js"
import { MESSAGES } from "./messages.js"


const MOVEMENT_CONTROLS = {
    tilt_pan: "tilt_pan",
    truck_dolly: "truck_dolly",
}


export default class UserControls extends EventEmitter
{
    constructor(experience, sizes, camera)
    {
        super()

        this.state = {
            movement_controls: MOVEMENT_CONTROLS.truck_dolly,
            pointer_is_down: false,
        }
        this.experience = experience
        this.sizes = sizes
        this.camera = camera
        this.create_ui()
        this.listen_for_pointer()
    }

    create_ui()
    {
        // Setup
        const root_el = document.getElementsByTagName("body")[0]

        this.container_el = document.createElement("div")
        root_el.appendChild(this.container_el)
        this.container_el.className = "user_controls"

        // style sheet
        const style_el = document.createElement("style")
        this.container_el.appendChild(style_el)
        // Add styles
        style_el.innerHTML = `
            .user_controls {
                position: fixed;
                bottom: 0;
                left: 0;
                padding: 10px;
                margin: 5px;
                display: flex;
                flex-direction: column;
                gap: 20px;
                background-color: rgba(255, 255, 255, 0.5);
                border-radius: 10px;
            }

            .user_controls img {
                width: 50px;
                height: 50px;
                cursor: pointer;
                opacity: 0.8;
            }

            .user_controls img:hover {
                opacity: 1.0;
            }

            .user_controls img.disabled {
                display: none;
            }
        `

        this.arrows_truck_dolly = this.add_image("./textures/symbols/arrows_truck_dolly_thin.png", () =>
        {
            this.trigger(MESSAGES.UserControls.movement_controls, [MOVEMENT_CONTROLS.tilt_pan])
        })

        this.arrows_tilt_pan = this.add_image("./textures/symbols/arrows_tilt_pan_thin.png", () =>
        {
            this.trigger(MESSAGES.UserControls.movement_controls, [MOVEMENT_CONTROLS.truck_dolly])
        })

        this.update_ui()

        this.on(MESSAGES.UserControls.movement_controls, (movement_controls) =>
        {
            this.state.movement_controls = movement_controls
            this.update_ui()
        })
    }

    add_image(src, callback)
    {
        const image_el = document.createElement("img")
        this.container_el.appendChild(image_el)
        image_el.src = src
        image_el.addEventListener("click", callback)
        image_el.classList.add("disabled")

        return image_el
    }

    update_ui()
    {
        this.arrows_truck_dolly.classList.toggle("disabled", this.state.movement_controls === MOVEMENT_CONTROLS.tilt_pan)
        this.arrows_tilt_pan.classList.toggle("disabled", this.state.movement_controls === MOVEMENT_CONTROLS.truck_dolly)
    }

    listen_for_pointer()
    {
        const raycaster = new THREE.Raycaster()
        const screen_cursor = new THREE.Vector2()
        let terrain_mesh = null
        let start_world_point = null
        let current_world_point = null


        document.addEventListener("pointerdown", (event) =>
        {
            this.state.pointer_is_down = event.buttons === 1
            start_world_point = null
            current_world_point = null
        })

        document.addEventListener("pointerup", (event) =>
        {
            this.state.pointer_is_down = false
            start_world_point = null
            current_world_point = null
        })

        document.addEventListener("pointermove", (event) =>
        {
            if (!terrain_mesh)
            {
                if (this.experience.world?.terrain.simple_under_mesh)
                {
                    terrain_mesh = this.experience.world.terrain.simple_under_mesh
                } else return
            }

            if (!this.state.pointer_is_down) return

            screen_cursor.x = (event.clientX / this.sizes.width) * 2 - 1
            screen_cursor.y = - (event.clientY / this.sizes.height) * 2 + 1

            raycaster.setFromCamera(screen_cursor, this.camera.instance)
            const intersections = raycaster.intersectObject(terrain_mesh)
            if (!intersections.length) return

            if (!start_world_point) start_world_point = intersections[0].point
            current_world_point = intersections[0].point

            this.trigger(MESSAGES.UserControls.pointer_down, [start_world_point, current_world_point])
        })
    }
}

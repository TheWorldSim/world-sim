import * as THREE from "three"

import EventEmitter from "./EventEmitter.js"
import { MESSAGES } from "./messages.js"
import { MOVEMENT_CONTROLS, TERRAIN_HEIGHT_MAP } from "../constants.js"


const next_terrain_height_map = (current) =>
{
    if (current === TERRAIN_HEIGHT_MAP.none) return TERRAIN_HEIGHT_MAP.height
    if (current === TERRAIN_HEIGHT_MAP.height) return TERRAIN_HEIGHT_MAP.watersheds
    return TERRAIN_HEIGHT_MAP.none
}
const terrain_height_map_to_string = (current) =>
{
    if (current === TERRAIN_HEIGHT_MAP.none) return "Show height map"
    if (current === TERRAIN_HEIGHT_MAP.height) return "Show watersheds"
    return "Hide height map"
}


export default class UserControls extends EventEmitter
{
    constructor(experience, sizes, camera)
    {
        super()

        this.state = {
            beavers_present: false,
            movement_controls: MOVEMENT_CONTROLS.truck_dolly,
            pointer_is_down: false,
            terrain_height_map: TERRAIN_HEIGHT_MAP.none,
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
                margin: 10px;
                display: flex;
                flex-direction: row;
                gap: 10px;
            }

            .user_controls > img, .user_controls > div {
                width: 40px;
                height: 40px;
                cursor: pointer;
                opacity: 0.8;
                background-color: rgba(255, 255, 255, 0.5);
                border-radius: 10px;
                padding: 10px;
            }

            .user_controls > div {
                width: 60px;
                height: 60px;
                padding: 0px;
            }

            .user_controls > img:hover, .user_controls > div:hover {
                opacity: 1.0;
            }

            .user_controls > img.disabled {
                display: none;
            }

            .user_controls > div {
                font-size: 13px;
                font-family: sans-serif;
                align-content: center;
                text-align: center;
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

        this.toggle_beavers = this.add_div("Beavers", () =>
        {
            this.state.beavers_present = !this.state.beavers_present
            this.trigger(MESSAGES.UserControls.beaver_presence, [this.state.beavers_present])
            this.update_ui()
        })

        this.toggle_terrain_height_map = this.add_div("", () =>
        {
            this.state.terrain_height_map = next_terrain_height_map(this.state.terrain_height_map)
            this.trigger(MESSAGES.UserControls.terrain_height_map, [this.state.terrain_height_map])
            this.update_ui()
        })
        this.toggle_terrain_height_map.style.fontSize = "10px"

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

    add_div(text, callback)
    {
        const div_el = document.createElement("div")
        this.container_el.appendChild(div_el)

        div_el.innerText = text
        div_el.addEventListener("click", callback)
        // div_el.classList.add("disabled")

        return div_el
    }

    update_ui()
    {
        this.arrows_truck_dolly.classList.toggle("disabled", this.state.movement_controls === MOVEMENT_CONTROLS.tilt_pan)
        this.arrows_tilt_pan.classList.toggle("disabled", this.state.movement_controls === MOVEMENT_CONTROLS.truck_dolly)
        this.toggle_beavers.innerText = this.state.beavers_present ? "Remove beavers" : "Add beavers"
        this.toggle_terrain_height_map.innerText = terrain_height_map_to_string(this.state.terrain_height_map)
    }

    listen_for_pointer()
    {
        const raycaster = new THREE.Raycaster()
        const screen_cursor = new THREE.Vector2()
        let terrain_mesh = null
        let start_world_point = null
        let current_world_point = null


        // Prevent the screen from moving when the user drags a slide in the
        // lil-gui
        const lil_gui_el = document.getElementsByClassName("lil-gui")[0]
        lil_gui_el?.addEventListener("pointerdown", (event) =>
        {
            event.stopPropagation()
        })

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

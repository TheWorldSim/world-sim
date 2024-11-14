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
    constructor(experience, debug, sizes, camera)
    {
        super()

        this.state = {
            beavers_present: false,
            movement_controls: MOVEMENT_CONTROLS.truck_dolly,
            pointer_is_down: false,
            terrain_height_map: TERRAIN_HEIGHT_MAP.none,
        }
        this.experience = experience
        this.debug = debug
        this.sizes = sizes
        this.camera = camera
        this.create_ui()
        this.listen_for_pointer()
    }

    create_ui()
    {
        const body_el = document.getElementsByTagName("body")[0]
        this.create_buttons_drawer(body_el)
        this.create_menu(body_el)
    }

    // Create the buttons drawer at the bottom of the screen
    create_buttons_drawer(body_el)
    {
        // Setup
        const container_el = document.createElement("div")
        body_el.appendChild(container_el)
        container_el.className = "user_controls_drawer"

        // style sheet
        const style_el = document.createElement("style")
        style_el.id = "user_controls_drawer_style"
        container_el.appendChild(style_el)
        // Add styles
        style_el.innerHTML = `
            .user_controls_drawer {
                position: fixed;
                bottom: 0;
                left: 0;
                margin: 10px;
                display: flex;
                flex-direction: row;
                gap: 10px;
            }

            .user_controls_drawer > img, .user_controls_drawer > div {
                width: 60px;
                height: 60px;
                cursor: pointer;
                opacity: 0.8;
                background-color: rgba(255, 255, 255, 0.5);
                border-radius: 10px;
                padding: 10px;
            }

            .user_controls_drawer > div {
                padding: 0px;
            }

            .user_controls_drawer > img:hover, .user_controls_drawer > div:hover {
                opacity: 1.0;
            }

            .user_controls_drawer > img.disabled {
                display: none;
            }

            .user_controls_drawer > div {
                font-size: 13px;
                font-family: sans-serif;
                align-content: center;
                text-align: center;
            }
        `

        this.arrows_truck_dolly = add_image(container_el, "./textures/symbols/arrows_truck_dolly_thin.png", () =>
        {
            this.trigger(MESSAGES.UserControls.movement_controls, [MOVEMENT_CONTROLS.tilt_pan])
        })

        this.arrows_tilt_pan = add_image(container_el, "./textures/symbols/arrows_tilt_pan_thin.png", () =>
        {
            this.trigger(MESSAGES.UserControls.movement_controls, [MOVEMENT_CONTROLS.truck_dolly])
        })

        this.toggle_beavers = add_div(container_el, "Beavers", () =>
        {
            this.state.beavers_present = !this.state.beavers_present
            this.trigger(MESSAGES.UserControls.beaver_presence, [this.state.beavers_present])
            this.update_ui()
        })

        this.toggle_terrain_height_map = add_div(container_el, "", () =>
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

    create_menu(body_el)
    {
        const menu_el = document.createElement("div")
        body_el.appendChild(menu_el)
        menu_el.className = "user_controls_menu dropdown"
        menu_el.innerHTML = `
            <button
                class="btn btn-secondary dropdown-toggle"
                type="button"
                id="dropdownMenuButton"
                data-bs-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
            >
                Menu
            </button>
            <ul id="dropdown_menu" class="dropdown-menu" aria-labelledby="dropdownMenuButton"></ul>
        `

        const dropdown_menu_el = document.getElementById("dropdown_menu")

        this.add_menu_item_toggle_debug(dropdown_menu_el)

        // style sheet
        const style_el = document.createElement("style")
        body_el.appendChild(style_el)
        style_el.id = "user_controls_menu_style"
        // Add styles
        style_el.innerHTML = `
            .user_controls_menu {
                position: fixed;
                top: 0;
                right: 0;
                width: 100px;
                z-index: 1002; /* Above the lil-gui */
                text-align: right; /* Align the dropdown to the right */
                margin: 10px;
                cursor: pointer;
            }
        `
    }

    add_menu_item_toggle_debug(dropdown_menu_el)
    {
        function get_menu_item_text(debug_is_active)
        {
            return debug_is_active ? "Hide debug options" : "Show debug options"
        }

        const toggle_debug = (menu_item_el) =>
        {
            this.debug.toggle_debug()
            menu_item_el.innerText = get_menu_item_text(this.debug.is_active())
        }

        add_menu_item(dropdown_menu_el, "", (menu_item_el) =>
        {
            menu_item_el.innerText = get_menu_item_text(this.debug.is_active())

            return () => toggle_debug(menu_item_el)
        })
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



function add_image(container_el, src, callback)
{
    const image_el = document.createElement("img")
    container_el.appendChild(image_el)
    image_el.src = src
    image_el.addEventListener("click", callback)
    image_el.classList.add("disabled")

    return image_el
}

function add_div(container_el, text, callback)
{
    const div_el = document.createElement("div")
    container_el.appendChild(div_el)

    div_el.innerText = text
    div_el.addEventListener("click", callback)
    // div_el.classList.add("disabled")

    return div_el
}

function add_menu_item(dropdown_menu_el, text, factory_callback)
{
    const li_el = document.createElement("li")
    dropdown_menu_el.appendChild(li_el)
    li_el.className = "dropdown-item"
    li_el.innerText = text
    li_el.addEventListener("click", factory_callback(li_el))
    return li_el
}

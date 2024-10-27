import EventEmitter from "./EventEmitter.js"
import { MESSAGES } from "./messages.js"


const MOVEMENT_CONTROLS = {
    tilt_pan: "tilt_pan",
    truck_dolly: "truck_dolly",
}


export default class UserControls extends EventEmitter
{
    constructor()
    {
        super()

        this.state = {
            movement_controls: MOVEMENT_CONTROLS.tilt_pan,
        }
        this.create_ui()
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
}

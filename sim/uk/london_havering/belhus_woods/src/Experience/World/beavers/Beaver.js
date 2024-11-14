import Experience from "../../Experience.js"
import { MESSAGES } from "../../Utils/messages.js"
import EventEmitter from "../../Utils/EventEmitter.js"


export default class Beaver extends EventEmitter
{
    constructor(water)
    {
        super()

        this.state = {
            is_active: false,
            built_dam: false,
            dam_built_x_position: null,
            dam_built_z_height: null,
            target_dam_x_position: 50,
            target_dam_z_height: 100,
        }

        const experience = new Experience()
        experience.user_controls.on(MESSAGES.UserControls.beaver_presence, (is_active) =>
        {
            this.state.is_active = is_active
        })

        this.water = water

        this.setup_debug_gui(experience.debug.ui)
    }

    setup_debug_gui(debug_gui)
    {
        this.debug_gui_folder = debug_gui.addFolder("Beaver")
        this.debug_gui_folder
            .add(this.state, "target_dam_x_position")
            .min(50).max(300).step(1)
            .name("Dam position")

        this.debug_gui_folder
            .add(this.state, "target_dam_z_height")
            .min(20).max(200).step(1)
            .name("Dam height")
    }

    update()
    {
        this.state.is_active ? this.ensure_dam_built() : this.reset()
    }

    ensure_dam_built()
    {
        if (this.state.built_dam && this.state.dam_built_x_position === this.state.target_dam_x_position && this.state.dam_built_z_height === this.state.target_dam_z_height) return
        this.state.built_dam = true
        this.state.dam_built_x_position = this.state.target_dam_x_position
        this.state.dam_built_z_height = this.state.target_dam_z_height

        this.water.reset_height_map_texture_to_initial()
        this.water.alter_height_map_texture(watershed_input_data =>
        {
            const { image_data, width, height } = watershed_input_data

            // let min = Number.POSITIVE_INFINITY
            // let max = Number.NEGATIVE_INFINITY
            // for (let i = 0; i < image_data.length; ++i)
            // {
            //     min = Math.min(min, image_data[i])
            //     max = Math.max(max, image_data[i])
            // }
            // console.log(`min, max: ${min}, ${max}`)

            for (let i = 0; i < height; ++i)
            {
                const index = (i * width) + this.state.target_dam_x_position
                const current_z = image_data[index]
                const new_z = Math.max(current_z, this.state.target_dam_z_height)
                image_data[index] = new_z
            }

            return watershed_input_data
        })
    }

    reset()
    {
        if (!this.state.built_dam) return
        this.state.built_dam = false

        this.water.reset_height_map_texture_to_initial()
        // this.trigger(MESSAGES.Beaver.dam_status_changed, [this.state.dam_built])
    }
}

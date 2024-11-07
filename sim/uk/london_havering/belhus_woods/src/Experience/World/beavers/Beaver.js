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
            dam_built: false,
        }

        const experience = new Experience()
        experience.user_controls.on(MESSAGES.UserControls.beaver_presence, (is_active) =>
        {
            this.state.is_active = is_active
        })

        this.water = water
    }

    update()
    {
        this.state.is_active ? this.ensure_dam_built() : this.reset()
    }

    ensure_dam_built()
    {
        if (this.state.dam_built) return
        this.state.dam_built = true
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
                image_data[(i * width) + 50] = 248
            }

            return watershed_input_data
        })
        // this.trigger(MESSAGES.Beaver.dam_status_changed, [this.state.dam_built])
    }

    reset()
    {
        if (!this.state.dam_built) return
        this.state.dam_built = false
        this.water.reset_height_map_texture_to_initial()
        // this.trigger(MESSAGES.Beaver.dam_status_changed, [this.state.dam_built])
    }
}

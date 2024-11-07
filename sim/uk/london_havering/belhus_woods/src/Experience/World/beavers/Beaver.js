import Experience from "../../Experience.js"
import { MESSAGES } from "../../Utils/messages.js"
import EventEmitter from "../../Utils/EventEmitter.js"


export default class Beaver extends EventEmitter
{
    constructor()
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
    }

    update()
    {
        this.state.is_active ? this.ensure_dam_built() : this.reset()
    }

    ensure_dam_built()
    {
        if (this.state.dam_built) return
        this.state.dam_built = true
        this.trigger(MESSAGES.Beaver.dam_status_changed, [this.state.dam_built])
    }

    reset()
    {
        if (!this.state.dam_built) return
        this.state.dam_built = false
        this.trigger(MESSAGES.Beaver.dam_status_changed, [this.state.dam_built])
    }
}

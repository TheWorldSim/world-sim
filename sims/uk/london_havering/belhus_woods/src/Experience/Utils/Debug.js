import GUI from "lil-gui"


export default class Debug
{
    constructor()
    {
        const style_el = document.createElement("style")
        style_el.id = "debug_style"
        style_el.textContent = `
        .lil-gui.autoPlace
        {
            right: 50px;
        }
        `
        document.head.appendChild(style_el)

        this.ui = new GUI()
        this.update_ui()
    }

    is_active()
    {
        return window.location.hash.includes("debug")
    }

    toggle_debug()
    {
        if (this.is_active()) window.location.hash = window.location.hash.replace("debug", "")
        else window.location.hash += "debug"

        this.update_ui()
    }

    update_ui()
    {
        if (this.is_active())
        {
            this.ui.show()
        }
        else
        {
            this.ui.hide()
        }
    }
}

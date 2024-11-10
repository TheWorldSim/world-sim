import * as THREE from "three"

import Debug from "./Utils/Debug.js"
import Sizes from "./Utils/Sizes.js"
import Time from "./Utils/Time.js"
import Camera from "./Camera.js"
import Renderer from "./Renderer.js"
import World from "./World/World.js"
import Resources from "./Utils/Resources.js"
import UserControls from "./Utils/UserControls.js"
import { MESSAGES } from "./Utils/messages.js"

import sources, { define_shader_chunks } from "./sources.js"

let instance = null

export default class Experience
{
    constructor(_canvas)
    {
        // Singleton
        if(instance)
        {
            return instance
        }
        instance = this

        // Global access
        window.experience = this

        // Options
        this.canvas = _canvas

        // Setup
        this.debug = new Debug()
        this.sizes = new Sizes()
        this.time = new Time()
        this.scene = new THREE.Scene()
        this.resources = new Resources(sources)
        define_shader_chunks()
        this.camera = new Camera()
        this.user_controls = new UserControls(this, this.sizes, this.camera)
        this.camera.setup_listen_to_user_controls(this.user_controls)
        this.renderer = new Renderer()
        this.world = new World()

        // Resize event
        this.sizes.on(MESSAGES.Sizes.resize, () =>
        {
            this.resize()
        })

        // Time tick event
        this.time.on(MESSAGES.Time.tick, () =>
        {
            this.update()
        })
    }

    resize()
    {
        this.camera.resize()
        this.renderer.resize()
    }

    update()
    {
        this.camera.update()
        this.world.update()
        this.renderer.update()
    }

    destroy()
    {
        this.sizes.off(MESSAGES.Sizes.resize)
        this.time.off(MESSAGES.Time.tick)

        // Traverse the whole scene
        this.scene.traverse((child) =>
        {
            // Test if it's a mesh
            if(child instanceof THREE.Mesh)
            {
                child.geometry.dispose()

                // Loop through the material properties
                for(const key in child.material)
                {
                    const value = child.material[key]

                    // Test if there is a dispose function
                    if(value && typeof value.dispose === "function")
                    {
                        value.dispose()
                    }
                }
            }
        })

        this.camera.controls.dispose()
        this.renderer.instance.dispose()

        if(this.debug.active)
            this.debug.ui.destroy()
    }
}

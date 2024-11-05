import * as THREE from "three"
import Experience from "../Experience.js"
import { MESSAGES } from "../Utils/messages.js"

export default class Fox
{
    constructor()
    {
        this.finised_initialising = false

        this.experience = new Experience()
        this.scene = this.experience.scene
        this.resources = this.experience.resources
        this.time = this.experience.time
        this.debug = this.experience.debug

        // Debug
        if(this.debug.active)
        {
            this.debugFolder = this.debug.ui.addFolder("fox")
        }

        this.resources.on(MESSAGES.Resources.ready, () =>
        {
            // Resource
            this.resource = this.resources.items.foxModel

            this.setModel()
            this.setAnimation()
            this.finised_initialising = true
        })
    }

    setModel()
    {
        this.model_scale = 0.002
        this.model = this.resource.scene
        this.model.scale.set(this.model_scale, this.model_scale, this.model_scale)
        this.scene.add(this.model)

        this.model.traverse((child) =>
        {
            if(child instanceof THREE.Mesh)
            {
                child.castShadow = true
            }
        })

        this.model_angle = 0
        this.track_radius = 1
        this.model.position.set(this.track_radius, 0.7, 0)
    }

    setAnimation()
    {
        this.animation = {}

        // Mixer
        this.animation.mixer = new THREE.AnimationMixer(this.model)

        // Actions
        this.animation.actions = {}

        this.animation.actions.idle = this.animation.mixer.clipAction(this.resource.animations[0])
        this.animation.actions.walking = this.animation.mixer.clipAction(this.resource.animations[1])
        this.animation.actions.running = this.animation.mixer.clipAction(this.resource.animations[2])

        this.animation.actions.current = this.animation.actions.walking
        this.animation.actions.current.play()

        // Play the action
        this.animation.play = (name) =>
        {
            const newAction = this.animation.actions[name]
            const oldAction = this.animation.actions.current

            newAction.reset()
            newAction.play()
            newAction.crossFadeFrom(oldAction, 0.3)

            this.animation.actions.current = newAction
        }

        // Debug
        if(this.debug.active)
        {
            const debugObject = {
                playIdle: () => {
                    if (this.animation.actions.current == this.animation.actions.running)
                    {
                        this.animation.play("walking")
                        setTimeout(() => { this.animation.play("idle") }, 500)
                    }
                    else
                    {
                        this.animation.play("idle")
                    }
                },
                playWalking: () => { this.animation.play("walking") },
                playRunning: () => { this.animation.play("running") }
            }
            this.debugFolder.add(debugObject, "playIdle")
            this.debugFolder.add(debugObject, "playWalking")
            this.debugFolder.add(debugObject, "playRunning")
        }
    }

    update()
    {
        if (!this.finised_initialising) return

        const is_model_walking = this.animation.actions.current == this.animation.actions.walking
        const is_model_running = this.animation.actions.current == this.animation.actions.running

        const animation_speed = is_model_running ? 1.6 : 1

        this.animation.mixer.update(this.time.delta * 0.001 * animation_speed)

        if (is_model_walking || is_model_running)
        {
            this.model_angle += (is_model_walking ? 0.13 : 0.4) * this.time.delta * this.model_scale

            this.model.position.z = Math.sin(this.model_angle) * this.track_radius
            this.model.position.x = Math.cos(this.model_angle) * this.track_radius
            this.model.rotation.y = -this.model_angle
        }
    }
}

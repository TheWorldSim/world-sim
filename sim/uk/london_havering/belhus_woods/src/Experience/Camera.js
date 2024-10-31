import * as THREE from "three"
import Experience from "./Experience.js"
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls.js"

export default class Camera
{
    constructor()
    {
        this.experience = new Experience()
        this.canvas = this.experience.canvas
        this.time = this.experience.time
        this.scene = this.experience.scene
        this.sizes = this.experience.sizes

        this.setInstance()
        this.setControls()
    }

    setInstance()
    {
        this.look_at_speed_ms = 3000
        this.look_at = {
            start_time: this.time.current,
            end_time: this.time.current,
            current: new THREE.Vector3(),
            target: new THREE.Vector3(),
        }
        this.instance = new THREE.PerspectiveCamera(35, this.sizes.width / this.sizes.height, 0.1, 300)
        this.instance.position.y = 40
        this.instance.position.x = 50
        this.instance.position.z = 60
        this.scene.add(this.instance)
    }

    setControls()
    {
        this.controls = new OrbitControls(this.instance, this.canvas)
        this.controls.enableDamping = true
        this.controls.maxDistance = 100
        this.controls.maxPolarAngle = THREE.MathUtils.degToRad(85) // Don't let the camera go below the ground
        // this.controls.minAzimuthAngle = THREE.MathUtils.degToRad(0)
        // this.controls.maxAzimuthAngle = THREE.MathUtils.degToRad(180)
        this.controls.zoomSpeed = 1.5
        this.controls.rotateSpeed = 0.6

        this.lookAt(new THREE.Vector3(0, 0, 0))
    }

    resize()
    {
        this.instance.aspect = this.sizes.width / this.sizes.height
        this.instance.updateProjectionMatrix()
    }

    update()
    {
        this.controls.update()

        if (this.look_at.end_time > this.time.current)
        {
            const elapsed_time = this.time.current - this.look_at.start_time
            const t = Math.min(elapsed_time / this.look_at_speed_ms, 1)
            const look_at_position = this.look_at.current.lerp(this.look_at.target, t)
            this.#look_at_position(look_at_position)

            if (t > 1)
            {
                this.look_at.start_time = 0
                this.look_at.end_time = 0
            }
        }
    }

    lookAt(position)
    {
        this.look_at = {
            start_time: this.time.current,
            end_time: this.time.current + this.look_at_speed_ms,
            current: this.controls.target.clone(),
            target: position,
        }
    }

    // private methods
    #look_at_position(position)
    {
        this.instance.lookAt(position)
        this.controls.target.set(position.x, position.y, position.z)
    }
}

import * as THREE from "three"
import Experience from "../Experience.js"

export default class Environment
{
    constructor()
    {
        this.experience = new Experience()
        this.scene = this.experience.scene
        this.resources = this.experience.resources
        this.debug = this.experience.debug
        this.debug_object = {
            camera_size: 2,
        }

        // Debug
        if(this.debug.active)
        {
            this.debugFolder = this.debug.ui.addFolder("environment")
            this.debugFolder.close()
        }

        this.setSunLight()
        this.setBackground()
        // this.setEnvironmentMap()
    }

    setSunLight()
    {
        this.sunLight = new THREE.DirectionalLight("#ffffff", 4)
        this.sunLight.castShadow = true
        this.sunLight.shadow.camera.far = 30
        this.sunLight.shadow.mapSize.set(1024, 1024)
        this.sunLight.shadow.normalBias = 0.05
        this.sunLight.position.set(10, 6, 8)
        this.scene.add(this.sunLight)

        // show the camera for the light
        this.camera_helper = new THREE.CameraHelper(this.sunLight.shadow.camera)
        this.scene.add(this.camera_helper)
        this.camera_helper.visible = false

        this.change_camera_size(this.debug_object.camera_size)

        // Debug
        if(this.debug.active)
        {
            const sunlight_folder = this.debugFolder.addFolder("Sun light")

            sunlight_folder
                .add(this.sunLight, "intensity")
                .name("Intensity")
                .min(0)
                .max(10)
                .step(0.001)

            sunlight_folder
                .add(this.sunLight.position, "x")
                .name("X")
                .min(-5)
                .max(20)
                .step(0.001)

            sunlight_folder
                .add(this.sunLight.position, "y")
                .name("Y")
                .min(-5)
                .max(20)
                .step(0.001)

            sunlight_folder
                .add(this.sunLight.position, "z")
                .name("Z")
                .min(-5)
                .max(20)
                .step(0.001)

            sunlight_folder
                .add(this.debug_object, "camera_size")
                .name("Size")
                .min(1)
                .max(20)
                .step(0.1)
                .onChange(() =>
                {
                    this.change_camera_size(this.debug_object.camera_size)
                })

            sunlight_folder
                .add(this.camera_helper, "visible")
                .name("Toggle camera helper")
        }
    }

    change_camera_size (camera_size)
    {
        this.sunLight.shadow.camera.left = -camera_size
        this.sunLight.shadow.camera.right = camera_size
        this.sunLight.shadow.camera.top = -camera_size * 0.5
        this.sunLight.shadow.camera.bottom = camera_size * 0.5
        this.sunLight.shadow.camera.updateProjectionMatrix()
        this.camera_helper.update()
    }

    setBackground()
    {
        this.scene.background = new THREE.Color(0x95b6da); // Hexadecimal color value
        if (this.debug.active)
        {
            this.debug.ui.addColor(this.scene, "background").name("Background")
        }
        // this.
    }

    // setEnvironmentMap()
    // {
    //     this.environmentMap = {}
    //     this.environmentMap.intensity = 0.4
    //     this.environmentMap.texture = this.resources.items.environmentMapTexture
    //     this.environmentMap.texture.colorSpace = THREE.SRGBColorSpace

    //     this.scene.environment = this.environmentMap.texture

    //     this.environmentMap.updateMaterials = () =>
    //     {
    //         this.scene.traverse((child) =>
    //         {
    //             if(child instanceof THREE.Mesh && child.material instanceof THREE.MeshStandardMaterial)
    //             {
    //                 child.material.envMap = this.environmentMap.texture
    //                 child.material.envMapIntensity = this.environmentMap.intensity
    //                 child.material.needsUpdate = true
    //             }
    //         })
    //     }
    //     this.environmentMap.updateMaterials()

    //     // Debug
    //     if(this.debug.active)
    //     {
    //         this.debugFolder
    //             .add(this.environmentMap, "intensity")
    //             .name("envMapIntensity")
    //             .min(0)
    //             .max(4)
    //             .step(0.001)
    //             .onChange(this.environmentMap.updateMaterials)
    //     }
    // }
}

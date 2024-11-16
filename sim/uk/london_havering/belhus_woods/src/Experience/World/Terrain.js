import * as THREE from "three"
import Experience from "../Experience.js"
import { MESSAGES } from "../Utils/messages.js"
import EventEmitter from "../Utils/EventEmitter.js"

export default class Terrain extends EventEmitter
{
    constructor()
    {
        super()

        this.experience = new Experience()
        this.scene = this.experience.scene
        this.resources = this.experience.resources
        this.debug = this.experience.debug
        this.gui_debug_folder = this.debug.ui.addFolder("Terrain")
        // this.gui_debug_folder.close()
        this.debug_object = {
            use_standard_material: false,
        }
        this.custom_uniforms = {
            uTextureMap:       { value: null },
            uTerrainHeightMap: { value: null },
            uBumpScale:        { value: 3.5 },
            uTerrainColourMin:   { value: -0.05 },
            uTerrainColourRange: { value: 0.39 },
        }

        this.size = { x: 100, z: 100 }

        // Wait for resources
        this.resources.on(MESSAGES.Resources.ready, () =>
        {
            this.set_geometry()
            this.set_textures()
            this.set_material()
            this.set_mesh()
        })
    }

    set_geometry()
    {
        this.geometry = new THREE.PlaneGeometry(this.size.x, this.size.z, 1000, 1000)
    }

    set_textures()
    {
        this.textures = {}

        this.textures.colour = this.resources.items.satellite_texture_colour
        this.textures.colour.colorSpace = THREE.SRGBColorSpace
        this.textures.bump = this.resources.items.dtm_texture_height_map
        this.textures.bump.wrapS = this.textures.bump.wrapT = THREE.RepeatWrapping
        // this.textures.color.repeat.set(1.5, 1.5)
        // this.textures.color.wrapS = THREE.RepeatWrapping
        // this.textures.color.wrapT = THREE.RepeatWrapping

        // this.textures.normal = this.resources.items.grassNormalTexture
        // this.textures.normal.repeat.set(1.5, 1.5)
        // this.textures.normal.wrapS = THREE.RepeatWrapping
        // this.textures.normal.wrapT = THREE.RepeatWrapping
    }

    set_material()
    {
        const terrain_vertex_shader = this.resources.items.terrain_vertex_shader
        const terrain_fragment_shader = this.resources.items.terrain_fragment_shader

        this.custom_uniforms.uTextureMap.value = this.textures.colour
        this.custom_uniforms.uTerrainHeightMap.value = this.textures.bump

        const material_custom = new THREE.ShaderMaterial({
            uniforms: this.custom_uniforms,
            vertexShader: terrain_vertex_shader,
            fragmentShader: terrain_fragment_shader,
            transparent: true,
        })

        const material_standard = new THREE.MeshStandardMaterial({
            map: this.textures.colour,
        })

        const update_material = () => {
            this.material = this.debug_object.use_standard_material ? material_standard : material_custom
            if (this.mesh) this.mesh.material = this.material
        }
        update_material()

        if (this.gui_debug_folder)
        {
            this.gui_debug_folder.add(this.material, "wireframe").name("Wireframe")
            const gui_use_standard_material = this.gui_debug_folder
                .add(this.debug_object, "use_standard_material")
                .name("Standard material")
                .onChange(() => update_material())

            const use_shader_material = () =>
            {
                this.debug_object.use_standard_material = false
                gui_use_standard_material.updateDisplay()
                update_material()
            }

            this.gui_debug_folder
                .add(this.custom_uniforms.uBumpScale, "value")
                .min(2).max(60).step(0.1).name("Scale")
                .onChange(() => {
                    use_shader_material()
                    this.trigger(MESSAGES.Terrain.scale_changed)
                })
            this.gui_debug_folder
                .add(this.custom_uniforms.uTerrainColourMin, "value")
                .min(-0.1).max(0.1).step(0.001).name("Colour Min")
                .onChange(() => {
                    use_shader_material()
                })
            this.gui_debug_folder
                .add(this.custom_uniforms.uTerrainColourRange, "value")
                .min(0).max(1.0).step(0.01).name("Colour Range")
                .onChange(() => {
                    use_shader_material()
                })
        }
    }

    set_mesh()
    {
        this.mesh = new THREE.Mesh(this.geometry, this.material)
        this.mesh.rotation.x = - Math.PI * 0.5
        this.mesh.receiveShadow = true
        this.scene.add(this.mesh)

        // Set up a simple under mesh for capturing pointer events to move the
        // camera around
        this.simple_under_mesh = new THREE.Mesh(
            new THREE.PlaneGeometry(this.size.x * 3, this.size.z * 3),
            new THREE.MeshBasicMaterial({
                // wireframe: true,
                visible: false,
            })
        )
        this.simple_under_mesh.rotation.x = - Math.PI * 0.5
        this.scene.add(this.simple_under_mesh)
    }

    // calc_water_y(water_level)
    // {
    //     return ((water_level + 3) / 100) * this.custom_uniforms.uBumpScale.value
    // }
}

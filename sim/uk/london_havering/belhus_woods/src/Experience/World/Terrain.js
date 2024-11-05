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
        this.gui_debug_folder = this.debug.active && this.debug.ui.addFolder("Terrain")
        // this.gui_debug_folder.close()
        this.debug_object = {
            use_standard_material: false,
        }
        this.customUniforms = {
            uTextureMap:       { value: null },
            uTerrainHeightMap: { value: null },
            uBumpScale:        { value: 6.5 },
            uTerrainColourMin:   { value: -0.018 },
            uTerrainColourRange: { value: 0.52 },
        }

        this.size = { x: 100, z: 100 }

        // Wait for resources
        this.resources.on(MESSAGES.Resources.ready, () =>
        {
            this.setGeometry()
            this.setTextures()
            this.setMaterial()
            this.setMesh()
        })
    }

    setGeometry()
    {
        this.geometry = new THREE.PlaneGeometry(this.size.x, this.size.z, 1000, 1000)
    }

    setTextures()
    {
        this.textures = {}

        this.textures.colour = this.resources.items.satellite_v2b_texture_colour
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

    setMaterial()
    {
        const terrain_vertex_shader = this.resources.items.terrain_vertex_shader
        const terrain_fragment_shader = this.resources.items.terrain_fragment_shader

        this.customUniforms.uTextureMap.value = this.textures.colour
        this.customUniforms.uTerrainHeightMap.value = this.textures.bump

        const material_custom = new THREE.ShaderMaterial({
            uniforms: this.customUniforms,
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
                .add(this.customUniforms.uBumpScale, "value")
                .min(2).max(60).step(0.1).name("Scale")
                .onChange(() => {
                    use_shader_material()
                    this.trigger(MESSAGES.Terrain.scale_changed)
                })
            this.gui_debug_folder
                .add(this.customUniforms.uTerrainColourMin, "value")
                .min(-0.1).max(0.1).step(0.001).name("Colour Min")
                .onChange(() => {
                    use_shader_material()
                })
            this.gui_debug_folder
                .add(this.customUniforms.uTerrainColourRange, "value")
                .min(0).max(1.0).step(0.01).name("Colour Range")
                .onChange(() => {
                    use_shader_material()
                })
        }
    }

    setMesh()
    {
        this.mesh = new THREE.Mesh(this.geometry, this.material)
        this.mesh.rotation.x = - Math.PI * 0.5
        this.mesh.receiveShadow = true
        this.scene.add(this.mesh)
    }

    // calc_water_y(water_level)
    // {
    //     return ((water_level + 3) / 100) * this.customUniforms.uBumpScale.value
    // }
}

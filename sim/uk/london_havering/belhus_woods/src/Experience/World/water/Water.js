import * as THREE from "three"
import Experience from "../../Experience.js"
import { MESSAGES } from "../../Utils/messages.js"
import { get_minima_from_vertices, get_watershed_from_image_el } from "../../../watershed/src/watershed.js"


export default class Water
{
    constructor(terrain)
    {
        this.finised_initialising = false

        const experience = new Experience()
        this.resources = experience.resources
        this.terrain = terrain
        this.size = terrain.size

        this.experience = new Experience()
        this.scene = this.experience.scene
        this.debug = this.experience.debug

        this.gui_debug_folder = this.debug.active && this.debug.ui.addFolder("Water")
        // this.gui_debug_folder.close()
        this.debug_object = {
            use_standard_material: true,
        }

        this.customUniforms = {
            uHeightMap: { value: undefined },
            uBumpScale: { value: 1 }, // will get updated when we call onTerrainScaleChanged
            uTerrainHeightMap: { value: null },
            uHeightOffset: { value: -0.1959 },
            // uWaterColour: { value: new THREE.Vector3(37/255, 137/255, 185/255) }, // 0x2589b2
        }
        this.onTerrainScaleChanged() // update uBumpScale

        // Wait for resources
        this.resources.on(MESSAGES.Resources.ready, async () =>
        {
            await this.setGeometry()
            this.setTextures()
            this.setMaterial()
            this.setMesh()
            this.finised_initialising = true
        })

        this.terrain.on(MESSAGES.Terrain.scale_changed, this.onTerrainScaleChanged.bind(this))
    }

    async setGeometry()
    {
        const canvas_el = document.createElement("canvas")
        const max_z_diff = 2
        const magnify = Math.pow(2, -2)
        const watershed = await get_watershed_from_image_el(canvas_el, this.resources.items.dtm_texture_height_map.image, max_z_diff, magnify)

        const minima = get_minima_from_vertices(watershed.vertices, false)
        const map_minima_id_to_minima = {}
        minima.forEach(m => map_minima_id_to_minima[m.minimum_id] = m)

        const size = watershed.width * watershed.height
        const data = new Uint8Array(size)
        watershed.vertices.forEach((vertex, i) =>
        {
            const lowest_minimum = Math.min(...vertex.group_ids)
            const minima = map_minima_id_to_minima[lowest_minimum]
            // const z = vertex.z
            const z = minima.z

            const x_row = i % watershed.width
            const height_row = (watershed.height - Math.floor(i / watershed.width)) - 1
            const index = x_row + (height_row * watershed.width)
            data[index] = z //(z - 39) * (255 / (93 - 39))
        })

        const heightMapTexture = new THREE.DataTexture(data, watershed.width, watershed.height, THREE.RedFormat, THREE.UnsignedByteType)
        heightMapTexture.needsUpdate = true
        this.customUniforms.uHeightMap.value = heightMapTexture

        this.customUniforms.uTerrainHeightMap.value = this.resources.items.dtm_texture_height_map

        this.geometry = new THREE.PlaneGeometry(this.size.x, this.size.z, 1000, 1000)
    }

    setTextures()
    {
        this.textures = {}
        this.textures.colour = new THREE.Color(0x2589b2)
    }

    setMaterial()
    {
        const water_vertex_shader = this.resources.items.water_vertex_shader
        const water_fragment_shader = this.resources.items.water_fragment_shader

        // this.customUniforms.textureMap.value = this.textures.colour
        // this.customUniforms.bumpTexture.value = this.textures.bump

        this.material = new THREE.ShaderMaterial({
            uniforms: this.customUniforms,
            vertexShader: water_vertex_shader,
            fragmentShader: water_fragment_shader,
            transparent: true,
        })

        if (this.gui_debug_folder)
        {
            this.gui_debug_folder
                .add(this.customUniforms.uHeightOffset, "value")
                .min(-0.2).max(-0.15).step(0.0001).name("Height offset")

            // this.gui_debug_folder
            //     .add(this.customUniforms.uBumpScale2, "value")
            //     .min(1).max(150).step(0.1).name("Scale")
        }
    }

    setMesh()
    {
        this.mesh = new THREE.Mesh(this.geometry, this.material)
        this.mesh.rotation.x = - Math.PI * 0.5
        // this.setPosition(this.position)
        this.mesh.receiveShadow = true
        this.scene.add(this.mesh)
    }

    // changeSize(new_width, new_height)
    // {
    //     this.geometry.dispose()
    //     this.geometry = new THREE.PlaneGeometry(new_width, new_height, 10, 10)
    //     this.mesh.geometry = this.geometry
    // }

    // setPosition(new_position)
    // {
    //     this.position = new_position
    //     this.mesh.position.copy(new_position)
    //     this.mesh.position.y = this.terrain.calc_water_y(new_position.y)
    // }

    onTerrainScaleChanged()
    {
        this.customUniforms.uBumpScale.value = this.terrain.customUniforms.uBumpScale.value
    }
}

import * as THREE from "three"

import Experience from "../../Experience.js"
import { MESSAGES } from "../../Utils/messages.js"
import {
    construct_watershed,
    extract_image_data,
    get_minima_from_vertices,
} from "../../../watershed/src/watershed.js"


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

        this.custom_uniforms = {
            uHeightMap: { value: undefined },
            // Provided to shader so that we can hide the water that is underground
            uTerrainHeightMap: { value: null },
            uBumpScale: { value: 1 }, // will get updated when we call on_terrain_scale_change
            uHeightOffset: { value: 0 },
            // uWaterColour: { value: new THREE.Vector3(37/255, 137/255, 185/255) }, // 0x2589b2
        }
        this.on_terrain_scale_change() // update uBumpScale

        // Wait for resources
        this.resources.on(MESSAGES.Resources.ready, async () =>
        {
            this.set_geometry()
            this.set_textures()
            this.set_material()
            this.set_mesh()
            this.finised_initialising = true
        })

        this.terrain.on(MESSAGES.Terrain.scale_changed, this.on_terrain_scale_change.bind(this))
    }

    set_geometry()
    {
        const canvas_el = document.createElement("canvas")
        const max_z_diff = 9
        const magnify = Math.pow(2, -2)
        const img_el = this.resources.items.dtm_texture_height_map.image
        const watershed_input_data = extract_image_data(canvas_el, img_el, magnify)
        const watershed = construct_watershed(watershed_input_data, max_z_diff)

        const minima = get_minima_from_vertices(watershed.vertices, false)
        const map_minima_id_to_minima = {}
        minima.forEach(m => map_minima_id_to_minima[m.minimum_id] = m)

        const size = watershed.width * watershed.height
        const height_data = new Uint8Array(size)
        watershed.vertices.forEach((vertex, i) =>
        {
            const lowest_minimum = Math.min(...vertex.group_ids)
            const minima = map_minima_id_to_minima[lowest_minimum]
            // const z = vertex.z
            const z = minima.z

            const x_row = i % watershed.width
            const height_row = (watershed.height - Math.floor(i / watershed.width)) - 1
            const index = x_row + (height_row * watershed.width)
            height_data[index] = z //(z - 39) * (255 / (93 - 39))
        })

        const height_map_texture = new THREE.DataTexture(height_data, watershed.width, watershed.height, THREE.RedFormat, THREE.UnsignedByteType)
        height_map_texture.needsUpdate = true
        this.custom_uniforms.uHeightMap.value = height_map_texture

        this.custom_uniforms.uTerrainHeightMap.value = this.resources.items.dtm_texture_height_map

        this.geometry = new THREE.PlaneGeometry(this.size.x, this.size.z, 1000, 1000)
    }

    set_textures()
    {
        this.textures = {}
        this.textures.colour = new THREE.Color(0x2589b2)
    }

    set_material()
    {
        const water_vertex_shader = this.resources.items.water_vertex_shader
        const water_fragment_shader = this.resources.items.water_fragment_shader

        // this.custom_uniforms.textureMap.value = this.textures.colour
        // this.custom_uniforms.bumpTexture.value = this.textures.bump

        this.material = new THREE.ShaderMaterial({
            uniforms: this.custom_uniforms,
            vertexShader: water_vertex_shader,
            fragmentShader: water_fragment_shader,
            transparent: true,
        })

        if (this.gui_debug_folder)
        {
            this.gui_debug_folder
                .add(this.custom_uniforms.uHeightOffset, "value")
                .min(-0.2).max(1).step(0.01).name("Height offset")
        }
    }

    set_mesh()
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

    on_terrain_scale_change()
    {
        this.custom_uniforms.uBumpScale.value = this.terrain.custom_uniforms.uBumpScale.value
    }
}

import * as THREE from "three"

import Experience from "../../Experience.js"
import { TERRAIN_HEIGHT_MAP } from "../../constants.js"
import { MESSAGES } from "../../Utils/messages.js"
import {
    construct_watersheds,
    extract_image_data,
} from "../../../watershed/dist/watersheds.js"
import {
    calculate_watersheds_graph
} from "../../../watershed/dist/watersheds_graph/watersheds_graph.js"


const DEBUG_LAYER_HEIGHT_OFFSET = 0.5

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
        this.user_controls = this.experience.user_controls

        this.gui_debug_folder = this.debug.active && this.debug.ui.addFolder("Water")
        // this.gui_debug_folder.close()
        this.debug_object = {
            use_standard_material: true,
        }

        this.max_z_diff = 5
        this.water_custom_uniforms = {
            uMinWatershedHeightMap: { value: undefined },
            uMaxWatershedHeightMap: { value: undefined },
            // Provided to shader so that we can hide the water that is underground
            uTerrainHeightMap: { value: null },
            // uBumpScale will get updated when we call on_terrain_scale_change
            uBumpScale: new THREE.Uniform(),
            uHeightOffset: new THREE.Uniform(0.017),
            uHeightMinMaxMix: new THREE.Uniform(0),
            // uWaterColour: { value: new THREE.Vector3(37/255, 137/255, 185/255) }, // 0x2589b2
        }
        this.watershed_input_data_custom_uniforms = {
            uTerrainWatershedInputHeightMap: { value: undefined },
            // uBumpScale will get updated when we call on_terrain_scale_change
            uBumpScale: new THREE.Uniform(),
        }
        this.watershed_custom_uniforms = {
            uTerrainHeightMap: { value: undefined },
            // uBumpScale will get updated when we call on_terrain_scale_change
            uBumpScale: new THREE.Uniform(),
            uVerticesByWatershedGroupMap: { value: undefined },
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

            // // temporary
            // this.render_watersheds()
        })

        this.terrain.on(MESSAGES.Terrain.scale_changed, this.on_terrain_scale_change.bind(this))
        this.user_controls.on(MESSAGES.UserControls.terrain_height_map, (value) =>
        {
            if (value === TERRAIN_HEIGHT_MAP.height)
            {
                this.render_watershed_input_data()
            }
            else if (value === TERRAIN_HEIGHT_MAP.watersheds)
            {
                this.hide_watershed_input_data()
                this.render_watersheds()
            }
            else
            {
                this.hide_watersheds()
            }
        })
    }

    set_geometry()
    {
        const img_el = this.resources.items.dtm_texture_height_map.image
        this.calc_geometry_initial_height_data(img_el)
        this.set_height_map_texture(this.intial_height_data_args)

        this.water_custom_uniforms.uTerrainHeightMap = new THREE.Uniform(this.resources.items.dtm_texture_height_map)
        this.watershed_custom_uniforms.uTerrainHeightMap = new THREE.Uniform(this.resources.items.dtm_texture_height_map)

        this.geometry = new THREE.PlaneGeometry(
            this.size.x,
            this.size.z,
            1000, //height_map_texture.source.data.width,
            1000, //height_map_texture.source.data.height,
        )
    }

    calc_geometry_initial_height_data(img_el)
    {
        const canvas_el = document.createElement("canvas")
        const magnify = Math.pow(2, -2)
        this.initial_watershed_input_data = extract_image_data(canvas_el, img_el, magnify, true, true)

        this.intial_height_data_args = this.calc_height_data_args_from_input_data(this.initial_watershed_input_data)
    }

    calc_height_data_args_from_input_data(watershed_input_data)
    {
        const watersheds = construct_watersheds(watershed_input_data, this.max_z_diff)
        const exits = [{ x: 0, y: watersheds.input_height/2, z: 111 }]
        const watersheds_graph = calculate_watersheds_graph(watersheds, exits)
        // // We might want to get the highest minimum for vertices that
        // // are members of multiple watersheds as perhaps the water level of
        // // these boundary vertices will more likely be determined by the
        // // minimum of the higher watershed as this will likely also be the
        // // minimum which is closest to that vertex.
        // const get_lowest_minimum_for_vertex = false
        // const get_minimum_for_vertex = factory_get_minimum_for_vertex(watershed.vertices, get_lowest_minimum_for_vertex)

        const size = watersheds.input_width * watersheds.input_height
        const min_watershed_height_data = new Uint8Array(size)
        const max_watershed_height_data = new Uint8Array(size)
        watersheds.vertices.forEach((vertex, i) =>
        {
            const heights_info = watersheds_graph.get_watershed_heights_info(vertex)
            min_watershed_height_data[i] = heights_info.ground_z
            max_watershed_height_data[i] = heights_info.max_z
        })

        return {
            min_watershed_height_data,
            max_watershed_height_data,
            size: {
                width: watersheds.input_width,
                height: watersheds.input_height,
            }
        }
    }

    set_height_map_texture(height_data_args)
    {
        this.water_custom_uniforms.uMinWatershedHeightMap.value?.dispose()
        this.water_custom_uniforms.uMaxWatershedHeightMap.value?.dispose()

        const min_watershed_height_map_texture = new THREE.DataTexture(
            height_data_args.min_watershed_height_data,
            height_data_args.size.width,
            height_data_args.size.height,
            THREE.RedFormat,
            THREE.UnsignedByteType,
        )

        const max_watershed_height_map_texture = new THREE.DataTexture(
            height_data_args.max_watershed_height_data,
            height_data_args.size.width,
            height_data_args.size.height,
            THREE.RedFormat,
            THREE.UnsignedByteType,
        )

        min_watershed_height_map_texture.needsUpdate = true
        max_watershed_height_map_texture.needsUpdate = true

        this.water_custom_uniforms.uMinWatershedHeightMap.value = min_watershed_height_map_texture
        this.water_custom_uniforms.uMaxWatershedHeightMap.value = max_watershed_height_map_texture
    }

    // This function also allows us to reset the height map data to the initial
    // state
    reset_height_map_texture_to_initial()
    {
        this.modified_watershed_input_data = null
        this.set_height_map_texture(this.intial_height_data_args)
    }

    alter_height_map_texture(modifier)
    {
        const modified_watershed_input_data = {
            ...(this.modified_watershed_input_data || this.initial_watershed_input_data)
        }
        modified_watershed_input_data.image_data = new Uint8ClampedArray(modified_watershed_input_data.image_data)

        this.modified_watershed_input_data = modifier(modified_watershed_input_data)
        const height_data_args = this.calc_height_data_args_from_input_data(this.modified_watershed_input_data)
        this.set_height_map_texture(height_data_args)
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
            uniforms: this.water_custom_uniforms,
            vertexShader: water_vertex_shader,
            fragmentShader: water_fragment_shader,
            transparent: true,
        })
        // this.material = new THREE.MeshStandardMaterial({
        //     wireframe: true,
        // })

        if (this.gui_debug_folder)
        {
            this.gui_debug_folder
                .add(this.water_custom_uniforms.uHeightOffset, "value")
                .min(-0.1).max(0.1).step(0.001).name("Height offset")

            this.gui_debug_folder
                .add(this.water_custom_uniforms.uHeightMinMaxMix, "value")
                .min(-0.15).max(0.5).step(0.01).name("Height min max mix")
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
        const new_value = this.terrain.custom_uniforms.uBumpScale.value
        this.water_custom_uniforms.uBumpScale.value = new_value
        this.watershed_input_data_custom_uniforms.uBumpScale.value = new_value
        this.watershed_custom_uniforms.uBumpScale.value = new_value
    }

    render_watershed_input_data()
    {
        this.hide_watershed_input_data()

        const terrain_height_map_vertex_shader = this.resources.items.terrain_height_map_vertex_shader
        const terrain_height_map_fragment_shader = this.resources.items.terrain_height_map_fragment_shader

        const watershed_input_data = (this.modified_watershed_input_data || this.initial_watershed_input_data)
        const terrain_watershed_input_height_map_texture = new THREE.DataTexture(
            watershed_input_data.image_data,
            watershed_input_data.width,
            watershed_input_data.height,
            THREE.RedFormat,
            THREE.UnsignedByteType,
        )
        terrain_watershed_input_height_map_texture.needsUpdate = true
        this.watershed_input_data_custom_uniforms.uTerrainWatershedInputHeightMap = new THREE.Uniform(terrain_watershed_input_height_map_texture)

        this.watershed_input_data_mesh = new THREE.Mesh(
            new THREE.PlaneGeometry(
                this.size.x,
                this.size.z,
                1000,
                1000,
            ),
            new THREE.ShaderMaterial({
                uniforms: this.watershed_input_data_custom_uniforms,
                vertexShader: terrain_height_map_vertex_shader,
                fragmentShader: terrain_height_map_fragment_shader,
                transparent: true,
            }),
        )

        this.watershed_input_data_mesh.position.y = DEBUG_LAYER_HEIGHT_OFFSET
        this.watershed_input_data_mesh.rotation.x = - Math.PI * 0.5
        this.scene.add(this.watershed_input_data_mesh)
    }

    hide_watershed_input_data()
    {
        if (this.watershed_input_data_mesh)
        {
            this.watershed_input_data_mesh.geometry.dispose()
            this.watershed_input_data_mesh.material.dispose()
            this.scene.remove(this.watershed_input_data_mesh)
        }
    }

    render_watersheds()
    {
        this.hide_watersheds()

        const watersheds_vertex_shader = this.resources.items.watersheds_vertex_shader
        const watersheds_fragment_shader = this.resources.items.watersheds_fragment_shader

        const watershed_input_data = (this.modified_watershed_input_data || this.initial_watershed_input_data)

        const watershed = construct_watersheds(watershed_input_data, this.max_z_diff)
        // const minima = get_minima_from_vertices(watershed.vertices, false)
        // const map_minima_id_to_minima = {}
        // minima.forEach(m => map_minima_id_to_minima[m.minimum_id] = m)

        const size = watershed.input_width * watershed.input_height
        const vertices_by_watershed = new Uint8Array(size * 4)
        watershed.vertices.forEach((vertex, i) =>
        {
            const stride = i * 4
            vertices_by_watershed[stride + 3] = 180

            if (vertex.watershed_ids.size > 1)
            {
                vertices_by_watershed[stride    ] = 255
                vertices_by_watershed[stride + 1] = 255
                vertices_by_watershed[stride + 2] = 255
                return
            }

            const id = [...vertex.watershed_ids][0]
            const colour = colour_for_watershed_id(id, watershed.watershed_count)

            vertices_by_watershed[stride    ] = colour.r
            vertices_by_watershed[stride + 1] = colour.g
            vertices_by_watershed[stride + 2] = colour.b
        })

        const vertices_by_watershed_group_texture = new THREE.DataTexture(
            vertices_by_watershed,
            watershed_input_data.width,
            watershed_input_data.height,
            THREE.RGBAFormat,
            THREE.UnsignedByteType,
        )
        // const vertices_by_watershed_group_texture = new THREE.DataTexture(
        //     new Uint8Array([255, 50, 255, 255]),
        //     1,
        //     1,
        //     THREE.RGBAFormat,
        //     THREE.UnsignedByteType,
        // )
        vertices_by_watershed_group_texture.needsUpdate = true
        this.watershed_custom_uniforms.uVerticesByWatershedGroupMap = new THREE.Uniform(vertices_by_watershed_group_texture)

        this.watersheds_mesh = new THREE.Mesh(
            new THREE.PlaneGeometry(
                this.size.x,
                this.size.z,
                1000,
                1000,
            ),
            new THREE.ShaderMaterial({
                uniforms: this.watershed_custom_uniforms,
                vertexShader: watersheds_vertex_shader,
                fragmentShader: watersheds_fragment_shader,
                transparent: true,
            }),
        )

        this.watersheds_mesh.position.y = DEBUG_LAYER_HEIGHT_OFFSET
        this.watersheds_mesh.rotation.x = - Math.PI * 0.5
        this.scene.add(this.watersheds_mesh)
    }

    hide_watersheds()
    {
        if (this.watersheds_mesh)
        {
            this.watersheds_mesh.geometry.dispose()
            this.watersheds_mesh.material.dispose()
            this.scene.remove(this.watersheds_mesh)
        }
    }
}


function colour_for_watershed_id(watershed_id, total_watersheds)
{
    const hue = (watershed_id / total_watersheds) * Math.PI * 2
    const r = Math.sin(hue) * 127 + 128
    const g = Math.sin(hue + Math.PI * 2 / 3) * 127 + 128
    const b = Math.sin(hue + Math.PI * 4 / 3) * 127 + 128

    return new THREE.Color(r, g, b)
}

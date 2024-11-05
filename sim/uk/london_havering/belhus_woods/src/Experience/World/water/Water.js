import * as THREE from "three"
import Experience from "../../Experience.js"
import { MESSAGES } from "../../Utils/messages.js"


export default class Water
{
    constructor(terrain)
    {
        this.finised_initialising = false

        const experience = new Experience()
        this.resources = experience.resources
        this.terrain = terrain
        this.size = terrain.size
        // this.position = new THREE.Vector3()
        // this.size = size

        this.experience = new Experience()
        this.scene = this.experience.scene
        this.debug = this.experience.debug
        // this.gui_debug_folder = this.debug.active && this.debug.ui.addFolder("Terrain")
        // this.gui_debug_folder.close()
        // this.debug_object = {
        //     use_standard_material: true,
        // }
        this.customUniforms = {
            uYBase: { value: 0 },
            uWaterColour: { value: new THREE.Vector3(37/255, 137/255, 185/255) }, // 0x2589b2
        }
        this.onTerrainScaleChanged() // update uYBase

        // Wait for resources
        this.resources.on(MESSAGES.Resources.ready, () =>
        {
            this.setGeometry()
            this.setTextures()
            this.setMaterial()
            this.setMesh()
            this.finised_initialising = true
        })

        this.terrain.on(MESSAGES.Terrain.scale_changed, this.onTerrainScaleChanged.bind(this))
    }

    setGeometry()
    {
        this.geometry = new THREE.PlaneGeometry(this.size.x, this.size.z, 10, 10)
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

        // this.material = new THREE.MeshStandardMaterial({
        //     color: this.textures.colour,
        //     opacity: 0.9,
        //     transparent: true,
        // })
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
        this.customUniforms.uYBase.value = this.terrain.calc_water_y(0)
    }
}

import * as THREE from "three"
import Experience from "../../Experience.js"
import { MESSAGES } from "../../Utils/messages.js"

export default class Water
{
    constructor({ terrain, position, size })
    {
        this.terrain = terrain
        this.position = position
        this.size = size

        this.experience = new Experience()
        this.scene = this.experience.scene
        this.debug = this.experience.debug
        // this.gui_debug_folder = this.debug.active && this.debug.ui.addFolder("Terrain")
        // this.gui_debug_folder.close()
        // this.debug_object = {
        //     use_standard_material: true,
        // }

        this.setGeometry()
        this.setTextures()
        this.setMaterial()
        this.setMesh()

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
        this.material = new THREE.MeshStandardMaterial({
            color: this.textures.colour,
            opacity: 0.9,
            transparent: true,
        })
    }

    setMesh()
    {
        this.mesh = new THREE.Mesh(this.geometry, this.material)
        this.mesh.rotation.x = - Math.PI * 0.5
        this.setPosition(this.position)
        this.mesh.receiveShadow = true
        this.scene.add(this.mesh)
    }

    changeSize(new_width, new_height)
    {
        this.geometry.dispose()
        this.geometry = new THREE.PlaneGeometry(new_width, new_height, 10, 10)
        this.mesh.geometry = this.geometry
    }

    setPosition(new_position)
    {
        this.position = new_position
        this.mesh.position.copy(new_position)
        this.mesh.position.y = this.terrain.calc_water_y(new_position.y)
    }

    onTerrainScaleChanged()
    {
        this.mesh.position.y = this.terrain.calc_water_y(this.position.y)
    }
}

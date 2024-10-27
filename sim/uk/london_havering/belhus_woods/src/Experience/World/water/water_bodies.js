import * as THREE from "three"

import Water from "./Water.js"


export const WATER_IDS = {
    TOP_LAKE: "top_lake",
    DAM_LAKE: "dam_lake",
}

export function make_water_bodies(experience, terrain)
{
    const debug_folder = experience.debug.active && experience.debug.ui.addFolder("Water bodies")

    const water_bodies = []

    const water_body_args_list = [
        { id: WATER_IDS.TOP_LAKE, position: { x: 37.5, z: -26, y: -2.01 }, size: { x: 25, z: 20 } },
        { id: WATER_IDS.DAM_LAKE, position: { x: 0, z: 11.5, y: -11.31 }, size: { x: 100, z: 36 } },
        // { x: 0, z: 10, y: 10 },
        // { x: 0, z: -10, y: 10 },
        // { x: 10, z: 0, y: 10 },
        // { x: -10, z: 0, y: 10 },
    ]

    water_body_args_list.forEach(water_body_args =>
    {
        const name = water_body_args.id.replace("_", " ")

        const water_body = new Water({ ...water_body_args, terrain })
        water_bodies.push(water_body)

        const debug_object = {
            look_at: () =>
            {
                experience.camera.lookAt(water_body_args.position)
            },
            width_x: water_body_args.size.x,
            width_z: water_body_args.size.z,
            x: water_body_args.position.x,
            y: water_body_args.position.y,
            z: water_body_args.position.z,
        }
        const update_size = () => water_body.changeSize(debug_object.width_x, debug_object.width_z)
        const update_position = () => water_body.setPosition(new THREE.Vector3(debug_object.x, debug_object.y, debug_object.z))

        if (!debug_folder) return
        debug_folder.add(debug_object, "look_at").name(name)
        debug_folder
            .add(debug_object, "width_x")
            .min(10)
            .max(100)
            .step(1)
            .name(`${name} width (x)`)
            .onChange(update_size)
        debug_folder
            .add(debug_object, "width_z")
            .min(10)
            .max(100)
            .step(1)
            .name(`${name} width (z)`)
            .onChange(update_size)
        debug_folder
            .add(debug_object, "x")
            .min(-100)
            .max(100)
            .step(0.5)
            .name(`${name} x`)
            .onChange(update_position)
        debug_folder
            .add(debug_object, "y")
            .min(-20)
            .max(0)
            .step(0.01)
            .name(`${name} y`)
            .onChange(update_position)
        debug_folder
            .add(debug_object, "z")
            .min(-100)
            .max(100)
            .step(0.5)
            .name(`${name} z`)
            .onChange(update_position)
    })

    return {
        destroy: () =>
        {
            water_bodies.forEach(water_body => water_body.destroy())
        }
    }
}

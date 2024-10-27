import Experience from "../Experience.js"
import Environment from "./Environment.js"
import Terrain from "./Terrain.js"
import Fox from "./Fox.js"
import { make_water_bodies } from "./water/water_bodies.js"

export default class World
{
    constructor()
    {
        this.experience = new Experience()
        this.scene = this.experience.scene

        this.terrain = new Terrain()
        this.fox = new Fox()
        this.environment = new Environment()
        this.water_bodies = make_water_bodies(this.experience, this.terrain)
    }

    update()
    {
        this.fox?.update()
    }
}

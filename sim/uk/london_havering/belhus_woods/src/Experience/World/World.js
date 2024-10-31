import Experience from "../Experience.js"
import Environment from "./Environment.js"
import Terrain from "./Terrain.js"
import Fox from "./Fox.js"
import Water from "./water/Water.js"

export default class World
{
    constructor()
    {
        this.experience = new Experience()
        this.scene = this.experience.scene

        this.terrain = new Terrain()
        this.fox = new Fox()
        this.environment = new Environment()
        this.water = new Water(this.terrain)
    }

    update()
    {
        this.fox?.update()
    }
}

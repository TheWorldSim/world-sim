import { TimeUnitsAll } from "simulation"

export interface ModelConfig2
{
    timeStart: number
    timeLength: number
    timeUnits: TimeUnitsAll
}


export interface ModelVariableConfig2
{
    name: string
    value: number | boolean | string
    units?: string
    note?: string
}


export type ValueByWComponentId = {[wcomponent_id: string]: number}


export class Model2
{
    config: ModelConfig2
    state: ValueByWComponentId
    current_time: number
    constructor (config: ModelConfig2, model_state: ValueByWComponentId = {})
    {
        this.config = config
        this.current_time = config.timeStart
        this.state = model_state
    }

    // replace_all_state(model_state: ValueByWComponentId)
    // {
    //     this.state = model_state
    // }

    update_partial_state (model_state: ValueByWComponentId)
    {
        this.state = { ...this.state, ...model_state }
    }

    create_state_component (config: ModelVariableConfig2): SimulationComponent2 {
        return new SimulationComponent2(config)
    }

    simulate (): SimulationResult2 {
        return {
            state: this.state,
            time: this.current_time,
            time_units: this.config.timeUnits
        }
    }
}

export class SimulationComponent2
{
    config: ModelVariableConfig2
    constructor (config: ModelVariableConfig2)
    {
        this.config = config
    }
}

export interface SimulationResult2
{
    state: ValueByWComponentId
    time: number
    time_units: TimeUnitsAll
    note?: string
    error?: string
}


export function make_model_v2 (model_state: ValueByWComponentId, time_start: number, time_step: number, time_step_units: TimeUnitsAll)
{
    const model = new Model2({
        timeStart: time_start,
        timeLength: time_step,
        timeUnits: time_step_units,
    }, model_state)

    return model
}


function euler_solver (x: number, v: number, a: number, dt: number): [number, number] {
    let x_ = x + v * dt + 0.5 * a * dt * dt
    let v_ = v + a * dt
    return [x_, v_]
}

export function demonstrate_euler_solver () {
    let i = 0
    let x = 0
    let v = 0
    const a = 1
    const dt = 0.1
    console.log(x, v)
    while (i < 10) {
        let [x_, v_] = euler_solver(x, v, a, dt)
        console.log(x_, v_)
        x = x_
        v = v_
        i++
    }
}

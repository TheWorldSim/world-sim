import { TimeUnitsAll } from "simulation"

import { PlainCalculationObject } from "../data_curator/src/calculations/interfaces"
import { only_double_at_mentioned_uuids_regex } from "../data_curator/src/sharedf/rich_text/id_regexs"
import { wcomponent_is_action, wcomponent_is_statev2, WComponentNode } from "../data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { get_wcomponent_state_value_and_probabilities } from "../data_curator/src/wcomponent_derived/get_wcomponent_state_value_and_probabilities"


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
type ValueBySomeId = {[some_id: string]: number}


export class Model2
{
    config: ModelConfig2
    state: ValueByWComponentId
    current_time: number

    // subscribers_by_wcomponent_id: {[wcomponent_id: string]: ((value: number) => void)[]} = {}
    subscribers: ((value: ValueByWComponentId) => void)[] = []

    constructor (config: ModelConfig2, wcomponents: WComponentNode[])
    {
        this.config = config
        this.current_time = config.timeStart

        const model_state = build_model_state_from_wcomponents(wcomponents)
        this.state = model_state

        set_up_calculation_functions(wcomponents)
    }

    // replace_all_state(model_state: ValueByWComponentId)
    // {
    //     this.state = model_state
    // }

    // subscribe_to_state_change (wcomponent_id: string, subscriber: (value: number) => void)
    // {
    //     if (!this.subscribers_by_wcomponent_id[wcomponent_id]) this.subscribers_by_wcomponent_id[wcomponent_id] = []
    //     this.subscribers_by_wcomponent_id[wcomponent_id].push(subscriber)

    //     return () => {
    //         this.subscribers_by_wcomponent_id[wcomponent_id] = this.subscribers_by_wcomponent_id[wcomponent_id]!.filter(sub => sub !== subscriber)
    //     }
    // }

    on_state_change (subscriber: (state: ValueByWComponentId) => void)
    {
        this.subscribers.push(subscriber)

        return () => {
            this.subscribers = this.subscribers!.filter(sub => sub !== subscriber)
        }
    }

    update_partial_state (new_model_state: ValueByWComponentId)
    {
        this.state = { ...this.state, ...new_model_state }
        this.subscribers.forEach(subscriber => subscriber(this.state))
        // Object.entries(model_state).forEach(([key, value]) =>
        // {
        //     this.subscribers_by_wcomponent_id[key]?.forEach(subscriber => subscriber(value))
        // })
    }

    create_state_component (config: ModelVariableConfig2): SimulationComponent2 {
        return new SimulationComponent2(config)
    }

    apply_calculations(calculations: PlainCalculationObject[])
    {
        let state_for_calculations: ValueBySomeId = { ...this.state }

        calculations.forEach(calculation => {
            const result = apply_calculation(state_for_calculations, calculation)
            console.log("result", result)
            Object.entries(result).forEach(([key, value]) => {
                // if (state_for_calculations[key] !== undefined) state_for_calculations[key] += value
                // else state_for_calculations[key] = value

                // TODO: run validation methods to protect against invalid values
                state_for_calculations[key] = value
            })
        })
        const state_to_apply = filter_state_by_known_keys(state_for_calculations, Object.keys(this.state))
        this.update_partial_state(state_to_apply)
    }

    simulate (): SimulationResult2 {
        return {
            state: this.state,
            time: this.current_time,
            time_units: this.config.timeUnits
        }
    }
}


function build_model_state_from_wcomponents (wcomponents: WComponentNode[])
{
    const created_at_ms = new Date().getTime()
    const sim_ms = new Date().getTime()
    const value_by_wcomponent_id: ValueByWComponentId = {}

    wcomponents.forEach(wcomponent =>
    {
        if (wcomponent_is_statev2(wcomponent))
        {
            const most_probable_initial_values = get_wcomponent_state_value_and_probabilities({ wcomponent, VAP_set_id_to_counterfactual_v2_map: undefined, created_at_ms, sim_ms })
            const most_probable_initial_value = most_probable_initial_values.most_probable_VAP_set_values[0]
            let initial_value = most_probable_initial_value && most_probable_initial_value.parsed_value
            if (initial_value === undefined || typeof initial_value !== "number")
            {
                initial_value = 0
            }

            value_by_wcomponent_id[wcomponent.id] = initial_value
        }
    })

    return value_by_wcomponent_id
}



function set_up_calculation_functions (wcomponents: WComponentNode[])
{
    const calculation_functions: CalculationFunctions = {}
    ;(window as any).calculation_functions = calculation_functions

    document.getElementById("calculation_functions")?.remove()

    const new_script = document.createElement("script")
    new_script.type = "text/javascript"
    new_script.id = "calculation_functions"
    new_script.text = `(function () {\n`

    const wcomponent_actions = wcomponents.filter(wcomponent_is_action)

    wcomponent_actions.map(action => {
        ;(action.calculations || []).map(calculation => {
            const calculation_str = calculation.value
            const calculation_func = convert_calculation_string_to_code(calculation_str)

            const escaped_calculation_str = calculation_str.replace(/\\/g, '\\\\')

            new_script.text += `
            window.calculation_functions["${escaped_calculation_str}"] = function (state) {
                ${calculation_func}
            }
            `
        })
    })

    new_script.text += `\n})()`
    document.body.appendChild(new_script)
}


function convert_calculation_string_to_code (calculation_str: string)
{
    const hard_coded: {[calculation_str: string]: string} = {
        "@@17edbf36-ad5b-4936-b3c5-7d803741c678 + 1": `return state["17edbf36-ad5b-4936-b3c5-7d803741c678"] + 1`,
        "If @@17edbf36-ad5b-4936-b3c5-7d803741c678 > 0 Then \\n  1\\nEnd If": `
            let value = 0
            if (state["17edbf36-ad5b-4936-b3c5-7d803741c678"] > 0)
            {
                value = 1
            }
            return value
        `,
        "@@b644e33f-c00f-4a50-acc4-f158e4e11be5 + [Stock A available]": `return state["b644e33f-c00f-4a50-acc4-f158e4e11be5"] + state["Stock A available"]`,
        "@@17edbf36-ad5b-4936-b3c5-7d803741c678 - [Stock A available]": `return state["17edbf36-ad5b-4936-b3c5-7d803741c678"] - state["Stock A available"]`,
    }

    const calculation_func = hard_coded[calculation_str]
    if (!calculation_func) throw new Error(`Calculation function not found for calculation during conversion: ${calculation_str}`)
    return calculation_func
}


function apply_calculation (initial_state_for_calculation: ValueBySomeId, calculation: PlainCalculationObject)
{
    const { name, value: calculation_str } = calculation
    const value = run_calculation(calculation_str, initial_state_for_calculation)

    let id = name.trim()
    const matches = only_double_at_mentioned_uuids_regex.exec(id)
    if (matches) id = matches[1]!.slice(2) // slice removes initial @@

    const final_state_for_calculation: ValueBySomeId = {[id]: value}

    return final_state_for_calculation
}

interface CalculationFunctions
{
    [calculation_str: string]: (state: ValueBySomeId) => number
}

function run_calculation (calculation_str: string, state: ValueBySomeId): number
{
    const calc_func = ((window as any).calculation_functions as CalculationFunctions)[calculation_str]
    if (!calc_func)
    {
        throw new Error(`Calculation function not found for calculation during running: ${calculation_str}`)
    }
    const value = calc_func(state)
    return value
}


function filter_state_by_known_keys (state: ValueBySomeId, keys: string[]): ValueBySomeId
{
    const filtered_state: ValueBySomeId = {}
    keys.forEach(key => {
        if (state[key] !== undefined)
        {
            filtered_state[key] = state[key]
        }
    })
    return filtered_state
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

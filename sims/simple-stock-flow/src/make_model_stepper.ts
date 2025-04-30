import {
    Model,
    ModelConfig,
    ModelStockConfig,
    ModelVariableConfig,
    OnPauseSimulationArg,
    Primitive,
    SimulationComponent,
    SimulationResult,
    TimeUnitsAll,
} from "simulation"

import { GetItemsReturn } from "./data_curator/src/state/sync/supabase/get_items"
import { SimplifiedWComponentsValueById } from "./data/get_wcomponents_values_by_id"
import { get_uuids_from_text, is_valid_uuid } from "./data_curator/src/sharedf/rich_text/id_regexs"
import { is_number } from "./utils/number"



// Extra fields that we add for convenience
interface SimulationComponentExtra extends SimulationComponent
{
    name: string
}


if (!Model.toString().match(/.*async simulateAsync\(.*/mg))
{
    const error_msg = `Model requires simulateAsync method.

    Please:
    1.  stop the vite server.
    2.  delete: rm -rf node_modules/.vite
    3.  open: node_modules/simulation/src and copy accross the relevant files
    `
    throw new Error(error_msg)
}


export type ModelValue = number | string | "True" | "False"
export interface ModelValues
{
    [component_id: string]: ModelValue
}
export interface SimulationStepResult
{
    values: ModelValues
    current_time: number
    current_step: number
    set_value?: (cell: Primitive | SimulationComponent, value: number) => void
}

export interface AddStockArgs extends Omit<ModelStockConfig, "name">
{
    wcomponent_id: string
    title?: string
    initial: ModelValue
}

export interface AddVariableArgs extends Omit<ModelVariableConfig, "name">
{
    wcomponent_id: string
    title?: string
    linked_ids?: string[]
}

export interface AddFlowArgs
{
    wcomponent_id: string
    title?: string
    flow_rate: string | number
    only_positive?: boolean
    from_id: string | undefined
    to_id: string | undefined
    linked_ids?: string[]
}


export interface AddActionArgs
{
    wcomponent_id: string
    title?: string
    action: string
    trigger_value: string
    linked_ids?: string[]
}


export function make_wrapped_model (
    args: { target_refresh_rate: number, data?: GetItemsReturn<SimplifiedWComponentsValueById> },
    model_config: ModelConfig = {},
): WrappedModel
{
    const { target_refresh_rate, data } = args
    const time_start = 2020

    const simulation_time_step = 1 / target_refresh_rate
    const time_units = "Seconds"

    const time_length = Math.round(1e5 / target_refresh_rate)

    const all_model_config: ModelConfigStrict = {
        timeStart: time_start,
        timeStep: simulation_time_step,
        timeLength: time_length,
        timeUnits: time_units,
        timePause: simulation_time_step,
        ...model_config,
    }

    const wrapped_model = _make_wrapped_model(all_model_config, { target_refresh_rate })

    add_model_components(wrapped_model, data)

    return wrapped_model
}


interface ModelConfigStrict
{
    timeStart: number
    timeStep: number
    timeLength?: number
    timeUnits?: TimeUnitsAll
    timePause?: number
}


export interface ExtendedSimulationResult extends SimulationResult, SimulationStepResult {}


export type OnSimulationStepCompletedFunction = (step_result: SimulationStepResult) => { reason_to_stop: string } | undefined
export type OnSimulationCompletedFunction = (simulation_result: ExtendedSimulationResult) => void
export type WrappedModel = ReturnType<typeof _make_wrapped_model>

function _make_wrapped_model (model_config: ModelConfigStrict, run_sim_config: { target_refresh_rate: number })
{
    const model = new Model(model_config)
    const target_refresh_rate = run_sim_config.target_refresh_rate

    let model_simulation_started = false
    let simulation_cancelled = false
    let current_simulation_step = 0

    const map_between_model_and_wcomponent_id: {[id: string]: string} = {}
    const map_between_wcomponent_and_model_id: {[id: string]: string} = {}
    function add_ids_to_map (model_id: string, wcomponent_id: string)
    {
        if (!model_id.match(/^\d+$/)) throw new Error(`Model id "${model_id}" is not a number`)
        if (!is_valid_uuid(wcomponent_id, true)) throw new Error(`Wcomponent id "${wcomponent_id}" is not a valid uuid`)
        map_between_model_and_wcomponent_id[model_id] = wcomponent_id
        map_between_wcomponent_and_model_id[wcomponent_id] = model_id
    }

    let latest_model_results: SimulationStepResult = {
        values: {},
        current_time: model_config.timeStart,
        current_step: 0,
        set_value: undefined,
    }

    // const actions_by_id: {[action_id: string]: number} = {}


    function get_id_as_model_id (id: string | undefined, throw_on_missing?: false): string | undefined
    function get_id_as_model_id (id: string | undefined, throw_on_missing: true): string
    function get_id_as_model_id (id: string | undefined, throw_on_missing?: boolean): string | undefined
    {
        if (!id) return undefined

        const model_id = map_between_wcomponent_and_model_id[id]
        if (model_id) return model_id

        const wcomponent_id = map_between_model_and_wcomponent_id[id]
        if (wcomponent_id) return id

        if (throw_on_missing) throw new Error(`No model id (or wcomponent id) found for id: "${id}"`)
        return ""
    }


    function get_node_from_id (id: string | undefined, throw_on_missing?: false): SimulationComponent | undefined
    function get_node_from_id (id: string | undefined, throw_on_missing: true): SimulationComponent
    function get_node_from_id (id: string | undefined, throw_on_missing?: boolean): SimulationComponent | undefined
    {
        let node: SimulationComponent | undefined = undefined
        const model_id = get_id_as_model_id(id)
        if (model_id) node = model.getId(model_id) || undefined

        if (!node && throw_on_missing) throw new Error(`Node not found for id: "${id}"`)

        return node
    }


    function add_stock (args: AddStockArgs): SimulationComponentExtra
    {
        const { wcomponent_id, title, initial } = args

        const stock = model.Stock({
            name: wcomponent_id,
            initial,
            note: `${title || "no human readable name"}: ${wcomponent_id}`,
        })
        add_ids_to_map(stock._node.id, wcomponent_id)

        latest_model_results.values[stock._node.id] = initial
        latest_model_results.values[wcomponent_id] = initial

        // Add extra fields
        const stock_extra: SimulationComponentExtra = {
            ...stock,
            name: wcomponent_id,
        }

        console.debug(`added stock ${stock_extra.name} with id: ${stock_extra._node.id} initial: ${initial}`)

        return stock_extra
    }


    function add_variable (args: AddVariableArgs): SimulationComponentExtra
    {
        const { wcomponent_id, title, value, linked_ids = [] } = args

        const variable = model.Variable({
            name: wcomponent_id,
            value,
            note: `${title || "no human readable name"}: ${wcomponent_id}`,
        })
        add_ids_to_map(variable._node.id, wcomponent_id)

        const nodes = linked_ids.map(id => get_node_from_id(id, true))
        nodes.forEach(node => model.Link(node, variable))

        latest_model_results.values[variable._node.id] = value
        latest_model_results.values[wcomponent_id] = value

        // if (args.is_action)
        // {
        //     actions_by_id[wcomponent_id] = typeof value === "number" ? value : 0
        // }

        // Add extra fields
        const variable_extra: SimulationComponentExtra = {
            ...variable,
            name: wcomponent_id,
        }

        console.debug(`added variable ${variable_extra.name} with id: ${variable_extra._node.id} value: ${value}`)

        return variable_extra
    }


    function add_flow (args: AddFlowArgs): SimulationComponentExtra
    {
        const { wcomponent_id, title, linked_ids = [] } = args

        const from_node = get_node_from_id(args.from_id)
        const to_node = get_node_from_id(args.to_id)

        const flow = model.Flow(
            from_node,
            to_node,
            {
                name: wcomponent_id,
                note: `${title || "no human readable name"}: ${wcomponent_id}`,
                rate: args.flow_rate,
                nonNegative: args.only_positive ?? true,
            }
        )
        add_ids_to_map(flow._node.id, wcomponent_id)

        const nodes = linked_ids.map(id => get_node_from_id(id, true))
        nodes.forEach(node => model.Link(node, flow))

        // Add extra fields
        const flow_extra: SimulationComponentExtra = {
            ...flow,
            name: wcomponent_id,
        }

        console.debug(`added flow ${flow_extra.name} with id: ${flow_extra._node.id}`)

        return flow_extra
    }


    function add_action (args: AddActionArgs): SimulationComponentExtra
    {
        const { wcomponent_id, title, linked_ids = [] } = args

        const action = model.Action({
            name: wcomponent_id,
            action: args.action,
            trigger: "Condition",
            value: args.trigger_value,
            note: `${title || "no human readable name"}: ${wcomponent_id}`,
        })
        add_ids_to_map(action._node.id, wcomponent_id)

        const nodes = linked_ids.map(id => get_node_from_id(id, true))
        nodes.forEach(node => model.Link(node, action))

        // Add extra fields
        const action_extra: SimulationComponentExtra = {
            ...action,
            name: wcomponent_id,
        }

        console.debug(`added action ${action_extra.name} with id: ${action_extra._node.id}`)

        return action_extra
    }


    function get_current_time (): number
    {
        return latest_model_results.current_time
    }


    function get_latest_state_by_id (id: string): ModelValue | undefined
    {
        const model_id = get_id_as_model_id(id) || ""

        return latest_model_results.values[model_id]
    }


    /**
     * Use this in a react component like:
     *   const action__increase_stock_a = useMemo(() => model_stepper.make_apply_action(IDS.variable_component__action_increase_a), [])
    */
    const factory_trigger_action = (action_id: string, default_change_in_value: number=1) =>
    {
        return (custom_change_in_value?: number | Event) =>
        {
            const change_in_value = is_number(custom_change_in_value)
                ? custom_change_in_value
                : default_change_in_value
            mark_action_as_triggered(action_id, change_in_value)
        }
    }


    let actions_taken: {[action_id: string]: number} = {}
    const mark_action_as_triggered = (action_id: string, change_in_value: number=1) =>
    {
        const new_value = (actions_taken[action_id] || 0) + change_in_value
        actions_taken[action_id] = new_value
    }


    const past_actions_taken: {step: number, actions_taken: {[action_id: string]: number}}[] = []
    function apply_actions(result: SimulationStepResult)
    {
        const { set_value } = result
        if (!set_value) return { reason_to_stop: "Error: no set_value" }

        // Reset previously taken actions
        const last_actions_taken = past_actions_taken[past_actions_taken.length - 1]
        if (last_actions_taken && (last_actions_taken.step + 1) === result.current_step)
        {
            const last_action_ids_taken = Object.keys(last_actions_taken.actions_taken)
            last_action_ids_taken.forEach(action_id =>
            {
                const action = get_node_from_id(action_id, true)
                set_value(action, 0)
            })
        }

        // Apply actions taken
        const actions_taken_list = Object.entries(actions_taken)

        actions_taken_list.forEach(([action_id, value]) =>
        {
            const action = get_node_from_id(action_id, true)
            set_value(action, value)
        })

        if (actions_taken_list.length)
        {
            past_actions_taken.push({ step: result.current_step, actions_taken })
            actions_taken = {}
        }

        return undefined
    }


    function run_simulation (args: {
        on_simulation_step_completed: OnSimulationStepCompletedFunction;
        on_simulation_completed?: OnSimulationCompletedFunction;
    })
    {
        if (simulation_cancelled) throw new Error("Can not yet restart a cancelled simulation")
        if (model_simulation_started) throw new Error("Can not start an already started simulation")

        console .log(`Starting simulation running at ${target_refresh_rate} Hz`)
        model_simulation_started = true

        const ms_per_step = 1000 / target_refresh_rate
        const start_time_ms = new Date().getTime()


        function wait_for_next_simulation_step (): Promise<void>
        {
            return new Promise((resolve, reject) =>
            {
                const animate = () => {
                    if (simulation_cancelled)
                    {
                        reject(`Stopping simulation as requested by clean up function in "animate" function`)
                        return
                    }

                    const time_since_start_ms = new Date().getTime() - start_time_ms
                    // console.log("time_since_start_ms:", time_since_start_ms)
                    const potential_simulation_step = Math.floor(time_since_start_ms / ms_per_step)
                    if (potential_simulation_step > current_simulation_step)
                    {
                        current_simulation_step += 1
                        resolve()
                    }
                    else
                    {
                        // Schedule the next frame
                        requestAnimationFrame(animate)
                    }
                }

                // Start the simulation animation
                requestAnimationFrame(animate)
            })
        }


        async function onPause (simulation: OnPauseSimulationArg)
        {
            if (simulation_cancelled)
            {
                throw new Error(`Stopping simulation as requested by clean up function in "onPause" function`)
            }

            const step_results = extract_step_result(simulation.results, map_between_model_and_wcomponent_id)
            step_results.set_value = simulation.setValue
            const stop_simulation = args.on_simulation_step_completed(step_results)

            if (stop_simulation?.reason_to_stop)
            {
                throw new Error(`Stopping simulation as requested by client through on_simulation_step_completed because: "${stop_simulation?.reason_to_stop}"`)
            }

            // Not 100% sure this is the right time to apply actions. Perhaps we
            // should apply them before `on_simulation_step_completed` is
            // called but then they won't be applied until afterwards anyway.
            apply_actions(step_results)

            await wait_for_next_simulation_step()
        }


        // Start the simulation
        model.simulateAsync({ onPause })
        .then(result =>
        {
            if (args.on_simulation_completed)
            {
                const extended_result: ExtendedSimulationResult = {
                    ...result,
                    ...extract_step_result(result, map_between_model_and_wcomponent_id),
                }
                args.on_simulation_completed(extended_result)
            }
        })

        // Clean up function to cancel the simulation / animation
        function dispose_of_simulation()
        {
            console .log("Disposing simulation...")
            simulation_cancelled = true
        }

        return dispose_of_simulation
    }


    return {
        model: model as any,

        add_stock,
        add_variable,
        add_flow,
        add_action,

        // extract_step_result,
        // get_ids_map: () => map_between_model_and_wcomponent_id,

        // get_actions_by_id: () => ({...actions_by_id}),
        // apply_action: mark_action_as_triggered,
        factory_trigger_action,

        get_node_from_id,
        get_current_time,
        get_latest_state_by_id,
        // simulate_step,
        run_simulation,
        // cancel_simulation: () => { simulation_cancelled = true },
    }
}



function extract_step_result (simulation_result: SimulationResult, map_between_model_and_wcomponent_id: {[id: string]: string}): SimulationStepResult
{
    const values: ModelValues = {}

    Object.entries(simulation_result._data.data.last() || {}).forEach(([model_id, value]) =>
    {
        values[model_id] = value as any as number
        const wcomponent_id = map_between_model_and_wcomponent_id[model_id]
        if (wcomponent_id) values[wcomponent_id] = value as any as number
    })

    const latest_model_results: SimulationStepResult = {
        values,
        current_time: simulation_result._data.times.last() || 0,
        current_step: simulation_result._data.times.length - 1,
        set_value: undefined,  // This is set in the onPause function
    }

    return latest_model_results
}


function add_model_components (wrapped_model: WrappedModel, data?: GetItemsReturn<SimplifiedWComponentsValueById>)
{
    if (!data) return

    const stocks = data.value.statev2
    const actions = data.value.action
    const flows = data.value.causal_link

    Object.entries(stocks).forEach(([id, stock]) =>
    {
        wrapped_model.add_stock({
            wcomponent_id: id,
            title: stock.title,
            initial: stock.state,
        })
    })

    Object.entries(actions).forEach(([id, action]) =>
    {
        if (!action.calculation)
        {
            wrapped_model.add_variable({
                wcomponent_id: id,
                title: action.title,
                value: 0,
            })
        }
        else
        {
            const action_variable = wrapped_model.add_variable({
                wcomponent_id: id,
                title: action.title + " (variable)",
                value: 0,
            })

            wrapped_model.add_action({
                wcomponent_id: id + "_action",
                title: action.title + " (action)",
                action: action.calculation,
                trigger_value: `[${action_variable.name}]`,
                linked_ids: action.linked_ids,
            })
        }
    })

    Object.entries(flows).forEach(([id, flow]) =>
    {
        const linked_ids = get_uuids_from_text(flow.effect)

        wrapped_model.add_flow({
            wcomponent_id: id,
            title: flow.title,
            flow_rate: flow.effect,
            from_id: flow.from_id,
            to_id: flow.to_id,
            linked_ids,
        })
    })
}

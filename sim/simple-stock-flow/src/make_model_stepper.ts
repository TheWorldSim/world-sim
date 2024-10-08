import { ModelConfig, ModelStockConfig, ModelVariableConfig, OnPauseSimulationArg, Primitive, SimulationComponent, TimeUnitsAll } from "simulation"
import { MutableRef } from "preact/hooks"
const { Model } = await import("simulation")



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


type ModelValue = number | string | "True" | "False"
export interface ModelValues
{
    [component_id: string]: ModelValue
}
export interface ModelStepResult
{
    values: ModelValues
    current_time: number
    current_step: number
    set_value?: (cell: Primitive | SimulationComponent, value: number) => void
}

export interface AddStockArgs extends ModelStockConfig
{
    wcomponent_id: string
    initial: ModelValue
}

export interface AddVariableArgs extends ModelVariableConfig
{
    wcomponent_id: string
    is_action?: boolean
    linked_ids?: string[]
}

export interface AddFlowArgs
{
    wcomponent_id: string
    name: string
    flow_rate: string | number
    only_positive?: boolean
    from_id: string | undefined
    to_id: string | undefined
    linked_ids?: string[]
}


export interface AddActionArgs
{
    wcomponent_id: string
    name: string
    action: string
    trigger_value: string
    linked_ids?: string[]
}


export function make_model_stepper (
    // wcomponents: WComponentNode[],
    args: { target_refresh_rate: number },
    model_config: ModelConfig = {},
): ModelStepper
{
    // const wcomponent_by_id: WComponentsById = {}
    // wcomponents.forEach(wcomponent => wcomponent_by_id[wcomponent.id] = wcomponent)
    const { target_refresh_rate } = args
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

    const wrapped_model = make_wrapped_model(all_model_config, { target_refresh_rate })

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


export type ModelStepper = ReturnType<typeof make_wrapped_model>

function make_wrapped_model (model_config: ModelConfigStrict, run_sim_config: { target_refresh_rate: number })
{
    const model = new Model(model_config)
    const target_refresh_rate = run_sim_config.target_refresh_rate

    let model_simulation_started = false
    let simulation_cancelled = false
    let current_simulation_step = 0

    const map_between_model_and_wcomponent_id: {[id: string]: string} = {}
    const map_between_wcomponent_and_model_id: {[id: string]: string} = {}

    let latest_model_results: ModelStepResult = {
        values: {},
        current_time: model_config.timeStart,
        current_step: 0,
        set_value: undefined,
    }

    const actions_by_id: {[action_id: string]: number} = {}


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
        const { wcomponent_id, name, initial } = args

        const stock = model.Stock({
            name,
            initial,
            note: wcomponent_id,
        })

        map_between_model_and_wcomponent_id[stock._node.id] = wcomponent_id
        map_between_wcomponent_and_model_id[wcomponent_id] = stock._node.id
        latest_model_results.values[stock._node.id] = initial
        latest_model_results.values[wcomponent_id] = initial

        // Add extra fields
        const stock_extra: SimulationComponentExtra = {
            ...stock,
            name,
        }

        console.debug(`added stock ${stock_extra.name} with id: ${stock_extra._node.id} initial: ${initial}`)

        return stock_extra
    }


    function add_variable (args: AddVariableArgs): SimulationComponentExtra
    {
        const { wcomponent_id, name, value, linked_ids = [] } = args

        const variable = model.Variable({
            name,
            value,
            note: wcomponent_id,
        })

        const nodes = linked_ids.map(id => get_node_from_id(id, true))
        nodes.forEach(node => model.Link(node, variable))

        map_between_model_and_wcomponent_id[variable._node.id] = wcomponent_id
        map_between_wcomponent_and_model_id[wcomponent_id] = variable._node.id
        latest_model_results.values[variable._node.id] = value
        latest_model_results.values[wcomponent_id] = value

        if (args.is_action)
        {
            actions_by_id[wcomponent_id] = typeof value === "number" ? value : 0
        }

        // Add extra fields
        const variable_extra: SimulationComponentExtra = {
            ...variable,
            name,
        }

        console.debug(`added variable ${variable_extra.name} with id: ${variable_extra._node.id} value: ${value}`)

        return variable_extra
    }


    function add_flow (args: AddFlowArgs): SimulationComponentExtra
    {
        const { wcomponent_id, linked_ids = [] } = args

        const from_node = get_node_from_id(args.from_id)
        const to_node = get_node_from_id(args.to_id)

        const flow = model.Flow(
            from_node,
            to_node,
            {
                name: args.name,
                note: wcomponent_id,
                rate: args.flow_rate,
                nonNegative: args.only_positive ?? true,
            }
        )

        map_between_model_and_wcomponent_id[wcomponent_id] = flow._node.id
        map_between_model_and_wcomponent_id[flow._node.id] = wcomponent_id

        const nodes = linked_ids.map(id => get_node_from_id(id, true))

        nodes.forEach(node => model.Link(node, flow))

        // Add extra fields
        const flow_extra: SimulationComponentExtra = {
            ...flow,
            name: args.name,
        }

        console.debug(`added flow ${flow_extra.name} with id: ${flow_extra._node.id}`)

        return flow_extra
    }


    function add_action (args: AddActionArgs): SimulationComponentExtra
    {
        const { wcomponent_id, linked_ids = [] } = args

        const action = model.Action({
            name: args.name,
            action: args.action,
            trigger: "Condition",
            value: args.trigger_value,
            note: wcomponent_id,
        })

        map_between_model_and_wcomponent_id[action._node.id] = wcomponent_id
        map_between_wcomponent_and_model_id[wcomponent_id] = action._node.id

        const nodes = linked_ids.map(id => get_node_from_id(id, true))

        nodes.forEach(node => model.Link(node, action))

        // Add extra fields
        const action_extra: SimulationComponentExtra = {
            ...action,
            name: args.name,
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


    const apply_action = (actions_taken: MutableRef<{[action_id: string]: number}>, action_id: string, change_in_value: number=1) =>
    {
        const new_value = (actions_taken.current[action_id] || 0) + change_in_value
        actions_taken.current[action_id] = new_value
    }


    /**
     * Use this in a react component like:
     *   const actions_taken = useRef<{[action_id: string]: number}>({})
     *   const action__increase_stock_a = useMemo(model_stepper.make_apply_action(actions_taken, IDS.variable_component__action_increase_a), [])
     */
    const make_apply_action = (actions_taken: MutableRef<{[action_id: string]: number}>, action_id: string, change_in_value: number=1) =>
    {
        return () => () => apply_action(actions_taken, action_id, change_in_value)
    }


    function run_simulation (on_simulation_step_completed: (step_results: ModelStepResult) => { reason_to_stop: string } | undefined)
    {
        if (simulation_cancelled) throw new Error("Can not yet restart a cancelled simulation")
        if (model_simulation_started) throw new Error("Can not start an already started simulation")

        console.log(`Starting simulation running at ${target_refresh_rate} Hz`)
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
                        reject("Stopping simulation as requested by clean up function")
                        return
                    }

                    const time_since_start_ms = new Date().getTime() - start_time_ms
                    const simulation_step = Math.floor(time_since_start_ms / ms_per_step)
                    if (simulation_step > current_simulation_step)
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
                throw new Error("Stopping simulation as requested by clean up function")
            }

            const step_results = extract_step_results(simulation, current_simulation_step, map_between_model_and_wcomponent_id)
            const stop_simulation = on_simulation_step_completed(step_results)

            if (stop_simulation?.reason_to_stop)
            {
                throw new Error(`Stopping simulation as requested by client through on_simulation_step_completed because: "${stop_simulation?.reason_to_stop}"`)
            }

            await wait_for_next_simulation_step()
        }


        // Start the simulation
        model.simulateAsync({ onPause })

        // Clean up function to cancel the simulation / animation
        return () => {
            console.log("firing async clean up...")
            simulation_cancelled = true
        }
    }


    return {
        model: model as any,

        add_stock,
        add_variable,
        add_flow,
        add_action,

        // extract_step_results,
        // get_ids_map: () => map_between_model_and_wcomponent_id,

        get_actions_by_id: () => ({...actions_by_id}),
        apply_action,
        make_apply_action,

        get_node_from_id,
        get_current_time,
        get_latest_state_by_id,
        // simulate_step,
        // apply_action,
        run_simulation,
        cancel_simulation: () => { simulation_cancelled = true },
    }
}



function extract_step_results (simulation: OnPauseSimulationArg, current_simulation_step: number, map_between_model_and_wcomponent_id: {[id: string]: string}): ModelStepResult
{
    const values: ModelValues = {}

    Object.entries(simulation.results._data.data.last() || {}).forEach(([model_id, value]) =>
    {
        values[model_id] = value as any as number
        const wcomponent_id = map_between_model_and_wcomponent_id[model_id]
        if (wcomponent_id) values[wcomponent_id] = value as any as number
    })

    const latest_model_results: ModelStepResult = {
        values,
        current_time: simulation.results._data.times.last() || 0,
        current_step: current_simulation_step,
        set_value: simulation.setValue,
    }

    return latest_model_results
}

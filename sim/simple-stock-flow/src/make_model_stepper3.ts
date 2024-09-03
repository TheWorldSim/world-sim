import { ModelStockConfig, ModelVariableConfig, onPauseResArg, SimulationComponent, TimeUnitsAll } from "simulation"
import { IDS } from "./data/get_data"
const { Model } = await import("simulation")

if (Model.toString().match(/.*simulate\(\).*/mg))
{
    const error_msg = `Model requires a customised simulate method.  Please replace with the following code:
    simulate(config) {
        config = {
            silent: true,
            model: this,
            ...config,
        }

        let results = runSimulation(config);
        if (!results) return undefined
        // rest of function...
    `
    throw new Error(error_msg)
}


type ModelValue = number | string | "True" | "False"
interface ModelValues
{
    [component_id: string]: ModelValue
}
export interface ModelStepResult
{
    values: ModelValues
    current_time: number
}

export interface RunSimulationConfig
{
    target_refresh_rate?: number
    on_simulation_step_completed: (step_results: ModelStepResult) => void
}

export interface AddStockArgs extends ModelStockConfig
{
    wcomponent_id: string
    initial: ModelValue
}

export interface AddVariableArgs extends ModelVariableConfig
{
    wcomponent_id: string
}

export interface AddFlowArgs
{
    wcomponent_id: string
    name: string
    flow_rate: string
    from_id: string | undefined
    to_id: string | undefined
    linked_ids?: string[]
}


export function make_model_stepper (
    // wcomponents: WComponentNode[],
    TARGET_REFRESH_RATE: number
): ModelStepper
{
    // const wcomponent_by_id: WComponentsById = {}
    // wcomponents.forEach(wcomponent => wcomponent_by_id[wcomponent.id] = wcomponent)

    const time_start = 2020

    const time_step = 1 / TARGET_REFRESH_RATE
    const time_units = "Seconds"

    const wrapped_model = make_wrapped_model({
        timeStart: time_start,
        timeStep: time_step,
        timeLength: 1e6,
        timeUnits: time_units,
        timePause: time_step,
    })

    wrapped_model.add_stock({ wcomponent_id: IDS.state_component__stock_a_id, name: "Stock A", initial: 100 })
    wrapped_model.add_stock({ wcomponent_id: IDS.state_component__stock_b_id, name: "Stock B", initial: 10 })
    wrapped_model.add_variable({ wcomponent_id: IDS.variable_component__action_move_a_to_b_id, name: "Action: Move A to B", value: 0 })
    wrapped_model.add_flow({
        wcomponent_id: IDS.flow_component__flow_a_to_b_id,
        name: "Flow A to B",
        flow_rate: `[Action: Move A to B]`,
        // flow_rate: "1",
        from_id: IDS.state_component__stock_a_id,
        to_id: IDS.state_component__stock_b_id,
        linked_ids: [IDS.variable_component__action_move_a_to_b_id],
    })

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


interface ModelStepper
{
    get_latest_state_by_id: (wcomponent_id: string) => ModelValue | undefined
    // subscribe_to_state_change: (wcomponent_id: string, subscriber: (value: number) => void) => () => void
    // on_state_change: (subscriber: (state: ValueByWComponentId) => void) => () => void
    simulate_step: (simulation_step_completed: (step_return: ModelStepResult) => void) => void
    apply_action: (wcomponent_id: string) => void
    add_stock: (add_stock_args: AddStockArgs) => SimulationComponent
    add_variable: (add_variable_args: AddVariableArgs) => SimulationComponent
    add_flow: (add_flow_args: AddFlowArgs) => SimulationComponent
    get_current_time: () => number
    model: typeof Model
    extract_step_results: (res: onPauseResArg) => ModelStepResult
    get_ids_map: () => {[id: string]: string}
    run_simulation: (run_sim_config: RunSimulationConfig) => () => () => void
}


function make_wrapped_model (model_config: ModelConfigStrict): ModelStepper
{
    const model = new Model(model_config)
    let model_simulation_started = false

    const map_between_model_and_wcomponent_id: {[id: string]: string} = {}
    const map_between_wcomponent_and_model_id: {[id: string]: string} = {}

    let latest_model_results: ModelStepResult = {
        values: {},
        current_time: model_config.timeStart,
    }


    function get_id_as_model_id (id: string | undefined)
    {
        if (!id) return undefined

        const model_id = map_between_wcomponent_and_model_id[id]
        if (model_id) return model_id

        const wcomponent_id = map_between_model_and_wcomponent_id[id]
        if (wcomponent_id) return id

        console.error(`No model id (or wcomponent id) found for id: "${id}"`)
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


    function add_stock (args: AddStockArgs): SimulationComponent
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

        return stock
    }


    function add_variable (args: AddVariableArgs): SimulationComponent
    {
        const { wcomponent_id, name, value } = args

        const variable = model.Variable({
            name,
            value,
            note: wcomponent_id,
        })

        map_between_model_and_wcomponent_id[variable._node.id] = wcomponent_id
        map_between_wcomponent_and_model_id[wcomponent_id] = variable._node.id
        latest_model_results.values[variable._node.id] = value
        latest_model_results.values[wcomponent_id] = value

        return variable
    }


    function add_flow (add_flow_args: AddFlowArgs): SimulationComponent
    {
        const { wcomponent_id, linked_ids = [] } = add_flow_args

        const from_node = get_node_from_id(add_flow_args.from_id)
        const to_node = get_node_from_id(add_flow_args.to_id)

        const flow = model.Flow(
            from_node,
            to_node,
            {
                name: add_flow_args.name,
                note: wcomponent_id,
                rate: add_flow_args.flow_rate,
            }
        )

        map_between_model_and_wcomponent_id[wcomponent_id] = flow._node.id
        map_between_model_and_wcomponent_id[flow._node.id] = wcomponent_id

        const nodes = linked_ids.map(id => get_node_from_id(id, true))

        nodes.forEach(node => model.Link(node, flow))

        return flow
    }


    function extract_step_results (res: onPauseResArg): ModelStepResult
    {
        const values: ModelValues = {}

        Object.entries(res.data.last() || {}).forEach(([model_id, value]) =>
        {
            values[model_id] = value as any as number
            const wcomponent_id = map_between_model_and_wcomponent_id[model_id]
            if (wcomponent_id) values[wcomponent_id] = value as any as number
        })

        latest_model_results = {
            values,
            current_time: res.times.last() || 0,
        }

        return latest_model_results
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


    function simulate_step (simulation_step_completed: (step_results: ModelStepResult) => void)
    {
        console.log(`simulate step ${get_current_time() + (model_config.timeStep || 1)}`)

        if (!model_simulation_started)
        {
            model_simulation_started = true
            model.simulate({ onPause: factory_on_pause(simulation_step_completed) })
        }
        else
        {
            model_resume()
        }
    }


    let model_resume: () => void = () => {
        throw new Error("model_resume not yet set")
    }
    const factory_on_pause = (simulation_step_completed: (step_results: ModelStepResult) => void) => (res: onPauseResArg) =>
    {
        console.log("onPause", res.data)
        model_resume = res.resume

        const step_results = extract_step_results(res)
        simulation_step_completed(step_results)
    }

    function apply_action (wcomponent_id: string)
    {
        // We do this through setting the "slider" value of a variable

        const model_id = map_between_model_and_wcomponent_id[wcomponent_id]
        if (!model_id) return


    }


    let simulation_cancelled = false
    function run_simulation (run_sim_config: RunSimulationConfig)
    {
        return () =>
        {

            if (simulation_cancelled) throw new Error("Can not yet restart a cancelled simulation")
            const target_refresh_rate = run_sim_config.target_refresh_rate || 1
            console.log(`Starting simulation running at ${target_refresh_rate} Hz`)

            const ms_per_step = 1000 / target_refresh_rate

            let start_time_ms = new Date().getTime()
            let current_simulation_step = 0

            const animate = () => {
                if (simulation_cancelled) return

                const time_since_start_ms = new Date().getTime() - start_time_ms
                const simulation_step = Math.floor(time_since_start_ms / ms_per_step)
                if (simulation_step > current_simulation_step)
                {
                    current_simulation_step = simulation_step

                    const simulation_step_completed = (step_result: ModelStepResult) =>
                    {
                        run_sim_config.on_simulation_step_completed(step_result)

                        // Restart scheduling the next frame
                        requestAnimationFrame(animate)
                    }

                    simulate_step(simulation_step_completed)
                }
                else
                {
                    // Schedule the next frame
                    requestAnimationFrame(animate)
                }
            }

            // Start the simulation / animation
            requestAnimationFrame(animate)

            // Cleanup function to cancel the simulation / animation
            return () => {
                console.log("firing clean up...")
                simulation_cancelled = true
            }
        }
    }


    return {
        model: model as any,
        add_stock,
        add_variable,
        add_flow,
        extract_step_results,
        get_ids_map: () => map_between_model_and_wcomponent_id,
        get_current_time,
        get_latest_state_by_id,
        simulate_step,
        apply_action,
        run_simulation,
    }
}

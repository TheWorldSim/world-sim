import { Model } from "simulation"

// import { ModelConfigStrict, ModelStepResult, ModelValues } from "./make_model_stepper3"


if (!Array.prototype.last) {
    Object.defineProperty(Array.prototype, "last", {
        enumerable: false,
        writable: false,
        configurable: false,
        // value: function last<T> (this: T[]): T | undefined {
        value: function () {
            return this[this.length - 1]
        }
    })
}


if (Model.toString().match(/.*simulate\(\).*/mg))
{
    const error_msg = `Model requires a customised simulate method.

    Please:
    1.  stop the vite server.
    2.  delete: rm -rf node_modules/.vite
    3.  open: node_modules/simulation/src/api/Model.js and replace with the following code:

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


// const model_config: ModelConfigStrict = {
const model_config = {
    timeStart: 0,
    timeStep: 1,
    timeLength: 10,
    timeUnits: "Seconds",
    timePause: 1,
}

const model = new Model(model_config)

const stock_a = model.Stock({
    name: "Stock A",
    initial: 100,
})

const action_increase_a = model.Variable({
    name: "Action: Increase Stock A",
    value: 0,
})

const flow_into_stock_a = model.Flow(undefined, stock_a, {
    name: "Flow into A",
    rate: `[Action: Increase Stock A]`,
})

model.Link(action_increase_a, flow_into_stock_a)


// function extract_step_results (current_simulation_step: number, res: onPauseResArg): ModelStepResult
function extract_step_results (current_simulation_step, res)
{
    // const values: ModelValues = {}
    const values = {}

    Object.entries(res.data.last() || {}).forEach(([model_id, value]) =>
    {
        // values[model_id] = value as any as number
        values[model_id] = value
    })

    latest_model_results = {
        values,
        current_time: res.times.last() || 0,
        current_step: current_simulation_step,
        set_value: res.setValue,
    }

    return latest_model_results
}

// let latest_model_results: ModelStepResult = {
let latest_model_results = {
    values: {},
    current_time: model_config.timeStart,
    current_step: 0,
    set_value: undefined,
}

// function onPause (res: onPauseResArg)
function onPause (res)
{
    latest_model_results = extract_step_results(latest_model_results.current_step + 1, res)
    console.log(`step completed: ${latest_model_results.current_step}.  Stock A: ${latest_model_results.values[stock_a._node.id]}.  Action: Increase Stock A: ${latest_model_results.values[action_increase_a._node.id]}`)

    // if step is 3 then user clicks button to increase stock 5 times
    if (latest_model_results.current_step === 3)
    {
        console.log("  Increase Stock A by 5")
        res.setValue(action_increase_a._node, 5)
    }
    else if (latest_model_results.current_step === 4)
    {
        console.log("  Reset Increase Stock A action to 0")
        res.setValue(action_increase_a._node, 0)
    }

    // Resume simulation
    res.resume()
}

model.simulate({ onPause })

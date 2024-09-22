import { Model } from "simulation"


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


if (Model.toString().match(/.*simulate\(\).*/mg) && !Model.toString().match(/.*async simulateAsync\(.*/mg))
{
    const error_msg = `Model requires a customised simulate method or async simulateAsync method.

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

const actual_action_increase_a = model.Action({
    name: "Increase Stock A",
    action: `[Stock A] <- [Stock A] + [Action: Increase Stock A]`,
    trigger: "Condition",
    value: `[Action: Increase Stock A]`,
})

model.Link(stock_a, actual_action_increase_a)
model.Link(action_increase_a, actual_action_increase_a)


// import type { onPauseSimulationArg } from "simulation"
// async function onPause (simulation: onPauseSimulationArg)
async function onPause (simulation)
{
    const current_time = simulation.time
    console.log(`step completed: ?, current time: ${current_time}.  Stock A: ${simulation.results.value(stock_a, current_time)}.  Action: Increase Stock A: ${simulation.results.value(action_increase_a, current_time)}`)

    // if step is 3 then user clicks button to increase stock 5 times
    if (current_time === 3)
    {
        console.log("  Increase Stock A by 5")
        simulation.setValue(action_increase_a, 5)
    }
    else if (current_time === 4)
    {
        console.log("  Reset Increase Stock A action to 0")
        simulation.setValue(action_increase_a, 0)
    }
}

model.simulateAsync({ onPause })

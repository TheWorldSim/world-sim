import { wcomponent_is_action, WComponentNode, WComponentsById } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { Model2, SimulationResult2, ValueByWComponentId } from "./simulation2/simulation2"
// const { Model, ModelVariableConfig } = await import("simulation")


interface ModelStepper
{
    state_by_id: (wcomponent_id: string) => number | undefined
    // subscribe_to_state_change: (wcomponent_id: string, subscriber: (value: number) => void) => () => void
    on_state_change: (subscriber: (state: ValueByWComponentId) => void) => () => void
    simulate_step: () => SimulationResult2
    apply_action: (wcomponent_id: string) => void
}


export function make_model_stepper2 (wcomponents: WComponentNode[]): ModelStepper
{
    const wcomponent_by_id: WComponentsById = {}
    wcomponents.forEach(wcomponent => wcomponent_by_id[wcomponent.id] = wcomponent)

    let time_start = 2020
    const time_step = 1
    const time_step_units = "Years"

    const model = new Model2({timeStart: time_start, timeLength: time_step, timeUnits: time_step_units}, wcomponents)

    return {
        state_by_id: (wcomponent_id: string) =>
        {
            return model.state[wcomponent_id]
        },
        // subscribe_to_state_change: (wcomponent_id: string, subscriber: (value: number) => void) =>
        // {
        //     console.log(`subscribe ${wcomponent_id}`)
        //     const wcomponent = wcomponent_by_id[wcomponent_id]
        //     if (!wcomponent) throw new Error(`No component with id ${wcomponent_id}`)
        //     if (!wcomponent_is_statev2(wcomponent)) throw new Error(`Component with id ${wcomponent_id} is not a state`)

        //     return model.subscribe_to_state_change(wcomponent_id, subscriber)
        // },
        on_state_change: (subscriber: (state: ValueByWComponentId) => void) =>
        {
            return model.on_state_change(subscriber)
        },
        simulate_step: () => {
            console.log(`simulate step ${time_start}`)
            const results = model.simulate()
            // print_values(results)

            time_start += time_step

            // const model_step = make_model(model_state, time_start, time_step, time_step_units)
            // model = model_step.model
            // stocks = model_step.stocks
            return results
        },
        apply_action: (wcomponent_id: string) => {
            console.log(`apply action ${wcomponent_id}`)
            const action = wcomponent_by_id[wcomponent_id]
            if (!action) throw new Error(`No action with id ${wcomponent_id}`)
            if (!wcomponent_is_action(action)) throw new Error(`Component with id ${wcomponent_id} is not an action`)

            const action_calculations = action.calculations || []
            model.apply_calculations(action_calculations)
        },
    }
}



// function make_model (model_state: ValueByWComponentId, time_start: number, time_step: number, time_step_units: TimeUnitsAll)
// {
//     const model = new Model({
//         timeStart: time_start,
//         timeLength: time_step,
//         timeUnits: time_step_units,
//     })

//     const stocks = make_model_stocks(model, model_state)

//     return {model, stocks}
// }


// function make_model_stocks(model: Model, model_state: ValueByWComponentId): ExtendedSimulationComponent[]
// {
//     const stocks: ExtendedSimulationComponent[] = Object.entries(model_state).map(([wcomponent_id, initial_value]) =>
//     {
//         const wcomponent = wcomponent_by_id[wcomponent_id]

//         const stock = model.Stock({
//             name: wcomponent_id,
//             initial: initial_value,
//             note: wcomponent?.title || `not found ${wcomponent_id}`,
//         })

//         return { ...stock, wcomponent_id }
//     })

//     return stocks
// }


function print_values (simulation_result: SimulationResult2, get_wcomponent_by_id: (id: string) => WComponentNode | undefined)
{
    const values_with_name: {name: string, value: number}[] = []

    Object.entries(simulation_result.state).forEach(([wcomponent_id, value]) =>
    {
        const wcomponent = get_wcomponent_by_id(wcomponent_id)
        values_with_name.push({ name: wcomponent?.title || `not found ${wcomponent_id}`, value})
    })

    console.log(values_with_name.map(e => `${e.name}: ${e.value}`).join(" | "))
}


// console.log("Simulation about to start...")
// const model_stepper = make_model_stepper(input)
// model_stepper.simulate_step()
// model_stepper.simulate_step()
// model_stepper.apply_action("8ecb8d21-4803-4028-9341-b7cd59b56cda")
// model_stepper.simulate_step()
// model_stepper.apply_action("2dc650ae-8458-47b6-be0e-6ad1cab3cd4d")
// model_stepper.simulate_step()


// let m = new Model({
//     timeStart: 2020,
//     timeLength: 100,
//     timeUnits: "Years"
// });

// // Start with 7 billion people in the "people" stock
// let people = m.Stock({
//     name: "People",
//     initial: 7e9
// });

// // Use a net growth rate of 2% a year
// let growthRate = m.Variable({
//     name: "Growth Rate",
//     value: 0.02
// });

// // The population growth each year is the number of people times the growth rate
// // Please note that we refer to the value of other primitives in the model with the
// // [name] syntax.
// let netGrowth = m.Flow(null, people, {
//     rate: "[People] * [Growth Rate]"
// });

// // For the netGrowth flow to be able to reference the growthRate, we need to link the primitives
// m.Link(growthRate, netGrowth);

// let results = m.simulate();

// import { table, plot } from "simulation-viz-console";
// table(results, people);



// let model = new Model({
//     timeStart: 2020,
//     timeLength: 3,
//     timeUnits: "Years",
// })

// interface SimulationStep
// {
//     finished: boolean
//     step_results: SimulationResult | undefined
// }

// // const make_model_simulate_stepper = (model: Model) =>
// // {
// //     const time_start = model.timeStart
// //     const time_length = model.timeLength
// //     let steps = 0
// //     let finished = time_length === 0
// //     let last_step_results: SimulationResult | undefined = undefined

// //     return (): SimulationStep =>
// //     {
// //         if (!finished)
// //         {
// //             model.timeStart = time_start + steps
// //             model.timeLength = 1
// //             debugger
// //             model.get(({ _node, model }) =>
// //             {
// //                 _node
// //             })
// //             last_step_results = model.simulate()
// //             steps++
// //             finished = steps >= time_length
// //         }

// //         return { finished, step_results: last_step_results }
// //     }
// // }

// let stockA = model.Stock({
//     name: "17edbf36-ad5b-4936-b3c5-7d803741c678",
//     initial: 3,
// })

// let calc1_output = model.Stock({
//     name: "Stock A available",
// })

// let calc1_equation = model.Flow(null, calc1_output, {
//     rate: "If [17edbf36-ad5b-4936-b3c5-7d803741c678] > 0 Then \n  1\nEnd If"
// })
// model.Link(stockA, calc1_equation)

// let stockB = model.Stock({
//     name: "b644e33f-c00f-4a50-acc4-f158e4e11be5",
//     note: "Stock B",
//     initial: 1,
// })

// // if we have the same name already then we don't need to form a new calc_output
// // name: "b644e33f-c00f-4a50-acc4-f158e4e11be5",
// // let calc_output2 = model.Stock({
// //     name: "Stock A available",
// // })

// let calc2_equation = model.Flow(null, stockB, {
//     rate: "[b644e33f-c00f-4a50-acc4-f158e4e11be5] + [Stock A available]"
// })
// model.Link(calc1_output, calc2_equation)

// let calc2 = model.Variable({

//     value: "[b644e33f-c00f-4a50-acc4-f158e4e11be5] = [b644e33f-c00f-4a50-acc4-f158e4e11be5] + [Stock A available]"
// })
// model.Link(stockB, calc2)
// model.Link(calc1_output, calc2)

// let calc3 = model.Variable({
//     id: 2,
//     name: "17edbf36-ad5b-4936-b3c5-7d803741c678",
//     value: "17edbf36-ad5b-4936-b3c5-7d803741c678 - [Stock A available]"
// })



// const print_initial_values = () =>
// {
//     console.log(
//         stockA.initial,
//         calc1_output.initial,
//         stockB.initial,
//     )
// }


// const print_simulation_result = (simulation_step: SimulationStep) =>
// {
//     const simulation_result = simulation_step.step_results
//     if (!simulation_result)
//     {
//         console.log("No simulation result")
//         return
//     }

//     const last_index = simulation_result._data.data.length - 1
//     const data = simulation_result._data.data[last_index]
//     if (!data) throw new Error("No simulation data")

//     const stockA_value = data[stockA._node.id]
//     const calc1_output_value = data[calc1_output._node.id]
//     // const calc1_equation_value = data[calc1_equation._node.id]
//     const stockB_value = data[stockB._node.id]
//     // const calc2_equation_value = data[calc2_equation._node.id]
//     // const calc2_value = data[calc2._node.id]
//     console.log(
//         stockA_value,
//         calc1_output_value,
//         // calc1_equation_value,
//         stockB_value,
//         // calc2_equation_value,
//     )
// }

// print_initial_values()
// // debugger
// const results = model.simulate()
// console.log(results)
// const simulation_stepper = make_model_simulate_stepper(model)
// print_simulation_result(simulation_stepper())
// print_simulation_result(simulation_stepper())
// print_simulation_result(simulation_stepper())
// print_simulation_result(simulation_stepper())

// ;[{
//     id: 0,
//     name: "Stock A available",
//     value: "If 17edbf36-ad5b-4936-b3c5-7d803741c678 > 0 Then \\n  1\\nEnd If"
// },
// {
//     id: 1,
//     name: "b644e33f-c00f-4a50-acc4-f158e4e11be5",
//     value: "b644e33f-c00f-4a50-acc4-f158e4e11be5 + [Stock A available]"
// },
// {
//     id: 2,
//     name: "17edbf36-ad5b-4936-b3c5-7d803741c678",
//     value: "17edbf36-ad5b-4936-b3c5-7d803741c678 - [Stock A available]"
// }].forEach(calculation =>
// {
//     const model_config: ModelVariableConfig = {
//         name: calculation.name,
//         value: calculation.value,
//     }
//     const model_component = model.Variable(model_config)
// })


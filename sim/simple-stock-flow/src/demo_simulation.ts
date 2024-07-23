import { Model /*, ModelVariableConfig*/ } from "simulation"
import { WComponentNodeStateV2 } from "./data_curator/src/wcomponent/interfaces/state"
import { WComponentNodeAction } from "./data_curator/src/wcomponent/interfaces/actions"
import { get_wcomponent_state_value_and_probabilities } from "./data_curator/src/wcomponent_derived/get_wcomponent_state_value_and_probabilities"
// const { Model, ModelVariableConfig } = await import("simulation")

const input: (WComponentNodeStateV2 | WComponentNodeAction)[] = [
    {
        "id": "17edbf36-ad5b-4936-b3c5-7d803741c678",
        "created_at": new Date("2024-05-03T15:49:53.649Z"),
        "title": "Stock A",
        "description": "",
        "type": "statev2",
        "values_and_prediction_sets": [
            {
                "id": "vpsbe224cf2-9956-4174-86d7-ce4a0859a65d",
                "created_at": new Date("2024-05-03T15:50:08.702Z"),
                "base_id": 1,
                "datetime": {},
                "entries": [
                    {
                        "id": "VAPc61f1e72-1873-4646-9286-efa8da3f4134",
                        "explanation": "",
                        "probability": 1,
                        "conviction": 1,
                        "value": "3",
                        "description": "",
                        "value_id": "e1aace4d-4dde-43d5-886e-c9b21b53d6f6"
                    }
                ]
            }
        ],
        "label_ids": [],
        "modified_by_username": "ajp",
        "modified_at": new Date("2024-05-28T14:04:06.163Z"),
        "subtype": "number",
        "value_possibilities": {
            "e1aace4d-4dde-43d5-886e-c9b21b53d6f6": {
                "id": "e1aace4d-4dde-43d5-886e-c9b21b53d6f6",
                "value": "3",
                "description": "",
                "order": 0
            }
        },
        "calculations": [
            {
                "id": 0,
                "name": "@@17edbf36-ad5b-4936-b3c5-7d803741c678",
                "value": "max(@@17edbf36-ad5b-4936-b3c5-7d803741c678, 0)",
                "result_description": "Validation calculation; ensures the value is always valid",
                "result_sig_figs": 2
            }
        ],
        "base_id": 1
    },
    {
        "id": "b644e33f-c00f-4a50-acc4-f158e4e11be5",
        "created_at": new Date("2024-05-03T15:59:42.459Z"),
        "title": "Stock B",
        "description": "",
        "type": "statev2",
        "values_and_prediction_sets": [
            {
                "id": "vps2083b32a-3ea0-4d9c-af56-048075306db1",
                "created_at": new Date("2024-05-03T16:00:02.021Z"),
                "base_id": 1,
                "datetime": {},
                "entries": [
                    {
                        "id": "VAPca079704-06a1-4bd8-b5f7-0feb256a430a",
                        "explanation": "",
                        "probability": 1,
                        "conviction": 1,
                        "value": "0",
                        "description": "",
                        "value_id": "cda07232-ed96-40a5-81b2-552927e9e9e4"
                    }
                ]
            }
        ],
        "label_ids": [],
        "modified_by_username": "ajp",
        "modified_at": new Date("2024-05-03T16:07:58.663Z"),
        "subtype": "number",
        "value_possibilities": {
            "cda07232-ed96-40a5-81b2-552927e9e9e4": {
                "id": "cda07232-ed96-40a5-81b2-552927e9e9e4",
                "value": "0",
                "description": "",
                "order": 0
            }
        },
        "calculations": [
            {
                "id": 0,
                "name": "@@b644e33f-c00f-4a50-acc4-f158e4e11be5",
                "value": "max(@@b644e33f-c00f-4a50-acc4-f158e4e11be5, 0)",
                "result_description": "Validation calculation; ensures the value is always valid"
            }
        ],
        "base_id": 1
    },
    {
        "id": "8ecb8d21-4803-4028-9341-b7cd59b56cda",
        "created_at": new Date("2024-05-03T15:50:17.175Z"),
        "title": "Increase stock A",
        "description": "",
        "type": "action",
        "values_and_prediction_sets": [],
        "label_ids": [],
        "modified_by_username": "ajp",
        "modified_at": new Date("2024-05-03T16:57:17.084Z"),
        "reason_for_status": "",
        "depends_on_action_ids": [],
        "calculations": [
            {
                "id": 1,
                "name": "@@17edbf36-ad5b-4936-b3c5-7d803741c678",
                "value": "@@17edbf36-ad5b-4936-b3c5-7d803741c678 + 1"
            }
        ],
        "base_id": 1
    },
    {
        "id": "2dc650ae-8458-47b6-be0e-6ad1cab3cd4d",
        "created_at": new Date("2024-05-03T16:00:22.385Z"),
        "title": "Move A to B",
        "description": "@@26ed80ff-1043-43bc-bdd4-8bf06cd8ebc0",
        "type": "action",
        "values_and_prediction_sets": [],
        "label_ids": [],
        "modified_by_username": "ajp",
        "modified_at": new Date("2024-05-24T16:51:25.392Z"),
        "calculations": [
            {
                "id": 0,
                "name": "Stock A available",
                "value": "If @@17edbf36-ad5b-4936-b3c5-7d803741c678 > 0 Then \\n  1\\nEnd If"
            },
            {
                "id": 1,
                "name": "@@b644e33f-c00f-4a50-acc4-f158e4e11be5",
                "value": "@@b644e33f-c00f-4a50-acc4-f158e4e11be5 + [Stock A available]"
            },
            {
                "id": 2,
                "name": "@@17edbf36-ad5b-4936-b3c5-7d803741c678",
                "value": "@@17edbf36-ad5b-4936-b3c5-7d803741c678 - [Stock A available]"
            }
        ],
        "reason_for_status": "",
        "depends_on_action_ids": [],
        "base_id": 1
    }
]


function make_model_stepper (input: (WComponentNodeStateV2 | WComponentNodeAction)[])
{
    let time_start = 2020
    const time_step = 1
    const time_step_units = "Years"

    const {model, stocks} = make_model_step(input, time_start, time_step, time_step_units)


    return {
        simulate_step: () => {
            console.log(`simulate step ${time_start}`)
            const results = model.simulate()
            print_values(results, stocks)
        },
        apply_action: (action_id: string) => {

        },
    }
}

function make_model_step (input: (WComponentNodeStateV2 | WComponentNodeAction)[], time_start: number, time_step: number, time_step_units: TimeUnitsAll)
{
    const model = new Model({
        timeStart: time_start,
        timeLength: time_step,
        timeUnits: time_step_units,
    })

    const stocks = get_stocks(model, input)

    return {model, stocks}
}


const created_at_ms = new Date().getTime()
const sim_ms = new Date().getTime()
function get_stocks(model: Model, input: (WComponentNodeStateV2 | WComponentNodeAction)[])
{
    const stock_inputs = input.filter(item => item.type === "statev2") as WComponentNodeStateV2[]

    const stocks = stock_inputs.map(stock_input =>
    {
        const most_probable_initial_values = get_wcomponent_state_value_and_probabilities({ wcomponent: stock_input, VAP_set_id_to_counterfactual_v2_map: undefined, created_at_ms, sim_ms })
        const most_probable_initial_value = most_probable_initial_values.most_probable_VAP_set_values[0]
        let initial_value = most_probable_initial_value && most_probable_initial_value.parsed_value
        if (initial_value === undefined || typeof initial_value !== "number")
        {
            initial_value = 0
        }

        const stock = model.Stock({
            name: stock_input.id,
            initial: initial_value,
            // Optional for debugging
            note: stock_input.title,
        })

        // const calculations = stock_input.calculations || []
        // calculations.forEach(calculation =>
        // {
        //     model.Variable({
        //         name: calculation.name,
        //         value: calculation.value,
        //     })
        // })

        return stock
    })

    return stocks
}

function print_values (simulation_result: SimulationResult, simulation_components: SimulationComponent[])
{
    const last_index = simulation_result._data.data.length - 1
    const data = simulation_result._data.data[last_index]
    if (!data) throw new Error("No simulation data")

    const values: {[node_id: string]: number} = {}

    simulation_components.forEach(component =>
    {
        const value = data[component._node.id]
        if (value === undefined) throw new Error(`No value for component ${component._node.id}`)
        values[component._node.id] = value
    })

    console.log(values)
}


console.log("Simulation about to start...")
const model_stepper = make_model_stepper(input)
model_stepper.simulate_step()


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


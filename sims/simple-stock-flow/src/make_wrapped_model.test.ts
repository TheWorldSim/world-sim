import { ModelConfig } from "simulation"
import { SimplifiedWComponentsValueById } from "./data/get_wcomponents_values_by_id"
import { describe, test } from "./data_curator/src/shared/utils/test"
import { GetItemsReturn } from "./data_curator/src/state/sync/supabase/get_items"
import {
    make_wrapped_model,
    SimulationStepResult,
    OnSimulationStepCompletedFunction,
    ExtendedSimulationResult,
    WrappedModel,
    ModelValue,
} from "./make_wrapped_model"
import { create_deferred_promise } from "./utils/promise"
import { dedent } from "./utils/string"


const IDs = {
    stock__state_a: "17edbf36-ad5b-4936-b3c5-7d803741c678",
    flow__flow_into_a: "c31be0d8-6ce5-4db2-8b4b-1d13ed48a869",
    action__change_a: "8ecb8d21-4803-4028-9341-b7cd59b56cda",

    stock__state_b: "e429827b-3b12-475b-bcd8-9afd6f4b9973",
    action__move_a_to_b: "8b65e686-2926-4f89-8919-79d450ac682f",
}


function fixture_model_data__single_stock__action_variable()
{
    const data: GetItemsReturn<SimplifiedWComponentsValueById> = {
        value: {
            statev2: {
                "17edbf36-ad5b-4936-b3c5-7d803741c678": {
                    title: "Stock A",
                    state: 10,
                },
            },
            causal_link: {
                "c31be0d8-6ce5-4db2-8b4b-1d13ed48a869": {
                    title: "Flow into @@17edbf36-ad5b-4936-b3c5-7d803741c678",
                    effect: "[8ecb8d21-4803-4028-9341-b7cd59b56cda]",
                    to_id: "17edbf36-ad5b-4936-b3c5-7d803741c678",
                }
            },
            action: {
                "8ecb8d21-4803-4028-9341-b7cd59b56cda": {
                    title: "Increase stock A by 1",
                    calculation: "",
                }
            }
        },
        error: undefined,
    }


    function get_values(step_result: SimulationStepResult)
    {
        return {
            stock_a: step_result.values[IDs.stock__state_a],
            action_change_a: step_result.values[IDs.action__change_a],
            flow_into_a: step_result.values[IDs.flow__flow_into_a],
        }
    }


    function make_model_manually(wrapped_model: WrappedModel)
    {
        const stock_a_value = data.value.statev2[IDs.stock__state_a]!
        wrapped_model.add_stock({
            wcomponent_id: IDs.stock__state_a,
            title: stock_a_value.title,
            initial: stock_a_value.state,
        })

        wrapped_model.add_variable({
            wcomponent_id: IDs.action__change_a,
            title: "Increase A by 1",
            value: 0,
        })

        wrapped_model.add_flow({
            wcomponent_id: IDs.flow__flow_into_a,
            title: "Flow into A",
            flow_rate: data.value.causal_link[IDs.flow__flow_into_a]?.effect || "",
            from_id: undefined,
            to_id: IDs.stock__state_a,
            linked_ids: [IDs.action__change_a],
        })
    }

    return { data, get_values, make_model_manually }
}



function fixture_model_data__single_stock__action_calc()
{
    const data: GetItemsReturn<SimplifiedWComponentsValueById> = {
        value: {
            statev2: {
                "17edbf36-ad5b-4936-b3c5-7d803741c678": {
                    title: "Stock A",
                    state: 10,
                },
            },
            causal_link: {},
            action: {
                "8ecb8d21-4803-4028-9341-b7cd59b56cda": {
                    title: "Change stock A by N",
                    calculation: "[17edbf36-ad5b-4936-b3c5-7d803741c678] <- [17edbf36-ad5b-4936-b3c5-7d803741c678] + [8ecb8d21-4803-4028-9341-b7cd59b56cda]",
                    linked_ids: [
                        "17edbf36-ad5b-4936-b3c5-7d803741c678",
                        "8ecb8d21-4803-4028-9341-b7cd59b56cda",
                    ],
                },
            }
        },
        error: undefined,
    }


    function get_values(step_result: SimulationStepResult)
    {
        return {
            stock_a: step_result.values[IDs.stock__state_a],
            action_change_a: step_result.values[IDs.action__change_a],
            flow_into_a: step_result.values[IDs.flow__flow_into_a],
        }
    }


    function make_model_manually(wrapped_model: WrappedModel)
    {
        const stock_a_value = data.value.statev2[IDs.stock__state_a]
        wrapped_model.add_stock({
            wcomponent_id: IDs.stock__state_a,
            title: "Stock A",
            initial: stock_a_value?.state!,
        })

        const action_variable__change_a = wrapped_model.add_variable({
            wcomponent_id: IDs.action__change_a,
            title: "Change A (variable)",
            value: 0,
        })

        const action__change_a = data.value.action[IDs.action__change_a]
        wrapped_model.add_action({
            wcomponent_id: IDs.action__change_a + "_action",
            title: "Change A (action)",
            action: action__change_a!.calculation,
            trigger_value: `[${action_variable__change_a.name}]`,
            linked_ids: [
                IDs.action__change_a,
                IDs.stock__state_a,
            ],
        })
    }

    return { data, get_values, make_model_manually }
}



function fixture_model_data__two_stocks__action_calc()
{
    const data: GetItemsReturn<SimplifiedWComponentsValueById> = {
        value: {
            statev2: {
                "17edbf36-ad5b-4936-b3c5-7d803741c678": {
                    title: "Stock A",
                    state: 10,
                },
                "e429827b-3b12-475b-bcd8-9afd6f4b9973": {
                    title: "Stock B",
                    state: 0,
                },
            },
            causal_link: {},
            action: {
                "8b65e686-2926-4f89-8919-79d450ac682f": {
                    title: "Move stock A to B by N",
                    calculation: dedent(`
                    stock_a_available <- max([17edbf36-ad5b-4936-b3c5-7d803741c678],0)
                    stock_a_to_move <- min(stock_a_available,[8b65e686-2926-4f89-8919-79d450ac682f])
                    [e429827b-3b12-475b-bcd8-9afd6f4b9973] <- [e429827b-3b12-475b-bcd8-9afd6f4b9973] + stock_a_to_move
                    [17edbf36-ad5b-4936-b3c5-7d803741c678] <- [17edbf36-ad5b-4936-b3c5-7d803741c678] - stock_a_to_move
                    `),
                    linked_ids: [
                        "17edbf36-ad5b-4936-b3c5-7d803741c678",
                        "8b65e686-2926-4f89-8919-79d450ac682f",
                        "e429827b-3b12-475b-bcd8-9afd6f4b9973",
                    ],
                },
            }
        },
        error: undefined,
    }


    function get_values(step_result: SimulationStepResult)
    {
        return {
            stock_a: step_result.values[IDs.stock__state_a],
            stock_b: step_result.values[IDs.stock__state_b],
            action_move_a_to_b: step_result.values[IDs.action__move_a_to_b],
        }
    }


    function make_model_manually(wrapped_model: WrappedModel)
    {
        const stock_a_value = data.value.statev2[IDs.stock__state_a]!
        const stock_b_value = data.value.statev2[IDs.stock__state_b]!

        wrapped_model.add_stock({
            wcomponent_id: IDs.stock__state_a,
            title: "Stock A",
            initial: stock_a_value.state,
        })
        wrapped_model.add_stock({
            wcomponent_id: IDs.stock__state_b,
            title: "Stock B",
            initial: stock_b_value.state,
        })

        const action_variable__move_a_to_b = wrapped_model.add_variable({
            wcomponent_id: IDs.action__move_a_to_b,
            title: "Move A to B (variable)",
            value: 0,
        })

        const action__move_a_to_b = data.value.action[IDs.action__move_a_to_b]!
        wrapped_model.add_action({
            wcomponent_id: IDs.action__move_a_to_b + "_action",
            title: "Move A to B (action)",
            action: action__move_a_to_b.calculation,
            trigger_value: `[${action_variable__move_a_to_b.name}]`,
            linked_ids: action__move_a_to_b.linked_ids,
        })
    }

    return { data, get_values, make_model_manually }
}


const model_config: ModelConfig = {
    timeStart: 10,
    timeStep: 1,
    timeLength: 3,
    timePause: 1,
}
const model_config5 = {...model_config, timeLength: 5}

export const test_make_wrapped_model = describe.delay("make_model_stepper", async () =>
{
    async function run_simulation(wrapped_model: WrappedModel, change_in_value?: number)
    {
        const run_simulation = create_deferred_promise<{ step_results: SimulationStepResult[], simulation_result: ExtendedSimulationResult }>()

        const step_results: SimulationStepResult[] = []
        const on_simulation_step_completed: OnSimulationStepCompletedFunction = step_result => {
            step_results.push(step_result)

            if (step_results.length === 1)
            {
                wrapped_model.factory_trigger_action(IDs.action__change_a)(change_in_value)
            }

            return undefined
        }

        wrapped_model.run_simulation({
            on_simulation_step_completed,
            on_simulation_completed: simulation_result => run_simulation.resolve({ step_results, simulation_result }),
        })

        const results = await run_simulation.promise
        return results
    }


    function assess_results(
        results: {
            step_results: SimulationStepResult[];
            simulation_result: ExtendedSimulationResult;
        },
        get_values: (step_result: SimulationStepResult) => {
            stock_a: ModelValue | undefined;
            action_change_a: ModelValue | undefined;
            flow_into_a: ModelValue | undefined;
        },
        flow_should_be_present: boolean,
        expected_change_in_value: number = 1,
    )
    {
        test(results.step_results.length, 2, "step_results count, note that we miss the initial state and the final step & state, hence being two less than the number of time steps in the simulation")
        test(results.simulation_result._data.times.length, 4, "number of simulation time steps")

        let step_result: SimulationStepResult = results.step_results.shift()!
        test(step_result.current_step, 1, "step result 1, current_step")
        test(step_result.current_time, 11, "step result 1, current_time")
        test(get_values(step_result), {
            stock_a: 10,
            action_change_a: 0,
            flow_into_a: flow_should_be_present ? 0 : undefined,
        }, "step result 1 values")

        step_result = results.step_results.shift()!
        test(step_result.current_step, 2, "step result 2, current_step")
        test(step_result.current_time, 12, "step result 2, current_time")
        test(get_values(step_result), {
            stock_a: 10 + (flow_should_be_present ? expected_change_in_value : 0),
            action_change_a: expected_change_in_value,
            flow_into_a: flow_should_be_present ? expected_change_in_value : undefined,
        }, "step result 2 values")
        test(results.step_results.length, 0, "should have assessed all step results")

        step_result = results.simulation_result
        test(step_result.current_step, 3, "step result 3, current_step")
        test(step_result.current_time, 13, "step result 3, current_time")
        test(get_values(step_result), {
            stock_a: 10 + expected_change_in_value,
            action_change_a: 0,
            flow_into_a: flow_should_be_present ? 0 : undefined,
        }, "step result 3 values")
    }


    await describe("actions as SimulationJS variables", async () =>
    {
        const { data, get_values, make_model_manually } = fixture_model_data__single_stock__action_variable()

        await describe("manually make model", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
            }, model_config)

            make_model_manually(wrapped_model)

            const results = await run_simulation(wrapped_model)
            assess_results(results, get_values, true)
        })

        await describe("make model automatically", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
                data,
            }, model_config)

            const results = await run_simulation(wrapped_model)
            assess_results(results, get_values, true)
        })

        await describe("make model automatically, increase by a different amount", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
                data,
            }, model_config)

            const change_in_value = 5
            const results = await run_simulation(wrapped_model, change_in_value)
            assess_results(results, get_values, true, change_in_value)
        })
    })


    await describe("actions as SimulationJS variable + action", async () =>
    {
        const { data, get_values, make_model_manually } = fixture_model_data__single_stock__action_calc()

        await describe("manually make model", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
            }, model_config)

            make_model_manually(wrapped_model)

            const results = await run_simulation(wrapped_model)
            assess_results(results, get_values, false)
        })

        await describe("make model automatically", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
                data,
            }, model_config)

            const results = await run_simulation(wrapped_model)
            assess_results(results, get_values, false)
        })

        await describe("make model automatically, increase by a different amount", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
                data,
            }, model_config)

            const change_in_value = 5
            const results = await run_simulation(wrapped_model, change_in_value)
            assess_results(results, get_values, false, change_in_value)
        })
    })


    async function run_two_stocks_simulation(wrapped_model: WrappedModel)
    {
        const run_simulation = create_deferred_promise<{ step_results: SimulationStepResult[], simulation_result: ExtendedSimulationResult }>()

        const step_results: SimulationStepResult[] = []
        const on_simulation_step_completed: OnSimulationStepCompletedFunction = step_result => {
            step_results.push(step_result)

            if ([1, 3].includes(step_results.length))
            {
                wrapped_model.factory_trigger_action(IDs.action__move_a_to_b)(9)
            }

            return undefined
        }

        wrapped_model.run_simulation({
            on_simulation_step_completed,
            on_simulation_completed: simulation_result => run_simulation.resolve({ step_results, simulation_result }),
        })

        const results = await run_simulation.promise
        return results
    }


    function assess_results_for_two_stocks(results: {
        step_results: SimulationStepResult[];
        simulation_result: ExtendedSimulationResult;
    }, get_values: (step_result: SimulationStepResult) => {
        stock_a: ModelValue | undefined;
        stock_b: ModelValue | undefined;
        action_move_a_to_b: ModelValue | undefined;
    })
    {
        test(results.step_results.length, 4, "step_results count, note that we miss the initial state and the final step & state, hence being two less than the number of time steps in the simulation")
        test(results.simulation_result._data.times.length, 6, "number of simulation time steps")

        let step_result: SimulationStepResult = results.step_results.shift()!
        test(step_result.current_step, 1, "step result 1, current_step")
        test(step_result.current_time, 11, "step result 1, current_time")
        test(get_values(step_result), {
            stock_a: 10,
            stock_b: 0,
            action_move_a_to_b: 0,
        }, "step result 1 values")

        step_result = results.step_results.shift()!
        test(step_result.current_step, 2, "step result 2, current_step")
        test(step_result.current_time, 12, "step result 2, current_time")
        test(get_values(step_result), {
            stock_a: 10,
            stock_b: 0,
            action_move_a_to_b: 9,
        }, "step result 2 values")

        step_result = results.step_results.shift()!
        test(step_result.current_step, 3, "step result 3, current_step")
        test(step_result.current_time, 13, "step result 3, current_time")
        test(get_values(step_result), {
            stock_a: 1,
            stock_b: 9,
            action_move_a_to_b: 0,
        }, "step result 3 values")

        step_result = results.step_results.shift()!
        test(step_result.current_step, 4, "step result 4, current_step")
        test(step_result.current_time, 14, "step result 4, current_time")
        test(get_values(step_result), {
            stock_a: 1,
            stock_b: 9,
            action_move_a_to_b: 9,
        }, "step result 4 values")

        test(results.step_results.length, 0, "should have assessed all step results")

        step_result = results.simulation_result
        test(step_result.current_step, 5, "step result 5, current_step")
        test(step_result.current_time, 15, "step result 5, current_time")
        test(get_values(step_result), {
            stock_a: 0,
            stock_b: 10,
            action_move_a_to_b: 0,
        }, "step result 5 values")
    }


    await describe("actions as SimulationJS variable + action effecting two stocks", async () =>
    {
        const { data, get_values, make_model_manually } = fixture_model_data__two_stocks__action_calc()

        await describe("manually make model", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
            }, model_config5)

            make_model_manually(wrapped_model)

            const results = await run_two_stocks_simulation(wrapped_model)
            assess_results_for_two_stocks(results, get_values)
        })

        await describe("make model automatically", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
                data,
            }, model_config5)

            const results = await run_two_stocks_simulation(wrapped_model)
            assess_results_for_two_stocks(results, get_values)
        })
    })


    function fixture_model_data__state_as_stock_and_variable()
    {
        const dam_height = 3
        const dam_area = 20
        const dam_width = 10
        const data: GetItemsReturn<SimplifiedWComponentsValueById> = {
            value: {
                statev2: {
                    "20000000-0000-4000-a000-000000000000":
                    {
                        title: "Water height",
                        state: "",
                        calculation: `[10000000-0000-4000-a000-000000000000]/${dam_area}`,
                        linked_ids: [
                            "10000000-0000-4000-a000-000000000000",
                        ],
                        simulationjs_variable: true,
                    },
                    // This is placed after the variable to ensure that this is
                    // created before the variable to avoid the error
                    // "Error: Node not found for id: "10000000-0000-4000-a000-000000000000""
                    "10000000-0000-4000-a000-000000000000":
                    {
                        title: "Water in dam",
                        state: 100,
                    },
                },
                causal_link: {
                    "30000000-0000-4000-a000-000000000000":
                    {
                        title: "Flow out of dam over wall",
                        effect: `MAX([20000000-0000-4000-a000-000000000000]-${dam_height},0)*${dam_width}`,
                        from_id: "10000000-0000-4000-a000-000000000000",
                    },
                },
                action: {},
            },
            error: undefined,
        }

        function get_values(step_result: SimulationStepResult)
        {
            return {
                stock_water_in_dam: step_result.values["10000000-0000-4000-a000-000000000000"],
                variable_water_height: step_result.values["20000000-0000-4000-a000-000000000000"],
                flow_out_of_dam_over_wall: step_result.values["30000000-0000-4000-a000-000000000000"],
            }
        }

        return { data, get_values }
    }


    async function run_state_as_stock_and_variable_model(wrapped_model: WrappedModel)
    {
        const run_simulation = create_deferred_promise<{ step_results: SimulationStepResult[], simulation_result: ExtendedSimulationResult }>()

        const step_results: SimulationStepResult[] = []
        const on_simulation_step_completed: OnSimulationStepCompletedFunction = step_result => {
            step_results.push(step_result)
            return undefined
        }

        wrapped_model.run_simulation({
            on_simulation_step_completed,
            on_simulation_completed: simulation_result => run_simulation.resolve({ step_results, simulation_result }),
        })

        const results = await run_simulation.promise
        return results
    }


    await describe("state wcomponents as SimulationJS stocks and variables", async () =>
    {
        const { data, get_values } = fixture_model_data__state_as_stock_and_variable()

        await describe("make model automatically", async () =>
        {
            const wrapped_model = make_wrapped_model({
                target_refresh_rate: 100,
                data,
            }, model_config5)

            const results = await run_state_as_stock_and_variable_model(wrapped_model)

            test(results.step_results.length, 4, "step_results count, note that we miss the initial state and the final step & state, hence being two less than the number of time steps in the simulation")
            test(results.simulation_result._data.times.length, 6, "number of simulation time steps")

            let step_result: SimulationStepResult = results.step_results.shift()!
            test(step_result.current_step, 1, "step result 1, current_step")
            test(step_result.current_time, 11, "step result 1, current_time")
            test(get_values(step_result), {
                stock_water_in_dam: 80,
                variable_water_height: 4,
                flow_out_of_dam_over_wall: 10,
            }, "step result 1 values")

            step_result = results.step_results.shift()!
            test(step_result.current_step, 2, "step result 2, current_step")
            test(step_result.current_time, 12, "step result 2, current_time")
            test(get_values(step_result), {
                stock_water_in_dam: 70,
                variable_water_height: 3.5,
                flow_out_of_dam_over_wall: 5,
            }, "step result 2 values")

            step_result = results.step_results.shift()!
            test(step_result.current_step, 3, "step result 3, current_step")
            test(step_result.current_time, 13, "step result 3, current_time")
            test(get_values(step_result), {
                stock_water_in_dam: 65,
                variable_water_height: 3.25,
                flow_out_of_dam_over_wall: 2.5,
            }, "step result 3 values")

            step_result = results.step_results.shift()!
            test(step_result.current_step, 4, "step result 4, current_step")
            test(step_result.current_time, 14, "step result 4, current_time")
            test(get_values(step_result), {
                stock_water_in_dam: 62.5,
                variable_water_height: 3.125,
                flow_out_of_dam_over_wall: 1.25,
            }, "step result 4 values")

            test(results.step_results.length, 0, "should have assessed all step results")

            step_result = results.simulation_result
            test(step_result.current_step, 5, "step result 5, current_step")
            test(step_result.current_time, 15, "step result 5, current_time")
            test(get_values(step_result), {
                stock_water_in_dam: 61.25,
                variable_water_height: 3.0625,
                flow_out_of_dam_over_wall: 0.625,
            }, "step result 5 values")
        })
    })
})

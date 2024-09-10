import { useEffect, useMemo, useRef, useState } from "preact/hooks"

import "./app.css"
import { make_model_stepper, ModelStepResult } from "./make_model_stepper3"
import { IDS } from "./data/get_data"
// import { SimulationResult2 } from "./simulation2/simulation2"
// import { WComponentNode } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"


const TARGET_REFRESH_RATE = 30 // Hz

export function DemoAppAddOneToStockV3 () {
    const model_stepper = useMemo(() =>
    {
        const wrapped_model = make_model_stepper({target_refresh_rate: TARGET_REFRESH_RATE})

        wrapped_model.add_stock({ wcomponent_id: IDS.stock_a, name: "Stock A", initial: 100 })
        wrapped_model.add_stock({ wcomponent_id: IDS.stock_b, name: "Stock B", initial: 10 })
        const action_component__increase_a = wrapped_model.add_variable({ wcomponent_id: IDS.variable__action_increase_a, name: "Action: Increase Stock A", value: 0, is_action: true })
        const action_component__move_a_to_b = wrapped_model.add_variable({ wcomponent_id: IDS.variable__action_move_a_to_b, name: "Action: Move A to B", value: 0, is_action: true })
        wrapped_model.add_flow({
            wcomponent_id: IDS.flow__flow_into_a,
            name: "Flow into A",
            flow_rate: `[${action_component__increase_a.name}]`,
            from_id: undefined,
            to_id: IDS.stock_a,
            linked_ids: [IDS.variable__action_increase_a],
        })
        wrapped_model.add_flow({
            wcomponent_id: IDS.flow__flow_a_to_b,
            name: "Flow A to B",
            flow_rate: `[${action_component__move_a_to_b.name}]`,
            from_id: IDS.stock_a,
            to_id: IDS.stock_b,
            linked_ids: [IDS.variable__action_move_a_to_b],
        })

        return wrapped_model
    }, [])

    // const created_at_date = "2024-05-28"
    // const created_at_time = "11:22:59"
    // http://localhost:5173/app/#wcomponents/17edbf36-ad5b-4936-b3c5-7d803741c678/&storage_location=1&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1218&y=-1538&z=0&zoom=68&sdate=2024-03-24&stime=22:42:19&cdate=2024-05-24&ctime=11:22:59

    const [current_time, set_current_time] = useState(model_stepper.get_current_time())
    const [stock_a, set_stock_a] = useState(model_stepper.get_latest_state_by_id(IDS.stock_a))
    const [stock_b, set_stock_b] = useState(model_stepper.get_latest_state_by_id(IDS.stock_b))
    const past_actions_taken = useRef<{step: number, actions_taken: {[action_id: string]: number}}[]>([])
    const actions_taken = useRef<{[action_id: string]: number}>({})

//   const start_time_ms = useRef(new Date().getTime())
//   const current_simulation_step = useRef(0)

    useEffect(() => model_stepper.run_simulation({
        target_refresh_rate: TARGET_REFRESH_RATE,
        on_simulation_step_completed: (result: ModelStepResult) =>
        {
            set_current_time(result.current_time)
            set_stock_a(result.values[IDS.stock_a])
            set_stock_b(result.values[IDS.stock_b])

            const { set_value } = result
            if (!set_value) return

            // Reset previously taken actions
            const last_actions_taken = past_actions_taken.current[past_actions_taken.current.length - 1]
            if (last_actions_taken && (last_actions_taken.step + 1) === result.current_step)
            {
                Object.keys(last_actions_taken.actions_taken).forEach(action_id =>
                {
                    const action = model_stepper.get_node_from_id(action_id, true)
                    set_value(action, 0)
                })
            }

            // Apply actions taken
            const actions_taken_list = Object.entries(actions_taken.current)

            actions_taken_list.forEach(([action_id, value]) =>
            {
                const action = model_stepper.get_node_from_id(action_id, true)
                set_value(action, value)
            })

            if (actions_taken_list.length)
            {
                past_actions_taken.current.push({ step: result.current_step, actions_taken: actions_taken.current })
                actions_taken.current = {}
            }
        }
    }), [])


    const action__increase_stock_a = useMemo(model_stepper.make_apply_action(
        actions_taken,
        IDS.variable__action_increase_a,
        TARGET_REFRESH_RATE
    ), [TARGET_REFRESH_RATE])

    const action__move_a_to_b = useMemo(model_stepper.make_apply_action(
        actions_taken,
        IDS.variable__action_move_a_to_b,
        TARGET_REFRESH_RATE
    ), [TARGET_REFRESH_RATE])

    return <>
        <div class="card">
            This is an implementation of&nbsp;
            <a
                href="https://insightmaker.com/insight/UGp2Y64uh2Pn2euRc9JSc"
                target="_blank"
            >
                Simple Stock Actions v1
            </a>
        </div>

        <div class="card">
            <div>Current time is {current_time.toFixed(1)}</div>
            <div>Stock A is {round_to_5_dp(stock_a)}</div>
            <div>Stock B is {round_to_5_dp(stock_b)}</div>

            <button onClick={action__increase_stock_a}>
                Increase stock A
            </button>

            <button onClick={action__move_a_to_b}>
                Move A to B
            </button>
        </div>
    </>
}


function round_to_5_dp (value: number | string | undefined)
{
    if (typeof value === "string" || value === undefined) return value
    const new_value = Math.round(value * 100000) / 100000
    return `${new_value}${value !== new_value ? " (rounded)" : ""}`
}

import { useEffect, useMemo, useRef, useState } from "preact/hooks"

import "./app.css"
import { make_model_stepper, ModelStepResult } from "./make_model_stepper3"
import { IDS_v4 } from "./data/get_data"


const TARGET_REFRESH_RATE = 30 // Hz

export function DemoAppAddOneToStockV4 () {
    const model_stepper = useMemo(() => {
        const wrapped_model = make_model_stepper({target_refresh_rate: TARGET_REFRESH_RATE})

        const stock_a = wrapped_model.add_stock({ wcomponent_id: IDS_v4.stock__state_a, name: "Stock A", initial: 100 })
        const stock_b = wrapped_model.add_stock({ wcomponent_id: IDS_v4.stock__state_b, name: "Stock B", initial: 10 })
        const action_component__increase_a = wrapped_model.add_variable({ wcomponent_id: IDS_v4.variable__action_increase_a, name: "Action: Increase Stock A", value: 0, is_action: true })
        const action_component__move_a_to_b = wrapped_model.add_variable({ wcomponent_id: IDS_v4.variable__action_move_a_to_b, name: "Action: Move A to B", value: 0, is_action: true })
        wrapped_model.add_action({
            wcomponent_id: IDS_v4.action__action_increase_stock_a,
            name: "Increase Stock A",
            action: `[${stock_a.name}] <- [${stock_a.name}] + [${action_component__increase_a.name}]`,
            trigger_value: `[${action_component__increase_a.name}]`,
            linked_ids: [
                IDS_v4.variable__action_increase_a,
                IDS_v4.stock__state_a,
            ],
        })
        wrapped_model.add_action({
            wcomponent_id: IDS_v4.action__action_move_a_to_b,
            name: "Move A to B",
            action: `[${stock_a.name}] <- [${stock_a.name}] - [${action_component__move_a_to_b.name}]; [${stock_b.name}] <- [${stock_b.name}] + [${action_component__move_a_to_b.name}]`,
            trigger_value: `[${action_component__move_a_to_b.name}]`,
            linked_ids: [
                IDS_v4.variable__action_move_a_to_b,
                IDS_v4.stock__state_a,
                IDS_v4.stock__state_b,
            ],
        })

        return wrapped_model
    }, [])

    const [current_time, set_current_time] = useState(model_stepper.get_current_time())
    const [stock_a, set_stock_a] = useState(model_stepper.get_latest_state_by_id(IDS_v4.stock__state_a))
    const [stock_b, set_stock_b] = useState(model_stepper.get_latest_state_by_id(IDS_v4.stock__state_b))
    const past_actions_taken = useRef<{step: number, actions_taken: {[action_id: string]: number}}[]>([])
    const actions_taken = useRef<{[action_id: string]: number}>({})

    useEffect(() => model_stepper.run_simulation({
        target_refresh_rate: TARGET_REFRESH_RATE,
        on_simulation_step_completed: (result: ModelStepResult) =>
        {
            set_current_time(result.current_time)
            set_stock_a(result.values[IDS_v4.stock__state_a])
            set_stock_b(result.values[IDS_v4.stock__state_b])

            const { set_value } = result
            if (!set_value) return

            // Reset previously taken actions
            const last_actions_taken = past_actions_taken.current[past_actions_taken.current.length - 1]
            if (last_actions_taken && (last_actions_taken.step + 2) === result.current_step)
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
        IDS_v4.variable__action_increase_a,
    ), [])

    const action__move_a_to_b = useMemo(model_stepper.make_apply_action(
        actions_taken,
        IDS_v4.variable__action_move_a_to_b,
    ), [])

    return <>
        <div class="card">
            This is an implementation of&nbsp;
            <a
                href="https://insightmaker.com/insight/6KLn6dwkVWUYRTdZStySyk"
                target="_blank"
            >
                Simple Stock Actions v4
            </a>
        </div>

        <div class="card">
            <div>Current time is {current_time.toFixed(1)}</div>
            <div>Stock A is {stock_a}</div>
            <div>Stock B is {stock_b}</div>

            <button onClick={action__increase_stock_a}>
                Increase stock A
            </button>

            <button onClick={action__move_a_to_b}>
                Move A to B
            </button>
        </div>
    </>
}

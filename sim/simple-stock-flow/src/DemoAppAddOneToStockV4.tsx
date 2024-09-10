import { useEffect, useMemo, useRef, useState } from "preact/hooks"

import "./app.css"
import { make_model_stepper, ModelStepResult } from "./make_model_stepper3"
import { IDS } from "./data/get_data"


const TARGET_REFRESH_RATE = 30 // Hz

export function DemoAppAddOneToStockV4 () {
    const model_stepper = useMemo(() => make_model_stepper({target_refresh_rate: TARGET_REFRESH_RATE}), [])

    const [current_time, set_current_time] = useState(model_stepper.get_current_time())
    const [stock_a, set_stock_a] = useState(model_stepper.get_latest_state_by_id(IDS.state_component__stock_a))
    const [stock_b, set_stock_b] = useState(model_stepper.get_latest_state_by_id(IDS.state_component__stock_b))
    const past_actions_taken = useRef<{step: number, actions_taken: {[action_id: string]: number}}[]>([])
    const actions_taken = useRef<{[action_id: string]: number}>({})

    useEffect(() => model_stepper.run_simulation({
        target_refresh_rate: TARGET_REFRESH_RATE,
        on_simulation_step_completed: (result: ModelStepResult) =>
        {
            set_current_time(result.current_time)
            set_stock_a(result.values[IDS.state_component__stock_a])
            set_stock_b(result.values[IDS.state_component__stock_b])

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
        IDS.variable_component__action_increase_a,
        TARGET_REFRESH_RATE
    ), [TARGET_REFRESH_RATE])

    const action__move_a_to_b = useMemo(model_stepper.make_apply_action(
        actions_taken,
        IDS.variable_component__action_move_a_to_b,
        TARGET_REFRESH_RATE
    ), [TARGET_REFRESH_RATE])

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

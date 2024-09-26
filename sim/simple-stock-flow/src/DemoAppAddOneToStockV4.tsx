import { useEffect, useMemo, useRef, useState } from "preact/hooks"

import { make_model_stepper, ModelStepper, ModelStepResult } from "./make_model_stepper3"
import { IDS_v4 } from "./data/get_data"
import { GetItemsReturn } from "./data_curator/src/state/sync/supabase/get_items"
import { get_wcomponents_values_by_id, SimplifiedWComponentsValueById } from "./data/get_wcomponents_values_by_id"
import { get_supabase } from "./data_curator/src/supabase/get_supabase"
import { supabase_get_wcomponents } from "./data_curator/src/state/sync/supabase/wcomponent"
import { WComponentsById } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"


const TARGET_REFRESH_RATE = 30 // Hz
const supabase = get_supabase()
const cached_data: GetItemsReturn<SimplifiedWComponentsValueById> = {
    value: {
        statev2: {
            "3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75": {
                state: 5
            },
            "e429827b-3b12-475b-bcd8-9afd6f4b9973": {
                state: 2
            }
        },
        causal_link: {},
        action: {
            "779e1a1b-b590-4a9f-b1a8-cfccb543b807": {
                calculation: "[3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75] <- [3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75] + [779e1a1b-b590-4a9f-b1a8-cfccb543b807]"
            },
            "8b65e686-2926-4f89-8919-79d450ac682f": {
                calculation: "Stock_A2_available <- max([3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75],0)\\nStock_A2_to_move <- min(Stock_A2_available,[8b65e686-2926-4f89-8919-79d450ac682f])\\n[e429827b-3b12-475b-bcd8-9afd6f4b9973] <- [e429827b-3b12-475b-bcd8-9afd6f4b9973] + Stock_A2_to_move\\n[3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75] <- [3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75] - Stock_A2_to_move"
            }
        }
    },
    error: undefined,
}


export function DemoAppAddOneToStockV4 () {
    const [data, set_data] = useState<GetItemsReturn<SimplifiedWComponentsValueById> | undefined>(cached_data)


    useEffect(() => {
        if (data) return

        const fetch_data = async () => {
            const ids = Object.values(IDS_v4)
            const wcomponents_response = await supabase_get_wcomponents({ supabase, base_id: undefined, all_bases: true, ids })

            const wcomponents_by_id: WComponentsById = {}
            wcomponents_response.value.forEach(wcomponent => wcomponents_by_id[wcomponent.id] = wcomponent)

            const wcomponents_values_by_id = get_wcomponents_values_by_id(wcomponents_by_id)

            const wcomponents_by_id_response: GetItemsReturn<SimplifiedWComponentsValueById> = {
                value: wcomponents_values_by_id,
                error: wcomponents_response.error,
            }
            set_data(wcomponents_by_id_response)
        }

        fetch_data()
    }, [data])


    const model_stepper: ModelStepper | undefined = useMemo(() =>
    {
        if (data === undefined || data.error) return undefined

        const wrapped_model = make_model_stepper({target_refresh_rate: TARGET_REFRESH_RATE})

        const stock_a_value = data.value.statev2[IDS_v4.stock__state_a]
        const stock_b_value = data.value.statev2[IDS_v4.stock__state_b]

        const stock_a = wrapped_model.add_stock({
            wcomponent_id: IDS_v4.stock__state_a,
            name: IDS_v4.stock__state_a,
            initial: stock_a_value?.state || 100,
        })
        const stock_b = wrapped_model.add_stock({
            wcomponent_id: IDS_v4.stock__state_b,
            name: IDS_v4.stock__state_b,
            initial: stock_b_value?.state || 100,
        })

        const action_component__increase_a = wrapped_model.add_variable({
            wcomponent_id: IDS_v4.action__action_increase_stock_a,
            name: IDS_v4.action__action_increase_stock_a,
            value: 0,
            is_action: true,
        })
        const action_component__move_a_to_b = wrapped_model.add_variable({
            wcomponent_id: IDS_v4.action__action_move_a_to_b,
            name: IDS_v4.action__action_move_a_to_b,
            value: 0,
            is_action: true,
        })

        const action_increase_stock_a_value = data.value.action[IDS_v4.action__action_increase_stock_a]
        const action_move_a_to_b_value = data.value.action[IDS_v4.action__action_move_a_to_b]

        wrapped_model.add_action({
            wcomponent_id: IDS_v4.action__action_increase_stock_a + "_action",
            name: "Add 1 to A",
            action: action_increase_stock_a_value?.calculation || "",
            trigger_value: `[${action_component__increase_a.name}]`,
            linked_ids: [
                IDS_v4.action__action_increase_stock_a,
                IDS_v4.stock__state_a,
            ],
        })
        wrapped_model.add_action({
            wcomponent_id: IDS_v4.action__action_move_a_to_b + "_action",
            name: "Move A to B",
            action: action_move_a_to_b_value?.calculation || "",
            trigger_value: `[${action_component__move_a_to_b.name}]`,
            linked_ids: [
                IDS_v4.action__action_move_a_to_b,
                IDS_v4.stock__state_a,
                IDS_v4.stock__state_b,
            ],
        })

        return wrapped_model
    }, [data])


    if (model_stepper) return <AppAddOneToStockV4
        model_stepper={model_stepper}
        trigger_fetching_live_data={() => set_data(undefined)}
    />

    if (data?.error) return <div>Error: {data.error.message}</div>
    return <div>Loading...</div>
}


function AppAddOneToStockV4 (props: { model_stepper: ModelStepper, trigger_fetching_live_data: () => void })
{
    const { model_stepper } = props

    const [current_time, set_current_time] = useState(model_stepper.get_current_time())
    const [stock_a, set_stock_a] = useState(model_stepper.get_latest_state_by_id(IDS_v4.stock__state_a))
    const [stock_b, set_stock_b] = useState(model_stepper.get_latest_state_by_id(IDS_v4.stock__state_b))
    const past_actions_taken = useRef<{step: number, actions_taken: {[action_id: string]: number}}[]>([])
    const actions_taken = useRef<{[action_id: string]: number}>({})

    useEffect(() => model_stepper.run_simulation((result: ModelStepResult) =>
        {
            set_current_time(result.current_time)
            set_stock_a(result.values[IDS_v4.stock__state_a])
            set_stock_b(result.values[IDS_v4.stock__state_b])

            const { set_value } = result
            if (!set_value) return { reason_to_stop: "Error: no set_value" }

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

            return undefined
        }), [])


    const action__increase_stock_a = useMemo(model_stepper.make_apply_action(
        actions_taken,
        IDS_v4.action__action_increase_stock_a,
    ), [])

    const action__move_a_to_b = useMemo(model_stepper.make_apply_action(
        actions_taken,
        IDS_v4.action__action_move_a_to_b,
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
            <br />
            <button onClick={() => props.trigger_fetching_live_data()}>
                Refresh data from DataCurator
            </button>
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

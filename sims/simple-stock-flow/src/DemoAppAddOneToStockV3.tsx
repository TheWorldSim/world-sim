import { useEffect, useMemo, useRef, useState } from "preact/hooks"

import { make_model_stepper, ModelStepper, ModelStepResult } from "./make_model_stepper"
import { IDS_v3 } from "./data/get_data"
import { supabase_get_wcomponents } from "./data_curator/src/state/sync/supabase/wcomponent"
import { WComponentsById } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { GetItemsReturn } from "./data_curator/src/state/sync/supabase/get_items"
import {
    get_wcomponents_values_by_id,
    SimplifiedWComponentsValueById,
} from "./data/get_wcomponents_values_by_id"
import { get_supabase } from "./data_curator/src/supabase/get_supabase"


const TARGET_REFRESH_RATE = 30 // Hz
const supabase = get_supabase()
const cached_data: GetItemsReturn<SimplifiedWComponentsValueById> = {
    value: {
        statev2: {
            "17edbf36-ad5b-4936-b3c5-7d803741c678": {
                state: 5
            },
            "b644e33f-c00f-4a50-acc4-f158e4e11be5": {
                state: 2
            }
        },
        causal_link: {
            "a8ca022f-1f52-4f8f-b9b2-045f77c65eea": {
                effect: "[2dc650ae-8458-47b6-be0e-6ad1cab3cd4d]"
            },
            "c31be0d8-6ce5-4db2-8b4b-1d13ed48a869": {
                effect: "[8ecb8d21-4803-4028-9341-b7cd59b56cda]"
            }
        },
        action: {
            "2dc650ae-8458-47b6-be0e-6ad1cab3cd4d": {
                calculation: ""
            },
            "8ecb8d21-4803-4028-9341-b7cd59b56cda": {
                calculation: ""
            }
        }
    },
    error: undefined,
}


export function DemoAppAddOneToStockV3 () {
    const [data, set_data] = useState<GetItemsReturn<SimplifiedWComponentsValueById> | undefined>(cached_data)

    useEffect(() => {
        if (data) return

        const fetch_data = async () => {
            const ids = Object.values(IDS_v3)
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

        const stock_a_value = data.value.statev2[IDS_v3.stock__state_a]
        const stock_b_value = data.value.statev2[IDS_v3.stock__state_b]

        wrapped_model.add_stock({
            wcomponent_id: IDS_v3.stock__state_a,
            name: "Stock A",
            initial: stock_a_value?.state || 100,
        })
        wrapped_model.add_stock({
            wcomponent_id: IDS_v3.stock__state_b,
            name: "Stock B",
            initial: stock_b_value?.state || 10,
        })

        const action_component__increase_a = wrapped_model.add_variable({
            wcomponent_id: IDS_v3.variable__action_increase_a,
            name: IDS_v3.variable__action_increase_a,
            value: 0,
            is_action: true,
        })
        const action_component__move_a_to_b = wrapped_model.add_variable({
            wcomponent_id: IDS_v3.variable__action_move_a_to_b,
            name: IDS_v3.variable__action_move_a_to_b,
            value: 0,
            is_action: true,
        })

        wrapped_model.add_flow({
            wcomponent_id: IDS_v3.flow__flow_into_a,
            name: "Flow into A",
            flow_rate: data.value.causal_link[IDS_v3.flow__flow_into_a]?.effect || "",
            from_id: undefined,
            to_id: IDS_v3.stock__state_a,
            linked_ids: [IDS_v3.variable__action_increase_a],
        })
        wrapped_model.add_flow({
            wcomponent_id: IDS_v3.flow__flow_a_to_b,
            name: "Flow A to B",
            flow_rate: data.value.causal_link[IDS_v3.flow__flow_a_to_b]?.effect || "",
            from_id: IDS_v3.stock__state_a,
            to_id: IDS_v3.stock__state_b,
            linked_ids: [IDS_v3.variable__action_move_a_to_b],
        })

        return wrapped_model
    }, [data])


    if (model_stepper) return <AppAddOneToStockV3
        model_stepper={model_stepper}
        trigger_fetching_live_data={() => set_data(undefined)}
    />
    if (data?.error) return <div>Error: {data.error.message}</div>
    return <div>Loading...</div>
}


function AppAddOneToStockV3(props: { model_stepper: ModelStepper, trigger_fetching_live_data: () => void })
{
    const { model_stepper } = props

    // const created_at_date = "2024-05-28"
    // const created_at_time = "11:22:59"
    // http://localhost:5173/app/#wcomponents/17edbf36-ad5b-4936-b3c5-7d803741c678/&storage_location=1&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1218&y=-1538&z=0&zoom=68&sdate=2024-03-24&stime=22:42:19&cdate=2024-05-24&ctime=11:22:59

    const [current_time, set_current_time] = useState(model_stepper.get_current_time())
    const [stock_a, set_stock_a] = useState(model_stepper.get_latest_state_by_id(IDS_v3.stock__state_a))
    const [stock_b, set_stock_b] = useState(model_stepper.get_latest_state_by_id(IDS_v3.stock__state_b))
    const past_actions_taken = useRef<{step: number, actions_taken: {[action_id: string]: number}}[]>([])
    const actions_taken = useRef<{[action_id: string]: number}>({})


    useEffect(() => model_stepper.run_simulation((result: ModelStepResult) =>
        {
            set_current_time(result.current_time)
            set_stock_a(result.values[IDS_v3.stock__state_a])
            set_stock_b(result.values[IDS_v3.stock__state_b])

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
        IDS_v3.variable__action_increase_a,
        TARGET_REFRESH_RATE
    ), [TARGET_REFRESH_RATE])

    const action__move_a_to_b = useMemo(model_stepper.make_apply_action(
        actions_taken,
        IDS_v3.variable__action_move_a_to_b,
        TARGET_REFRESH_RATE
    ), [TARGET_REFRESH_RATE])

    return <>
        <div class="card">
            This is an implementation of Simple Stock Actions v1{" "}
            <a
                href="https://insightmaker.com/insight/UGp2Y64uh2Pn2euRc9JSc"
                target="_blank"
            >
                (InsightMaker)
            </a>
            {" "}
            <a
                href="https://datacurator.org/app/#wcomponents/e8a16326-566a-4886-9222-5cbf62147e18/&storage_location=32&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1038&y=-86&zoom=76"
                target="_blank"
            >
                (DataCurator)
            </a>
            <br />
            <button onClick={() => props.trigger_fetching_live_data()}>
                Refresh data from DataCurator
            </button>
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
    const new_value = value.toFixed(1)
    return `${new_value}`//${value !== new_value ? " (rounded)" : ""}`
}

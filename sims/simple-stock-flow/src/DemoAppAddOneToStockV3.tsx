import { useEffect, useMemo, useState } from "preact/hooks"

import { make_wrapped_model, WrappedModel, SimulationStepResult } from "./make_model_stepper"
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
const cached_model_data: GetItemsReturn<SimplifiedWComponentsValueById> = {
    value: {
        statev2: {
            "17edbf36-ad5b-4936-b3c5-7d803741c678": {
                title: "Stock A1",
                state: 5,
            },
            "b644e33f-c00f-4a50-acc4-f158e4e11be5": {
                title: "Stock B1",
                state: 3,
            }
        },
        causal_link: {
            "a8ca022f-1f52-4f8f-b9b2-045f77c65eea": {
                title: "Flow from @@17edbf36-ad5b-4936-b3c5-7d803741c678 to @@b644e33f-c00f-4a50-acc4-f158e4e11be5",
                effect: "[2dc650ae-8458-47b6-be0e-6ad1cab3cd4d]",
                from_id: "17edbf36-ad5b-4936-b3c5-7d803741c678",
                to_id: "b644e33f-c00f-4a50-acc4-f158e4e11be5",
            },
            "c31be0d8-6ce5-4db2-8b4b-1d13ed48a869": {
                title: "Flow into @@17edbf36-ad5b-4936-b3c5-7d803741c678",
                effect: "[8ecb8d21-4803-4028-9341-b7cd59b56cda]",
                to_id: "17edbf36-ad5b-4936-b3c5-7d803741c678",
            }
        },
        action: {
            "2dc650ae-8458-47b6-be0e-6ad1cab3cd4d": {
                title: "Move A1 to B1",
                calculation: "",
            },
            "8ecb8d21-4803-4028-9341-b7cd59b56cda": {
                title: "Increase stock A1",
                calculation: "",
            }
        }
    },
    error: undefined,
}


export function DemoAppAddOneToStockV3 () {
    const [model_data, set_model_data] = useState<GetItemsReturn<SimplifiedWComponentsValueById> | undefined>(cached_model_data)

    useEffect(() => {
        if (model_data) return

        const fetch_model_data = async () => {
            const ids = Object.values(IDS_v3)
            const wcomponents_response = await supabase_get_wcomponents({ supabase, base_id: undefined, all_bases: true, ids })

            const wcomponents_by_id: WComponentsById = {}
            wcomponents_response.value.forEach(wcomponent => wcomponents_by_id[wcomponent.id] = wcomponent)

            const wcomponents_values_by_id = get_wcomponents_values_by_id(wcomponents_by_id)

            const wcomponents_by_id_response: GetItemsReturn<SimplifiedWComponentsValueById> = {
                value: wcomponents_values_by_id,
                error: wcomponents_response.error,
            }
            // console .log(JSON.stringify(wcomponents_by_id_response.value,null,4))
            set_model_data(wcomponents_by_id_response)
        }

        fetch_model_data()
    }, [model_data])


    const wrapped_model: WrappedModel | undefined = useMemo(() =>
    {
        if (model_data === undefined || model_data.error) return undefined

        return make_wrapped_model({target_refresh_rate: TARGET_REFRESH_RATE, data: model_data})
    }, [model_data])


    if (wrapped_model) return <AppAddOneToStockV3
        wrapped_model={wrapped_model}
        trigger_fetching_live_data={() => set_model_data(undefined)}
    />
    if (model_data?.error) return <div>Error: {model_data.error.message}</div>
    return <div>Loading...</div>
}


function AppAddOneToStockV3(props: { wrapped_model: WrappedModel, trigger_fetching_live_data: () => void })
{
    const { wrapped_model } = props

    // const created_at_date = "2024-05-28"
    // const created_at_time = "11:22:59"
    // http://localhost:5173/app/#wcomponents/17edbf36-ad5b-4936-b3c5-7d803741c678/&storage_location=1&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1218&y=-1538&z=0&zoom=68&sdate=2024-03-24&stime=22:42:19&cdate=2024-05-24&ctime=11:22:59

    const [current_time, set_current_time] = useState(wrapped_model.get_current_time())
    const [stock_a, set_stock_a] = useState(wrapped_model.get_latest_state_by_id(IDS_v3.stock__state_a))
    const [stock_b, set_stock_b] = useState(wrapped_model.get_latest_state_by_id(IDS_v3.stock__state_b))


    useEffect(() => wrapped_model.run_simulation({
        on_simulation_step_completed: (result: SimulationStepResult) =>
        {
            set_current_time(result.current_time)
            set_stock_a(result.values[IDS_v3.stock__state_a])
            set_stock_b(result.values[IDS_v3.stock__state_b])

            return undefined
        }
    }), [])


    const action__increase_stock_a = useMemo(() => wrapped_model.factory_trigger_action(
        IDS_v3.variable__action_increase_a,
        TARGET_REFRESH_RATE
    ), [TARGET_REFRESH_RATE])

    const action__move_a_to_b = useMemo(() => wrapped_model.factory_trigger_action(
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

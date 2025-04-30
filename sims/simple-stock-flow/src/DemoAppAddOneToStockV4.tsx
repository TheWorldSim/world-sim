import { useEffect, useMemo, useRef, useState } from "preact/hooks"

import { make_wrapped_model, WrappedModel, SimulationStepResult } from "./make_model_stepper"
import { IDS_v4 } from "./data/get_data"
import { GetItemsReturn } from "./data_curator/src/state/sync/supabase/get_items"
import { get_wcomponents_values_by_id, SimplifiedWComponentsValueById } from "./data/get_wcomponents_values_by_id"
import { get_supabase } from "./data_curator/src/supabase/get_supabase"
import { supabase_get_wcomponents } from "./data_curator/src/state/sync/supabase/wcomponent"
import { WComponentsById } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { dedent } from "./utils/string"


const TARGET_REFRESH_RATE = 30 // Hz
const supabase = get_supabase()
const cached_model_data: GetItemsReturn<SimplifiedWComponentsValueById> = {
    value: {
        statev2: {
            "3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75":
            {
                state: 5,
                title: "Stock A",
            },
            "e429827b-3b12-475b-bcd8-9afd6f4b9973":
            {
                state: 2,
                title: "Stock B",
            }
        },
        causal_link: {},
        action: {
            "779e1a1b-b590-4a9f-b1a8-cfccb543b807":
            {
                title: "Increase stock A",
                calculation: "[3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75] <- [3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75] + [779e1a1b-b590-4a9f-b1a8-cfccb543b807]",
                linked_ids: [
                    "3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75",
                    "779e1a1b-b590-4a9f-b1a8-cfccb543b807",
                ],
            },
            "8b65e686-2926-4f89-8919-79d450ac682f":
            {
                title: "Move A to B",
                calculation: dedent(`
                stock_a_available <- max([3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75],0)
                stock_a_to_move <- min(stock_a_available,[8b65e686-2926-4f89-8919-79d450ac682f])
                [e429827b-3b12-475b-bcd8-9afd6f4b9973] <- [e429827b-3b12-475b-bcd8-9afd6f4b9973] + stock_a_to_move
                [3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75] <- [3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75] - stock_a_to_move
                `),
                linked_ids: [
                    "3d77c2b9-e1cb-4b1b-9e22-66bc0b673f75",
                    "8b65e686-2926-4f89-8919-79d450ac682f",
                    "e429827b-3b12-475b-bcd8-9afd6f4b9973",
                ],
            }
        }
    },
    error: undefined,
}


export function DemoAppAddOneToStockV4 () {
    const [model_data, set_model_data] = useState<GetItemsReturn<SimplifiedWComponentsValueById> | undefined>(cached_model_data)


    useEffect(() => {
        if (model_data) return

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
            // console .log(JSON.stringify(wcomponents_by_id_response.value,null,4))
            set_model_data(wcomponents_by_id_response)
        }

        fetch_data()
    }, [model_data])


    const wrapped_model: WrappedModel | undefined = useMemo(() =>
    {
        if (model_data === undefined || model_data.error) return undefined

        return make_wrapped_model({target_refresh_rate: TARGET_REFRESH_RATE, data: model_data})
    }, [model_data])


    if (wrapped_model) return <AppAddOneToStockV4
        wrapped_model={wrapped_model}
        trigger_fetching_live_data={() => set_model_data(undefined)}
    />

    if (model_data?.error) return <div>Error: {model_data.error.message}</div>
    return <div>Loading...</div>
}


function AppAddOneToStockV4 (props: { wrapped_model: WrappedModel, trigger_fetching_live_data: () => void })
{
    const { wrapped_model } = props

    const [current_time, set_current_time] = useState(wrapped_model.get_current_time())
    const [stock_a, set_stock_a] = useState(wrapped_model.get_latest_state_by_id(IDS_v4.stock__state_a))
    const [stock_b, set_stock_b] = useState(wrapped_model.get_latest_state_by_id(IDS_v4.stock__state_b))

    useEffect(() => wrapped_model.run_simulation({
        on_simulation_step_completed: (result: SimulationStepResult) =>
        {
            set_current_time(result.current_time)
            set_stock_a(result.values[IDS_v4.stock__state_a])
            set_stock_b(result.values[IDS_v4.stock__state_b])

            return undefined
        }
    }), [])


    const action__increase_stock_a = useMemo(() => wrapped_model.factory_trigger_action(
        IDS_v4.action__action_increase_stock_a,
        TARGET_REFRESH_RATE,
    ), [TARGET_REFRESH_RATE])

    const action__move_a_to_b = useMemo(() => wrapped_model.factory_trigger_action(
        IDS_v4.action__action_move_a_to_b,
        TARGET_REFRESH_RATE,
    ), [TARGET_REFRESH_RATE])

    return <>
        <div class="card">
            This is an implementation of Simple Stock Actions v4{" "}
            <a
                href="https://insightmaker.com/insight/6KLn6dwkVWUYRTdZStySyk"
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

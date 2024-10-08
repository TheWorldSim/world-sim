import { ChangeEvent } from "preact/compat"
import { useEffect, useMemo, useRef, useState } from "preact/hooks"

import { make_model_stepper, ModelStepper, ModelStepResult } from "./make_model_stepper"
import { GetItemsReturn } from "./data_curator/src/state/sync/supabase/get_items"
import { get_wcomponents_values_by_id, SimplifiedWComponentsValueById } from "./data/get_wcomponents_values_by_id"
import { get_supabase } from "./data_curator/src/supabase/get_supabase"
import { supabase_get_wcomponents } from "./data_curator/src/state/sync/supabase/wcomponent"
import { wcomponent_has_VAP_sets, WComponentsById } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { get_composed_wcomponents_by_id } from "./data_curator/src/state/derived/get_composed_wcomponents_by_id"
import { KnowledgeViewWComponentIdEntryMap } from "./data_curator/src/shared/interfaces/knowledge_view"
import { get_created_at_ms } from "./data_curator/src/shared/utils_datetime/utils_datetime"


const TARGET_REFRESH_RATE = 30 // Hz
const supabase = get_supabase()


const IDS__scenario_base = {
    variable__g: "02d4a5f4-4abd-41a2-bb50-e68e358a3169",

    variable__pendulum_1_mass: "297916db-1365-48ad-a8e1-44809a63a599",
    variable__pendulum_2_mass: "b30dda15-ef05-4c42-a392-7d515552d63b",
    variable__pendulum_1_length: "e34796cf-ab9e-47b8-9ea5-20cb00fb611d",
    variable__pendulum_2_length: "b1448ce5-3015-4b78-ad34-24cbb0b85040",
    stock__pendulum_1_angle: "86635a8b-f4f8-4489-8a02-b4f09c0a22ec",
    stock__pendulum_2_angle: "892551e3-bb5b-4957-8104-6f49e578350a",

    variable__pendulum_mass_ratio: "56fcc4f1-29a9-4a97-b629-068b532ec19d",

    stock__pendulum_1_angular_velocity: "28c5c78a-3281-449c-b47b-6f094c15d337",
    stock__pendulum_2_angular_velocity: "8455647b-e016-476a-a6ae-a049deb8bdbb",
    variable__pendulum_1_angular_acceleration: "b264f09c-9a68-489d-b545-44176d1c866b",
    variable__pendulum_2_angular_acceleration: "83f8e273-0702-4bc1-8a84-83bc3c285e8d",

    flow__change_in_pendulum_1_angle: "f7daf461-16ad-4d4a-9b3f-e95247302681",
    flow__change_in_pendulum_1_angular_velocity: "d606c6f2-1774-4e5b-a4c6-2116c99e0e91",
    flow__change_in_pendulum_2_angle: "562bf403-8c34-4f28-b059-3754e9f830ac",
    flow__change_in_pendulum_2_angular_velocity: "95ea2714-7955-4e9f-be96-5ac1fcfe9e93",
}

const IDS__scenario_2 = {
    ...IDS__scenario_base,
    stock__pendulum_1_angle__alternative_value: "0be2bdea-c84f-44f0-8d8e-723e6d7ce9ba",
}

const IDS__ALL = {
    ...IDS__scenario_base,
    ...IDS__scenario_2,
}


enum ScenarioId
{
    base = "base",
    scenario_2 = "scenario_2",
}

interface Scenario
{
    id: ScenarioId
    title: string
    wcomponent_ids: Set<string>
    data: GetItemsReturn<SimplifiedWComponentsValueById>
}

const scenario_base: Scenario = {
    id: ScenarioId.base,
    title: "Base scenario",
    wcomponent_ids: new Set(Object.values(IDS__scenario_base)),
    data: {
        value: {
            statev2: {
                [IDS__scenario_base.variable__g]: {
                    state: 9.81
                },

                [IDS__scenario_base.variable__pendulum_1_mass]: {
                    state: 1
                },
                [IDS__scenario_base.variable__pendulum_2_mass]: {
                    state: 1
                },
                [IDS__scenario_base.variable__pendulum_1_length]: {
                    state: 1.5
                },
                [IDS__scenario_base.variable__pendulum_2_length]: {
                    state: 1
                },
                [IDS__scenario_base.stock__pendulum_1_angle]: {
                    state: 1.5
                },
                [IDS__scenario_base.stock__pendulum_2_angle]: {
                    state: 1.5
                },

                [IDS__scenario_base.variable__pendulum_mass_ratio]: {
                    state: 0.5,
                    calculation: "[b30dda15-ef05-4c42-a392-7d515552d63b] / [297916db-1365-48ad-a8e1-44809a63a599]",
                },

                [IDS__scenario_base.stock__pendulum_1_angular_velocity]: {
                    state: 0
                },
                [IDS__scenario_base.variable__pendulum_1_angular_acceleration]: {
                    state: "",
                    calculation: "term1 <- -(([02d4a5f4-4abd-41a2-bb50-e68e358a3169]/[e34796cf-ab9e-47b8-9ea5-20cb00fb611d])*[86635a8b-f4f8-4489-8a02-b4f09c0a22ec])\nterm2 <- ((([02d4a5f4-4abd-41a2-bb50-e68e358a3169]*[56fcc4f1-29a9-4a97-b629-068b532ec19d])/[e34796cf-ab9e-47b8-9ea5-20cb00fb611d])*[892551e3-bb5b-4957-8104-6f49e578350a])\nterm1 + term2",
                },
                [IDS__scenario_base.stock__pendulum_2_angular_velocity]: {
                    state: 0,
                },
                [IDS__scenario_base.variable__pendulum_2_angular_acceleration]: {
                    state: "",
                    calculation: "term1 <- ([02d4a5f4-4abd-41a2-bb50-e68e358a3169]/[e34796cf-ab9e-47b8-9ea5-20cb00fb611d])*[86635a8b-f4f8-4489-8a02-b4f09c0a22ec]\nterm2a <- [02d4a5f4-4abd-41a2-bb50-e68e358a3169]/[b1448ce5-3015-4b78-ad34-24cbb0b85040]\nterm2b <- ([02d4a5f4-4abd-41a2-bb50-e68e358a3169] *[56fcc4f1-29a9-4a97-b629-068b532ec19d])/[b1448ce5-3015-4b78-ad34-24cbb0b85040]\nterm2c <- ([02d4a5f4-4abd-41a2-bb50-e68e358a3169]*[56fcc4f1-29a9-4a97-b629-068b532ec19d])/[e34796cf-ab9e-47b8-9ea5-20cb00fb611d]\nterm2 <- (term2a + term2b + term2c)*[892551e3-bb5b-4957-8104-6f49e578350a]\nterm1 - term2",
                },
            },
            causal_link: {
                [IDS__scenario_base.flow__change_in_pendulum_2_angle]: {
                    effect: "[8455647b-e016-476a-a6ae-a049deb8bdbb]"
                },
                [IDS__scenario_base.flow__change_in_pendulum_2_angular_velocity]: {
                    effect: "[83f8e273-0702-4bc1-8a84-83bc3c285e8d]"
                },
                [IDS__scenario_base.flow__change_in_pendulum_1_angular_velocity]: {
                    effect: "[b264f09c-9a68-489d-b545-44176d1c866b]"
                },
                [IDS__scenario_base.flow__change_in_pendulum_1_angle]: {
                    effect: "[28c5c78a-3281-449c-b47b-6f094c15d337]"
                }
            },
            action: {}
        },
        error: undefined,
    },
}


const scenario_2_data: GetItemsReturn<SimplifiedWComponentsValueById> = JSON.parse(JSON.stringify(scenario_base.data))
scenario_2_data.value.statev2[IDS__scenario_base.stock__pendulum_1_angle]!.state = 3.14
scenario_2_data.value.statev2[IDS__scenario_base.stock__pendulum_2_angle]!.state = 3.14
const scenario_2: Scenario = {
    id: ScenarioId.scenario_2,
    title: "Scenario 2",
    wcomponent_ids: new Set(Object.values(IDS__scenario_2)),
    data: scenario_2_data,
}


const scenarios: Scenario[] = [scenario_base, scenario_2]
const scenarios_by_id: { [key in ScenarioId]: Scenario } = {
    [ScenarioId.base]: scenario_base,
    [ScenarioId.scenario_2]: scenario_2,
}

export function DemoAppDoublePendulum () {
    const [selected_scenario_id, set_selected_scenario_id] = useState<ScenarioId>(scenario_base.id)
    const [data_needs_refresh, set_data_needs_refresh] = useState(false)
    const [last_data_refresh_datetime_ms, set_last_data_refresh_datetime_ms] = useState(new Date().getTime())
    const selected_scenario = scenarios_by_id[selected_scenario_id]


    useEffect(() => {
        if (!data_needs_refresh) return

        let cancel_data_fetch = false

        const fetch_data = async () => {
            const ids = Object.values(IDS__ALL)
            const wcomponents_response = await supabase_get_wcomponents({
                supabase,
                base_id: undefined,
                all_bases: true,
                ids,
            })
            // Check that this component hasn't been unmounted before continuing
            // further to process the data.
            if (cancel_data_fetch) return

            const wcomponents_by_id: WComponentsById = {}
            let latest_created_at_ms = 0
            wcomponents_response.value.forEach(wcomponent =>
            {
                wcomponents_by_id[wcomponent.id] = wcomponent
                latest_created_at_ms = Math.max(latest_created_at_ms, get_created_at_ms(wcomponent))
                if (wcomponent_has_VAP_sets(wcomponent))
                {
                    (wcomponent.values_and_prediction_sets || []).forEach(VAP_set =>
                    {
                        latest_created_at_ms = Math.max(latest_created_at_ms, get_created_at_ms(VAP_set))
                    })
                }
            })

            scenarios.forEach(scenario =>
            {
                // For now we make our own composed_visible_wc_id_map but later
                // we could get this from DataCurator
                const composed_visible_wc_id_map: KnowledgeViewWComponentIdEntryMap = {}
                wcomponents_response.value.forEach(wcomponent =>
                {
                    if (scenario.wcomponent_ids.has(wcomponent.id)) composed_visible_wc_id_map[wcomponent.id] = { left: 0, top: 0 }
                })

                const composed_wcomponents_by_id = get_composed_wcomponents_by_id({
                    composed_visible_wc_id_map,
                    wcomponents_by_id,
                    created_at_ms: latest_created_at_ms,
                })
                const wcomponents_values_by_id = get_wcomponents_values_by_id(composed_wcomponents_by_id)

                scenario.data = {
                    value: wcomponents_values_by_id,
                    error: wcomponents_response.error,
                }
            })

            set_data_needs_refresh(false)
            set_last_data_refresh_datetime_ms(new Date().getTime())
        }

        fetch_data()

        return () => cancel_data_fetch = true
    }, [data_needs_refresh])


    const model_stepper: ModelStepper | undefined = useMemo(() =>
    {
        if (selected_scenario === undefined || selected_scenario.data.error) return undefined

        const scenario_data = selected_scenario.data


        const variable__g_value = scenario_data.value.statev2[IDS__scenario_base.variable__g]!.state
        const variable__pendulum_1_mass_value = scenario_data.value.statev2[IDS__scenario_base.variable__pendulum_1_mass]!.state
        const variable__pendulum_2_mass_value = scenario_data.value.statev2[IDS__scenario_base.variable__pendulum_2_mass]!.state
        const variable__pendulum_1_length_value = scenario_data.value.statev2[IDS__scenario_base.variable__pendulum_1_length]!.state
        const variable__pendulum_2_length_value = scenario_data.value.statev2[IDS__scenario_base.variable__pendulum_2_length]!.state
        const stock__pendulum_1_angle_value = scenario_data.value.statev2[IDS__scenario_base.stock__pendulum_1_angle]!.state
        const stock__pendulum_2_angle_value = scenario_data.value.statev2[IDS__scenario_base.stock__pendulum_2_angle]!.state
        const variable__pendulum_mass_ratio_value = scenario_data.value.statev2[IDS__scenario_base.variable__pendulum_mass_ratio]!.calculation!
        const stock__pendulum_1_angular_velocity_value = scenario_data.value.statev2[IDS__scenario_base.stock__pendulum_1_angular_velocity]!.state
        const stock__pendulum_2_angular_velocity_value = scenario_data.value.statev2[IDS__scenario_base.stock__pendulum_2_angular_velocity]!.state
        const variable__pendulum_1_angular_acceleration_value = scenario_data.value.statev2[IDS__scenario_base.variable__pendulum_1_angular_acceleration]!.calculation!
        const variable__pendulum_2_angular_acceleration_value = scenario_data.value.statev2[IDS__scenario_base.variable__pendulum_2_angular_acceleration]!.calculation!

        const flow__change_in_pendulum_1_angle_value = scenario_data.value.causal_link[IDS__scenario_base.flow__change_in_pendulum_1_angle]!.effect
        const flow__change_in_pendulum_1_angular_velocity_value = scenario_data.value.causal_link[IDS__scenario_base.flow__change_in_pendulum_1_angular_velocity]!.effect
        const flow__change_in_pendulum_2_angle_value = scenario_data.value.causal_link[IDS__scenario_base.flow__change_in_pendulum_2_angle]!.effect
        const flow__change_in_pendulum_2_angular_velocity_value = scenario_data.value.causal_link[IDS__scenario_base.flow__change_in_pendulum_2_angular_velocity]!.effect


        const wrapped_model = make_model_stepper(
            { target_refresh_rate: TARGET_REFRESH_RATE },
            { algorithm: "RK4" },
        )

        const variable__g = wrapped_model.add_variable({
            wcomponent_id: IDS__scenario_base.variable__g,
            name: IDS__scenario_base.variable__g,
            value: variable__g_value,
        })

        const variable__pendulum_1_mass = wrapped_model.add_variable({
            wcomponent_id: IDS__scenario_base.variable__pendulum_1_mass,
            name: IDS__scenario_base.variable__pendulum_1_mass,
            value: variable__pendulum_1_mass_value,
        })
        const variable__pendulum_2_mass = wrapped_model.add_variable({
            wcomponent_id: IDS__scenario_base.variable__pendulum_2_mass,
            name: IDS__scenario_base.variable__pendulum_2_mass,
            value: variable__pendulum_2_mass_value,
        })
        const variable__pendulum_1_length = wrapped_model.add_variable({
            wcomponent_id: IDS__scenario_base.variable__pendulum_1_length,
            name: IDS__scenario_base.variable__pendulum_1_length,
            value: variable__pendulum_1_length_value,
        })
        const variable__pendulum_2_length = wrapped_model.add_variable({
            wcomponent_id: IDS__scenario_base.variable__pendulum_2_length,
            name: IDS__scenario_base.variable__pendulum_2_length,
            value: variable__pendulum_2_length_value,
        })

        const stock__pendulum_1_angle = wrapped_model.add_stock({
            wcomponent_id: IDS__scenario_base.stock__pendulum_1_angle,
            name: IDS__scenario_base.stock__pendulum_1_angle,
            initial: stock__pendulum_1_angle_value,
        })
        const stock__pendulum_2_angle = wrapped_model.add_stock({
            wcomponent_id: IDS__scenario_base.stock__pendulum_2_angle,
            name: IDS__scenario_base.stock__pendulum_2_angle,
            initial: stock__pendulum_2_angle_value,
        })

        const variable__pendulum_mass_ratio = wrapped_model.add_variable({
            wcomponent_id: IDS__scenario_base.variable__pendulum_mass_ratio,
            name: IDS__scenario_base.variable__pendulum_mass_ratio,
            value: variable__pendulum_mass_ratio_value,
            linked_ids: [IDS__scenario_base.variable__pendulum_1_mass, IDS__scenario_base.variable__pendulum_2_mass],
        })

        const stock__pendulum_1_angular_velocity = wrapped_model.add_stock({
            wcomponent_id: IDS__scenario_base.stock__pendulum_1_angular_velocity,
            name: IDS__scenario_base.stock__pendulum_1_angular_velocity,
            initial: stock__pendulum_1_angular_velocity_value,
        })
        const stock__pendulum_2_angular_velocity = wrapped_model.add_stock({
            wcomponent_id: IDS__scenario_base.stock__pendulum_2_angular_velocity,
            name: IDS__scenario_base.stock__pendulum_2_angular_velocity,
            initial: stock__pendulum_2_angular_velocity_value,
        })
        const variable__pendulum_1_angular_acceleration = wrapped_model.add_variable({
            wcomponent_id: IDS__scenario_base.variable__pendulum_1_angular_acceleration,
            name: IDS__scenario_base.variable__pendulum_1_angular_acceleration,
            value: variable__pendulum_1_angular_acceleration_value,
            linked_ids: [
                IDS__scenario_base.variable__g,
                IDS__scenario_base.variable__pendulum_1_length,
                IDS__scenario_base.stock__pendulum_1_angle,
                IDS__scenario_base.variable__pendulum_mass_ratio,
                IDS__scenario_base.stock__pendulum_2_angle,
            ],
        })
        const variable__pendulum_2_angular_acceleration = wrapped_model.add_variable({
            wcomponent_id: IDS__scenario_base.variable__pendulum_2_angular_acceleration,
            name: IDS__scenario_base.variable__pendulum_2_angular_acceleration,
            value: variable__pendulum_2_angular_acceleration_value,
            linked_ids: [
                IDS__scenario_base.variable__g,
                IDS__scenario_base.variable__pendulum_1_length,
                IDS__scenario_base.stock__pendulum_1_angle,
                IDS__scenario_base.variable__pendulum_mass_ratio,
                IDS__scenario_base.stock__pendulum_2_angle,
                IDS__scenario_base.variable__pendulum_2_length,
            ],
        })

        const flow__change_in_pendulum_1_angle = wrapped_model.add_flow({
            wcomponent_id: IDS__scenario_base.flow__change_in_pendulum_1_angle,
            name: IDS__scenario_base.flow__change_in_pendulum_1_angle,
            flow_rate: flow__change_in_pendulum_1_angle_value,
            only_positive: false,
            from_id: undefined,
            to_id: IDS__scenario_base.stock__pendulum_1_angle,
            linked_ids: [IDS__scenario_base.stock__pendulum_1_angular_velocity],
        })
        const flow__change_in_pendulum_1_angular_velocity = wrapped_model.add_flow({
            wcomponent_id: IDS__scenario_base.flow__change_in_pendulum_1_angular_velocity,
            name: IDS__scenario_base.flow__change_in_pendulum_1_angular_velocity,
            flow_rate: flow__change_in_pendulum_1_angular_velocity_value,
            only_positive: false,
            from_id: undefined,
            to_id: IDS__scenario_base.stock__pendulum_1_angular_velocity,
            linked_ids: [IDS__scenario_base.variable__pendulum_1_angular_acceleration],
        })
        const flow__change_in_pendulum_2_angle = wrapped_model.add_flow({
            wcomponent_id: IDS__scenario_base.flow__change_in_pendulum_2_angle,
            name: IDS__scenario_base.flow__change_in_pendulum_2_angle,
            flow_rate: flow__change_in_pendulum_2_angle_value,
            only_positive: false,
            from_id: undefined,
            to_id: IDS__scenario_base.stock__pendulum_2_angle,
            linked_ids: [IDS__scenario_base.stock__pendulum_2_angular_velocity],
        })
        const flow__change_in_pendulum_2_angular_velocity = wrapped_model.add_flow({
            wcomponent_id: IDS__scenario_base.flow__change_in_pendulum_2_angular_velocity,
            name: IDS__scenario_base.flow__change_in_pendulum_2_angular_velocity,
            flow_rate: flow__change_in_pendulum_2_angular_velocity_value,
            only_positive: false,
            from_id: undefined,
            to_id: IDS__scenario_base.stock__pendulum_2_angular_velocity,
            linked_ids: [IDS__scenario_base.variable__pendulum_2_angular_acceleration],
        })

        return wrapped_model
    }, [last_data_refresh_datetime_ms, selected_scenario])


    if (!selected_scenario) return <div>Error: selected_scenario is undefined</div>
    if (selected_scenario.data.error) return <div>Error: {selected_scenario.data.error.message}</div>
    if (!model_stepper) return <div>Error: model_stepper is undefined</div>

    return <AppDoublePendulum
        selected_scenario={selected_scenario}
        set_selected_scenario_id={set_selected_scenario_id}
        model_stepper={model_stepper}
        trigger_fetching_live_data={() => set_data_needs_refresh(true)}
    />
}


function AppDoublePendulum (props: { selected_scenario: Scenario, set_selected_scenario_id: (scenario_id: ScenarioId) => void, model_stepper: ModelStepper, trigger_fetching_live_data: () => void })
{
    const { model_stepper } = props

    const [current_time, set_current_time] = useState(model_stepper.get_current_time())
    const [pendulum_1_angle, set_pendulum_1_angle] = useState(model_stepper.get_latest_state_by_id(IDS__scenario_base.stock__pendulum_1_angle))
    const [pendulum_2_angle, set_pendulum_2_angle] = useState(model_stepper.get_latest_state_by_id(IDS__scenario_base.stock__pendulum_2_angle))
    const pendulum_1_length = model_stepper.get_latest_state_by_id(IDS__scenario_base.variable__pendulum_1_length)
    const pendulum_2_length = model_stepper.get_latest_state_by_id(IDS__scenario_base.variable__pendulum_2_length)
    const pendulum_1_mass = model_stepper.get_latest_state_by_id(IDS__scenario_base.variable__pendulum_1_mass)
    const pendulum_2_mass = model_stepper.get_latest_state_by_id(IDS__scenario_base.variable__pendulum_2_mass)

    // const [stock_b, set_stock_b] = useState(model_stepper.get_latest_state_by_id(IDS_v4.stock__state_b))
    const past_actions_taken = useRef<{step: number, actions_taken: {[action_id: string]: number}}[]>([])
    const actions_taken = useRef<{[action_id: string]: number}>({})


    // Close previous model stepper
    const previous_model_stepper = useRef<ModelStepper | undefined>()
    if (previous_model_stepper.current !== model_stepper)
    {
        previous_model_stepper.current?.cancel_simulation()
        previous_model_stepper.current = model_stepper
    }


    useEffect(() => model_stepper.run_simulation((result: ModelStepResult) =>
    {
        set_current_time(result.current_time)
        set_pendulum_1_angle(result.values[IDS__scenario_base.stock__pendulum_1_angle])
        set_pendulum_2_angle(result.values[IDS__scenario_base.stock__pendulum_2_angle])

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
    }), [model_stepper])


    // const action__increase_stock_a = useMemo(model_stepper.make_apply_action(
    //     actions_taken,
    //     IDS_v4.action__action_increase_stock_a,
    // ), [])

    // const action__move_a_to_b = useMemo(model_stepper.make_apply_action(
    //     actions_taken,
    //     IDS_v4.action__action_move_a_to_b,
    // ), [])

    const handle_scenario_change = (event: ChangeEvent<HTMLSelectElement>) => {
        const target = event.target as HTMLSelectElement | null
        if (!target) throw new Error(`no event.target`)  // type guard

        // ensure target.value is in ScenarioId
        const scenario_id = target.value as ScenarioId
        if (!(scenario_id in ScenarioId)) return console.error(`Invalid scenario_id: ${scenario_id}`)
        props.set_selected_scenario_id(scenario_id)
    }

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
            <div>
                {/* <label htmlFor="options">Choose a scenario:</label> */}
                <select id="options" value={props.selected_scenario.id} onChange={handle_scenario_change}>
                    {scenarios.map(scenario => <option value={scenario.id}>{scenario.title}</option>)}
                </select>
            </div>
            <br />
            <button onClick={() => props.trigger_fetching_live_data()}>
                Refresh data from DataCurator
            </button>
        </div>

        <div class="card">
            <div>Current time is {current_time.toFixed(1)}</div>
            <div>Pendulum 1: {pendulum_1_length} m, {pendulum_1_mass} kg</div>
            <div>Pendulum 2: {pendulum_2_length} m, {pendulum_2_mass} kg</div>
            <div>Pendulum 1 angle: {as_number(pendulum_1_angle).toFixed(2)}</div>
            <div>Pendulum 2 angle: {as_number(pendulum_2_angle).toFixed(2)}</div>

            {/* <button onClick={action__increase_stock_a}>
                Increase stock A
            </button>

            <button onClick={action__move_a_to_b}>
                Move A to B
            </button> */}
        </div>

        <DoublePendulumCanvas
            pendulum_1_angle={as_number(pendulum_1_angle)}
            pendulum_2_angle={as_number(pendulum_2_angle)}
            pendulum_1_length={as_number(pendulum_1_length)}
            pendulum_2_length={as_number(pendulum_2_length)}
        />
    </>
}


function as_number (value: string | number | undefined): number
{
    if (typeof value === "number") return value
    else throw new Error(`Expected number but got: ${value}`)
}


function DoublePendulumCanvas(props: { pendulum_1_angle: number, pendulum_2_angle: number, pendulum_1_length: number, pendulum_2_length: number })
{
    const canvas_ref = useRef<HTMLCanvasElement | null>(null)

    if (canvas_ref.current) draw(canvas_ref.current, props)

    return <canvas
        id="canvas"
        width="800"
        height="400"
        style={{ border: "1px solid lightgrey" }}
        ref={canvas => canvas_ref.current = canvas}
    />
}


function draw (canvas: HTMLCanvasElement, props: { pendulum_1_angle: number, pendulum_2_angle: number, pendulum_1_length: number, pendulum_2_length: number })
{
    const ctx = canvas.getContext("2d")!
    const width = canvas.width
    const height = canvas.height

    const l1 = props.pendulum_1_length * 70
    const l2 = props.pendulum_2_length * 70
    let theta1 = props.pendulum_1_angle // * (Math.PI / 180)
    let theta2 = props.pendulum_2_angle // * (Math.PI / 180)

    function calculate_energy_in_system ()
    {
        // const potential_energy =
        // const kinetic_energy =
        // return {
        //     potential_energy,
        //     kinetic_energy,
        //     total_energy: potential_energy + kinetic_energy,
        // }
    }

    function draw ()
    {
        const x1 = l1 * Math.sin(theta1)
        const y1 = l1 * Math.cos(theta1)

        const x2 = x1 + l2 * Math.sin(theta2)
        const y2 = y1 + l2 * Math.cos(theta2)

        ctx.clearRect(0, 0, width, height)
        ctx.beginPath()
        ctx.moveTo(width / 2, height / 2)
        ctx.lineTo(x1 + width / 2, y1 + height / 2)
        ctx.lineTo(x2 + width / 2, y2 + height / 2)
        ctx.stroke()

        // const energy = calculate_energy_in_system()
        // // Write energy to screen
        // ctx.font = "30px Arial"
        // ctx.fillText(`Potential energy: ${energy.potential_energy.toFixed(2)}`, 10, 50)
        // ctx.fillText(`Kinetic energy: ${energy.kinetic_energy.toFixed(2)}`, 10, 100)
        // ctx.fillText(`Total energy: ${energy.total_energy.toFixed(2)}`, 10, 150)
    }

    draw()
}

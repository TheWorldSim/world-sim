import { useEffect, useMemo, useRef, useState } from "preact/hooks"

import { make_model_stepper, ModelStepper, ModelStepResult } from "./make_model_stepper"
import { GetItemsReturn } from "./data_curator/src/state/sync/supabase/get_items"
import { get_wcomponents_values_by_id, SimplifiedWComponentsValueById } from "./data/get_wcomponents_values_by_id"
import { get_supabase } from "./data_curator/src/supabase/get_supabase"
import { supabase_get_wcomponents } from "./data_curator/src/state/sync/supabase/wcomponent"
import { WComponentsById } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"


const TARGET_REFRESH_RATE = 30 // Hz
const supabase = get_supabase()


const IDS = {
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


const cached_data: GetItemsReturn<SimplifiedWComponentsValueById> = {
    value: {
        statev2: {
            [IDS.variable__g]: {
                state: 9.81
            },

            [IDS.variable__pendulum_1_mass]: {
                state: 1
            },
            [IDS.variable__pendulum_2_mass]: {
                state: 1
            },
            [IDS.variable__pendulum_1_length]: {
                state: 1.5
            },
            [IDS.variable__pendulum_2_length]: {
                state: 1
            },
            [IDS.stock__pendulum_1_angle]: {
                state: 1.5
            },
            [IDS.stock__pendulum_2_angle]: {
                state: 1.5
            },

            [IDS.variable__pendulum_mass_ratio]: {
                state: 0.5,
                calculation: "[b30dda15-ef05-4c42-a392-7d515552d63b] / [297916db-1365-48ad-a8e1-44809a63a599]",
            },

            [IDS.stock__pendulum_1_angular_velocity]: {
                state: 0
            },
            [IDS.variable__pendulum_1_angular_acceleration]: {
                state: "",
                calculation: "term1 <- -(([02d4a5f4-4abd-41a2-bb50-e68e358a3169]/[e34796cf-ab9e-47b8-9ea5-20cb00fb611d])*[86635a8b-f4f8-4489-8a02-b4f09c0a22ec])\nterm2 <- ((([02d4a5f4-4abd-41a2-bb50-e68e358a3169]*[56fcc4f1-29a9-4a97-b629-068b532ec19d])/[e34796cf-ab9e-47b8-9ea5-20cb00fb611d])*[892551e3-bb5b-4957-8104-6f49e578350a])\nterm1 + term2",
            },
            [IDS.stock__pendulum_2_angular_velocity]: {
                state: 0,
            },
            [IDS.variable__pendulum_2_angular_acceleration]: {
                state: "",
                calculation: "term1 <- ([02d4a5f4-4abd-41a2-bb50-e68e358a3169]/[e34796cf-ab9e-47b8-9ea5-20cb00fb611d])*[86635a8b-f4f8-4489-8a02-b4f09c0a22ec]\nterm2a <- [02d4a5f4-4abd-41a2-bb50-e68e358a3169]/[b1448ce5-3015-4b78-ad34-24cbb0b85040]\nterm2b <- ([02d4a5f4-4abd-41a2-bb50-e68e358a3169] *[56fcc4f1-29a9-4a97-b629-068b532ec19d])/[b1448ce5-3015-4b78-ad34-24cbb0b85040]\nterm2c <- ([02d4a5f4-4abd-41a2-bb50-e68e358a3169]*[56fcc4f1-29a9-4a97-b629-068b532ec19d])/[e34796cf-ab9e-47b8-9ea5-20cb00fb611d]\nterm2 <- (term2a + term2b + term2c)*[892551e3-bb5b-4957-8104-6f49e578350a]\nterm1 - term2",
            },
        },
        causal_link: {
            [IDS.flow__change_in_pendulum_2_angle]: {
                effect: "[8455647b-e016-476a-a6ae-a049deb8bdbb]"
            },
            [IDS.flow__change_in_pendulum_2_angular_velocity]: {
                effect: "[83f8e273-0702-4bc1-8a84-83bc3c285e8d]"
            },
            [IDS.flow__change_in_pendulum_1_angular_velocity]: {
                effect: "[b264f09c-9a68-489d-b545-44176d1c866b]"
            },
            [IDS.flow__change_in_pendulum_1_angle]: {
                effect: "[28c5c78a-3281-449c-b47b-6f094c15d337]"
            }
        },
        action: {}
    },
    error: undefined,
}


export function DemoAppDoublePendulum () {
    // const [data, set_data] = useState<GetItemsReturn<SimplifiedWComponentsValueById> | undefined>(undefined)
    const [data, set_data] = useState<GetItemsReturn<SimplifiedWComponentsValueById> | undefined>(cached_data)


    useEffect(() => {
        if (data) return

        const fetch_data = async () => {
            const ids = Object.values(IDS)
            const wcomponents_response = await supabase_get_wcomponents({ supabase, base_id: undefined, all_bases: true, ids })

            const wcomponents_by_id: WComponentsById = {}
            wcomponents_response.value.forEach(wcomponent => wcomponents_by_id[wcomponent.id] = wcomponent)

            const wcomponents_values_by_id = get_wcomponents_values_by_id(wcomponents_by_id)

            const wcomponents_by_id_response: GetItemsReturn<SimplifiedWComponentsValueById> = {
                value: wcomponents_values_by_id,
                error: wcomponents_response.error,
            }
            console.log("wcomponents_by_id_response", JSON.stringify(wcomponents_by_id_response, null, 4))
            set_data(wcomponents_by_id_response)
        }

        fetch_data()
    }, [data])


    const model_stepper: ModelStepper | undefined = useMemo(() =>
    {
        if (data === undefined || data.error) return undefined


        const variable__g_value = data.value.statev2[IDS.variable__g]!.state
        const variable__pendulum_1_mass_value = data.value.statev2[IDS.variable__pendulum_1_mass]!.state
        const variable__pendulum_2_mass_value = data.value.statev2[IDS.variable__pendulum_2_mass]!.state
        const variable__pendulum_1_length_value = data.value.statev2[IDS.variable__pendulum_1_length]!.state
        const variable__pendulum_2_length_value = data.value.statev2[IDS.variable__pendulum_2_length]!.state
        const stock__pendulum_1_angle_value = data.value.statev2[IDS.stock__pendulum_1_angle]!.state
        const stock__pendulum_2_angle_value = data.value.statev2[IDS.stock__pendulum_2_angle]!.state
        const variable__pendulum_mass_ratio_value = data.value.statev2[IDS.variable__pendulum_mass_ratio]!.calculation!
        const stock__pendulum_1_angular_velocity_value = data.value.statev2[IDS.stock__pendulum_1_angular_velocity]!.state
        const stock__pendulum_2_angular_velocity_value = data.value.statev2[IDS.stock__pendulum_2_angular_velocity]!.state
        const variable__pendulum_1_angular_acceleration_value = data.value.statev2[IDS.variable__pendulum_1_angular_acceleration]!.calculation!
        const variable__pendulum_2_angular_acceleration_value = data.value.statev2[IDS.variable__pendulum_2_angular_acceleration]!.calculation!

        const flow__change_in_pendulum_1_angle_value = data.value.causal_link[IDS.flow__change_in_pendulum_1_angle]!.effect
        const flow__change_in_pendulum_1_angular_velocity_value = data.value.causal_link[IDS.flow__change_in_pendulum_1_angular_velocity]!.effect
        const flow__change_in_pendulum_2_angle_value = data.value.causal_link[IDS.flow__change_in_pendulum_2_angle]!.effect
        const flow__change_in_pendulum_2_angular_velocity_value = data.value.causal_link[IDS.flow__change_in_pendulum_2_angular_velocity]!.effect


        const wrapped_model = make_model_stepper(
            { target_refresh_rate: TARGET_REFRESH_RATE },
            { algorithm: "RK4" },
        )

        const variable__g = wrapped_model.add_variable({
            wcomponent_id: IDS.variable__g,
            name: IDS.variable__g,
            value: variable__g_value,
        })

        const variable__pendulum_1_mass = wrapped_model.add_variable({
            wcomponent_id: IDS.variable__pendulum_1_mass,
            name: IDS.variable__pendulum_1_mass,
            value: variable__pendulum_1_mass_value,
        })
        const variable__pendulum_2_mass = wrapped_model.add_variable({
            wcomponent_id: IDS.variable__pendulum_2_mass,
            name: IDS.variable__pendulum_2_mass,
            value: variable__pendulum_2_mass_value,
        })
        const variable__pendulum_1_length = wrapped_model.add_variable({
            wcomponent_id: IDS.variable__pendulum_1_length,
            name: IDS.variable__pendulum_1_length,
            value: variable__pendulum_1_length_value,
        })
        const variable__pendulum_2_length = wrapped_model.add_variable({
            wcomponent_id: IDS.variable__pendulum_2_length,
            name: IDS.variable__pendulum_2_length,
            value: variable__pendulum_2_length_value,
        })

        const stock__pendulum_1_angle = wrapped_model.add_stock({
            wcomponent_id: IDS.stock__pendulum_1_angle,
            name: IDS.stock__pendulum_1_angle,
            initial: stock__pendulum_1_angle_value,
        })
        const stock__pendulum_2_angle = wrapped_model.add_stock({
            wcomponent_id: IDS.stock__pendulum_2_angle,
            name: IDS.stock__pendulum_2_angle,
            initial: stock__pendulum_2_angle_value,
        })

        const variable__pendulum_mass_ratio = wrapped_model.add_variable({
            wcomponent_id: IDS.variable__pendulum_mass_ratio,
            name: IDS.variable__pendulum_mass_ratio,
            value: variable__pendulum_mass_ratio_value,
            linked_ids: [IDS.variable__pendulum_1_mass, IDS.variable__pendulum_2_mass],
        })

        const stock__pendulum_1_angular_velocity = wrapped_model.add_stock({
            wcomponent_id: IDS.stock__pendulum_1_angular_velocity,
            name: IDS.stock__pendulum_1_angular_velocity,
            initial: stock__pendulum_1_angular_velocity_value,
        })
        const stock__pendulum_2_angular_velocity = wrapped_model.add_stock({
            wcomponent_id: IDS.stock__pendulum_2_angular_velocity,
            name: IDS.stock__pendulum_2_angular_velocity,
            initial: stock__pendulum_2_angular_velocity_value,
        })
        const variable__pendulum_1_angular_acceleration = wrapped_model.add_variable({
            wcomponent_id: IDS.variable__pendulum_1_angular_acceleration,
            name: IDS.variable__pendulum_1_angular_acceleration,
            value: variable__pendulum_1_angular_acceleration_value,
            linked_ids: [
                IDS.variable__g,
                IDS.variable__pendulum_1_length,
                IDS.stock__pendulum_1_angle,
                IDS.variable__pendulum_mass_ratio,
                IDS.stock__pendulum_2_angle,
            ],
        })
        const variable__pendulum_2_angular_acceleration = wrapped_model.add_variable({
            wcomponent_id: IDS.variable__pendulum_2_angular_acceleration,
            name: IDS.variable__pendulum_2_angular_acceleration,
            value: variable__pendulum_2_angular_acceleration_value,
            linked_ids: [
                IDS.variable__g,
                IDS.variable__pendulum_1_length,
                IDS.stock__pendulum_1_angle,
                IDS.variable__pendulum_mass_ratio,
                IDS.stock__pendulum_2_angle,
                IDS.variable__pendulum_2_length,
            ],
        })

        const flow__change_in_pendulum_1_angle = wrapped_model.add_flow({
            wcomponent_id: IDS.flow__change_in_pendulum_1_angle,
            name: IDS.flow__change_in_pendulum_1_angle,
            flow_rate: flow__change_in_pendulum_1_angle_value,
            only_positive: false,
            from_id: undefined,
            to_id: IDS.stock__pendulum_1_angle,
            linked_ids: [IDS.stock__pendulum_1_angular_velocity],
        })
        const flow__change_in_pendulum_1_angular_velocity = wrapped_model.add_flow({
            wcomponent_id: IDS.flow__change_in_pendulum_1_angular_velocity,
            name: IDS.flow__change_in_pendulum_1_angular_velocity,
            flow_rate: flow__change_in_pendulum_1_angular_velocity_value,
            only_positive: false,
            from_id: undefined,
            to_id: IDS.stock__pendulum_1_angular_velocity,
            linked_ids: [IDS.variable__pendulum_1_angular_acceleration],
        })
        const flow__change_in_pendulum_2_angle = wrapped_model.add_flow({
            wcomponent_id: IDS.flow__change_in_pendulum_2_angle,
            name: IDS.flow__change_in_pendulum_2_angle,
            flow_rate: flow__change_in_pendulum_2_angle_value,
            only_positive: false,
            from_id: undefined,
            to_id: IDS.stock__pendulum_2_angle,
            linked_ids: [IDS.stock__pendulum_2_angular_velocity],
        })
        const flow__change_in_pendulum_2_angular_velocity = wrapped_model.add_flow({
            wcomponent_id: IDS.flow__change_in_pendulum_2_angular_velocity,
            name: IDS.flow__change_in_pendulum_2_angular_velocity,
            flow_rate: flow__change_in_pendulum_2_angular_velocity_value,
            only_positive: false,
            from_id: undefined,
            to_id: IDS.stock__pendulum_2_angular_velocity,
            linked_ids: [IDS.variable__pendulum_2_angular_acceleration],
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
    const [pendulum_1_angle, set_pendulum_1_angle] = useState(model_stepper.get_latest_state_by_id(IDS.stock__pendulum_1_angle))
    const [pendulum_2_angle, set_pendulum_2_angle] = useState(model_stepper.get_latest_state_by_id(IDS.stock__pendulum_2_angle))
    const pendulum_1_length = model_stepper.get_latest_state_by_id(IDS.variable__pendulum_1_length)
    const pendulum_2_length = model_stepper.get_latest_state_by_id(IDS.variable__pendulum_2_length)
    const pendulum_1_mass = model_stepper.get_latest_state_by_id(IDS.variable__pendulum_1_mass)
    const pendulum_2_mass = model_stepper.get_latest_state_by_id(IDS.variable__pendulum_2_mass)

    // const [stock_b, set_stock_b] = useState(model_stepper.get_latest_state_by_id(IDS_v4.stock__state_b))
    const past_actions_taken = useRef<{step: number, actions_taken: {[action_id: string]: number}}[]>([])
    const actions_taken = useRef<{[action_id: string]: number}>({})

    useEffect(() => model_stepper.run_simulation((result: ModelStepResult) =>
        {
            set_current_time(result.current_time)
            set_pendulum_1_angle(result.values[IDS.stock__pendulum_1_angle])
            set_pendulum_2_angle(result.values[IDS.stock__pendulum_2_angle])

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


    // const action__increase_stock_a = useMemo(model_stepper.make_apply_action(
    //     actions_taken,
    //     IDS_v4.action__action_increase_stock_a,
    // ), [])

    // const action__move_a_to_b = useMemo(model_stepper.make_apply_action(
    //     actions_taken,
    //     IDS_v4.action__action_move_a_to_b,
    // ), [])

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
    const g = 9.81
    const l1 = props.pendulum_1_length * 70
    const l2 = props.pendulum_2_length * 70
    let theta1 = props.pendulum_1_angle // * (Math.PI / 180)
    let theta2 = props.pendulum_2_angle // * (Math.PI / 180)

    // function calculate_energy_in_system (theta1: number, theta2: number, omega1: number, omega2: number)
    // {
    //     const potential_energy = -m1 * g * l1 * Math.cos(theta1) - m2 * g * (l1 * Math.cos(theta1) + l2 * Math.cos(theta2))
    //     const kinetic_energy = 0.5 * m1 * l1 * l1 * omega1 * omega1 + 0.5 * m2 * (l1 * l1 * omega1 * omega1 + l2 * l2 * omega2 * omega2 + 2 * l1 * l2 * omega1 * omega2 * Math.cos(theta1 - theta2))
    //     return {
    //         potential_energy,
    //         kinetic_energy,
    //         total_energy: potential_energy + kinetic_energy,
    //     }
    // }

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
    }

    draw()

    // function animate ()
    // {
    //     draw()

    //     const energy = calculate_energy_in_system(theta1, theta2, omega1, omega2)
    //     // Write energy to screen
    //     ctx.font = "30px Arial"
    //     ctx.fillText(`Potential energy: ${energy.potential_energy.toFixed(2)}`, 10, 50)
    //     ctx.fillText(`Kinetic energy: ${energy.kinetic_energy.toFixed(2)}`, 10, 100)
    //     ctx.fillText(`Total energy: ${energy.total_energy.toFixed(2)}`, 10, 150)

    //     requestAnimationFrame(animate)
    // }

}
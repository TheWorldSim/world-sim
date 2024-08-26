import { useEffect, useRef, useState } from "preact/hooks"

import "./app.css"
import { make_model_stepper1 } from "./make_model_stepper1"
import { make_model_stepper2 } from "./make_model_stepper2"
import { get_data } from "./data/get_data"
import { WComponentNode } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"
// import { get_state_components_by_id } from "./data/get_state_components_by_id"
// import { perform_calculations } from "./data_curator/src/calculations/perform_calculations"
// import { get_action_component_by_id } from "./data/get_action_component_by_id"


const input: WComponentNode[] = get_data()


const state_component__stock_a_id = "17edbf36-ad5b-4936-b3c5-7d803741c678"
const state_component__stock_b_id = "b644e33f-c00f-4a50-acc4-f158e4e11be5"

const action_component__increase_stock_a_id = "8ecb8d21-4803-4028-9341-b7cd59b56cda"
const action_component__move_a_to_b_id = "2dc650ae-8458-47b6-be0e-6ad1cab3cd4d"

// const initial_stock_a_str = (state_component__stock_a?.values_and_prediction_sets || [])[0]?.entries[0]?.value || undefined
// const initial_stock_b_str = (state_component__stock_b?.values_and_prediction_sets || [])[0]?.entries[0]?.value || undefined

// const initial_stock_a = initial_stock_a_str ? parseInt(initial_stock_a_str) : undefined
// const initial_stock_b = initial_stock_b_str ? parseInt(initial_stock_b_str) : undefined
// const action_calculations__increase_stock_a = action_component__increase_stock_a?.calculations || []
// const action_calculations__move_a_to_b = action_component__move_a_to_b?.calculations || []

// const initial_state_by_id: {[component_id: string]: number} = {}

const TARGET_REFRESH_RATE = 2 // Hz
const model_stepper = make_model_stepper1(input, TARGET_REFRESH_RATE)
// const model_stepper = make_model_stepper2(input)

export function DemoAppAddOneToStock () {
  // const created_at_date = "2024-05-28"
  // const created_at_time = "11:22:59"
  // http://localhost:5173/app/#wcomponents/17edbf36-ad5b-4936-b3c5-7d803741c678/&storage_location=1&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1218&y=-1538&z=0&zoom=68&sdate=2024-03-24&stime=22:42:19&cdate=2024-05-24&ctime=11:22:59

  const [stock_a, set_stock_a] = useState(model_stepper.state_by_id(state_component__stock_a_id))
  const [stock_b, set_stock_b] = useState(model_stepper.state_by_id(state_component__stock_b_id))

  const start_time_ms = useRef(new Date().getTime())
  const current_simulation_step = useRef(0)

  useEffect(() => {
    let animationFrameId: number

    const animate = () => {
      const time_since_start_ms = new Date().getTime() - start_time_ms.current
      const simulation_step = Math.floor(time_since_start_ms / (1000 / TARGET_REFRESH_RATE))
      if (simulation_step > current_simulation_step.current)
      {
        const simulation_step_completed = () =>
        {
          // Restart scheduling the next frame
          animationFrameId = requestAnimationFrame(animate)
        }

        model_stepper.simulate_step(simulation_step_completed)
        current_simulation_step.current = simulation_step
      }
      else
      {
        // Schedule the next frame
        animationFrameId = requestAnimationFrame(animate)
      }
    }

    // Start the animation
    animationFrameId = requestAnimationFrame(animate)

    // Cleanup function to cancel the animation frame
    return () => {
      cancelAnimationFrame(animationFrameId)
    }
  }, [])


  useEffect(() => {
    return model_stepper.on_state_change((state) => {
      const new_stock_a = state[state_component__stock_a_id]
      const new_stock_b = state[state_component__stock_b_id]

      if (new_stock_a === undefined || new_stock_b === undefined)
      {
        throw new Error(`New Stock A or B is undefined`)
      }

      set_stock_a(new_stock_a)
      set_stock_b(new_stock_b)
    })
  }, [])

  function action__increase_stock_a ()
  {
    model_stepper.apply_action(action_component__increase_stock_a_id)
  }

  function action__move_a_to_b ()
  {
    model_stepper.apply_action(action_component__move_a_to_b_id)
  }

  return (
    <>
      <div class="card">
        <div>Stock A is {stock_a}</div>
        <div>Stock B is {stock_b}</div>

        <button onClick={() => action__increase_stock_a()}>
          Increase stock A
        </button>

        <button onClick={() => action__move_a_to_b()}>
          Move A to B
        </button>
      </div>
    </>
  )
}

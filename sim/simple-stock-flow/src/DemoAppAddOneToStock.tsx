import { useEffect, useRef, useState } from "preact/hooks"

import "./app.css"
import { make_model_stepper, ModelStepResult } from "./make_model_stepper3"
import { IDS } from "./data/get_data"
// import { SimulationResult2 } from "./simulation2/simulation2"
// import { WComponentNode } from "./data_curator/src/wcomponent/interfaces/SpecialisedObjects"


const TARGET_REFRESH_RATE = 1 // Hz
const model_stepper = make_model_stepper(TARGET_REFRESH_RATE)

export function DemoAppAddOneToStock () {
  // const created_at_date = "2024-05-28"
  // const created_at_time = "11:22:59"
  // http://localhost:5173/app/#wcomponents/17edbf36-ad5b-4936-b3c5-7d803741c678/&storage_location=1&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1218&y=-1538&z=0&zoom=68&sdate=2024-03-24&stime=22:42:19&cdate=2024-05-24&ctime=11:22:59

  const [current_time, set_current_time] = useState(model_stepper.get_current_time())
  const [stock_a, set_stock_a] = useState(model_stepper.get_latest_state_by_id(IDS.state_component__stock_a_id))
  const [stock_b, set_stock_b] = useState(model_stepper.get_latest_state_by_id(IDS.state_component__stock_b_id))

//   const start_time_ms = useRef(new Date().getTime())
//   const current_simulation_step = useRef(0)

  useEffect(model_stepper.run_simulation({
    target_refresh_rate: TARGET_REFRESH_RATE,
    on_simulation_step_completed: (result: ModelStepResult) =>
    {
      set_current_time(result.current_time)
      set_stock_a(result.values[IDS.state_component__stock_a_id])
      set_stock_b(result.values[IDS.state_component__stock_b_id])

    //   current_simulation_step.current += 1
    //   console.log(`Simulation step ${current_simulation_step.current} took ${new Date().getTime() - start_time_ms.current} ms`)
    //   start_time_ms.current = new Date().getTime()
    }
  }), [])


  function action__increase_stock_a ()
  {
    model_stepper.apply_action(IDS.action_component__increase_stock_a_id)
  }

  function action__move_a_to_b ()
  {
    model_stepper.apply_action(IDS.action_component__move_a_to_b_id)
  }

  return (
    <>
      <div class="card">
        <div>Current time is {current_time}</div>
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

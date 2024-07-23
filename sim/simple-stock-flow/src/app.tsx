import { useState } from 'preact/hooks'
import './app.css'
// import { get_state_components_by_id } from './data/get_state_components_by_id'
// import { perform_calculations } from './data_curator/src/calculations/perform_calculations'
// import { get_action_component_by_id } from './data/get_action_component_by_id'


// const state_components_by_id = get_state_components_by_id()

const state_component__stock_a_id = "17edbf36-ad5b-4936-b3c5-7d803741c678"
const state_component__stock_b_id = "b644e33f-c00f-4a50-acc4-f158e4e11be5"
// const state_component__stock_a = state_components_by_id["17edbf36-ad5b-4936-b3c5-7d803741c678"]
// const state_component__stock_b = state_components_by_id["b644e33f-c00f-4a50-acc4-f158e4e11be5"]
// const action_component__increase_stock_a = get_action_component_by_id("8ecb8d21-4803-4028-9341-b7cd59b56cda")
// const action_component__move_a_to_b = get_action_component_by_id("2dc650ae-8458-47b6-be0e-6ad1cab3cd4d")

// const initial_stock_a_str = (state_component__stock_a?.values_and_prediction_sets || [])[0]?.entries[0]?.value || undefined
// const initial_stock_b_str = (state_component__stock_b?.values_and_prediction_sets || [])[0]?.entries[0]?.value || undefined

// const initial_stock_a = initial_stock_a_str ? parseInt(initial_stock_a_str) : undefined
// const initial_stock_b = initial_stock_b_str ? parseInt(initial_stock_b_str) : undefined
// const action_calculations__increase_stock_a = action_component__increase_stock_a?.calculations || []
// const action_calculations__move_a_to_b = action_component__move_a_to_b?.calculations || []

const initial_state_by_id: {[component_id: string]: number} = {}

export function App () {
  // const created_at_date = "2024-05-28"
  // const created_at_time = "11:22:59"
  // http://localhost:5173/app/#wcomponents/17edbf36-ad5b-4936-b3c5-7d803741c678/&storage_location=1&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1218&y=-1538&z=0&zoom=68&sdate=2024-03-24&stime=22:42:19&cdate=2024-05-24&ctime=11:22:59

  const [stock_a, set_stock_a] = useState(initial_state_by_id[state_component__stock_a_id])
  const [stock_b, set_stock_b] = useState(initial_state_by_id[state_component__stock_b_id])

  function action__increase_stock_a ()
  {
    // perform_calculations([action_calculations__increase_stock_a])
  }

  function action__move_a_to_b ()
  {

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

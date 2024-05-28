import { useState } from 'preact/hooks'
import preactLogo from './assets/preact.svg'
import viteLogo from '/vite.svg'
import './app.css'
import { get_state_component_by_id } from './data/get_state_component_by_id'


export function App () {
  // const created_at_date = "2024-05-28"
  // const created_at_time = "11:22:59"
  // http://localhost:5173/app/#wcomponents/17edbf36-ad5b-4936-b3c5-7d803741c678/&storage_location=1&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1218&y=-1538&z=0&zoom=68&sdate=2024-03-24&stime=22:42:19&cdate=2024-05-24&ctime=11:22:59

  const stock_a_component = get_state_component_by_id("17edbf36-ad5b-4936-b3c5-7d803741c678")
  const stock_b_component = get_state_component_by_id("b644e33f-c00f-4a50-acc4-f158e4e11be5")
  const action_increase_a_component = get_state_component_by_id("8ecb8d21-4803-4028-9341-b7cd59b56cda")
  const action_move_a_to_b_component = get_state_component_by_id("2dc650ae-8458-47b6-be0e-6ad1cab3cd4d")

  const initial_stock_a_str = (stock_a_component?.values_and_prediction_sets || [])[0]?.entries[0]?.value || undefined
  const initial_stock_b_str = (stock_b_component?.values_and_prediction_sets || [])[0]?.entries[0]?.value || undefined

  const initial_stock_a = initial_stock_a_str ? parseInt(initial_stock_a_str) : undefined
  const initial_stock_b = initial_stock_b_str ? parseInt(initial_stock_b_str) : undefined

  const [stock_a, set_stock_a] = useState(initial_stock_a)
  const [stock_b, set_stock_b] = useState(initial_stock_b)

  return (
    <>
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} class="logo" alt="Vite logo" />
        </a>
        <a href="https://preactjs.com" target="_blank">
          <img src={preactLogo} class="logo preact" alt="Preact logo" />
        </a>
      </div>
      <h1>Vite + Preact</h1>
      <div class="card">
        <div>Stock A is {stock_a}</div>
        <div>Stock B is {stock_b}</div>

        <button onClick={() => {}}>
          Increase stock A
        </button>

        <button onClick={() => {}}>
          Move A to B
        </button>

        <p>
          Edit <code>src/app.tsx</code> and save to test HMR
        </p>
      </div>
      <p class="read-the-docs">
        Click on the Vite and Preact logos to learn more
      </p>
    </>
  )
}

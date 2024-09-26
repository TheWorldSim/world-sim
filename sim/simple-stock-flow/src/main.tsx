import { h, render } from "preact"
import "./index.css"

import "./monkey_patch"

import { DemoAppAddOneToStockV3 } from "./DemoAppAddOneToStockV3.tsx"
import { DemoAppAddOneToStockV4 } from "./DemoAppAddOneToStockV4.tsx"
import { useState } from "preact/hooks"
import { DemoAppDoublePendulum } from "./DemoAppDoublePendulum.tsx"
import "./app.css"


const Apps = {
    add_one_to_stock_v3: "Add One To Stock V3",
    add_one_to_stock_v4: "Add One To Stock V4",
    double_pendulum: "Double Pendulum",
}

function App ()
{
    const [selected_app_id, set_selected_app_id] = useState<undefined | keyof typeof Apps>(undefined)

    if (selected_app_id === undefined)
    {
        return <div>
            {Object.entries(Apps).map(([app_id, app_name]) => <div key={app_id}>
                <button onClick={() => set_selected_app_id(app_id as keyof typeof Apps)}>{app_name}</button>
            </div>)}
        </div>
    }

    const app_name = Apps[selected_app_id]
    let app_jsx: h.JSX.Element = <DemoAppAddOneToStockV3 />
    if (selected_app_id === "add_one_to_stock_v4")
    {
        app_jsx = <DemoAppAddOneToStockV4 />
    }
    else if (selected_app_id === "double_pendulum")
    {
        app_jsx = <DemoAppDoublePendulum />
    }

    return <div>
        <button onClick={() => set_selected_app_id(undefined)}>Back</button>
        <h3>{app_name}</h3>
        {app_jsx}
        <div id="debug_output"></div>
    </div>
}

render(<App />, document.getElementById("app")!)

// import { DemoAppDoublePendulum } from "./DemoAppDoublePendulum.tsx"
// render(<DemoAppDoublePendulum />, document.getElementById("app")!)

// import { DemoOscillator } from "./simulation2/demos/demo_oscillator.tsx"
// render(<DemoOscillator />, document.getElementById("app")!)
// import { DemoSimpleMotionWithNumericalIntegrals } from "./DemoSimpleMotionWithNumericalIntegrals.tsx"
// render(<DemoSimpleMotionWithNumericalIntegrals />, document.getElementById("app")!)

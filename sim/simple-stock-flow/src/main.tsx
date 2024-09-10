import { h, render } from "preact"
import "./index.css"

import "./monkey_patch"

import { DemoAppAddOneToStockV3 } from "./DemoAppAddOneToStockV3.tsx"
import { useState } from "preact/hooks"

function App ()
{
    const [app_id, set_app_id] = useState<undefined | 3>(undefined)

    if (app_id === undefined)
    {
        return <div>
            <button onClick={() => set_app_id(3)}>DemoAppAddOneToStockV3</button>
        </div>
    }

    let app: h.JSX.Element = <DemoAppAddOneToStockV3 />

    return <div>
        <button onClick={() => set_app_id(undefined)}>Back</button>
        {app}
    </div>
}

render(<App />, document.getElementById("app")!)

// import { DemoAppDoublePendulum } from "./DemoAppDoublePendulum.tsx"
// render(<DemoAppDoublePendulum />, document.getElementById("app")!)

// import { DemoOscillator } from "./simulation2/demos/demo_oscillator.tsx"
// render(<DemoOscillator />, document.getElementById("app")!)
// import { DemoSimpleMotionWithNumericalIntegrals } from "./DemoSimpleMotionWithNumericalIntegrals.tsx"
// render(<DemoSimpleMotionWithNumericalIntegrals />, document.getElementById("app")!)

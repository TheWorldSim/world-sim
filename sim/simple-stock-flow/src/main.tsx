import { render } from "preact"
import "./index.css"

import { DemoAppAddOneToStock } from "./DemoAppAddOneToStock.tsx"
render(<DemoAppAddOneToStock />, document.getElementById("app")!)

// import { DemoAppDoublePendulum } from "./DemoAppDoublePendulum.tsx"
// render(<DemoAppDoublePendulum />, document.getElementById("app")!)

// import { DemoOscillator } from "./simulation2/demos/demo_oscillator.tsx"
// render(<DemoOscillator />, document.getElementById("app")!)
// import { DemoSimpleMotionWithNumericalIntegrals } from "./DemoSimpleMotionWithNumericalIntegrals.tsx"
// render(<DemoSimpleMotionWithNumericalIntegrals />, document.getElementById("app")!)

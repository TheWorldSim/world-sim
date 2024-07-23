import { Model, SimulationError } from "simulation"
import { CalculationResult } from "./interfaces"



export function run_model (model: Model, initial_units: string | undefined, model_component: SimulationComponent, retrying_with_units = false): CalculationResult
{
    let value: number | undefined = undefined
    let error = ""
    let units = model_component._node.getAttribute("Units")

    try {
        const calculation_result = model.simulate()
        value = calculation_result!._data.data[0]![model_component!._node.id]
    }
    catch (e) {
        const err = e as SimulationError
        const units_error = (typeof err.message === "string") && err.message.startsWith("Wrong units generated for") && err.message.match(/and got (.+?)\.(?:$| Either)/)
        // If no units were initially specified and the error is about wrong
        // units then try to recompute using the units the model expects
        if (!initial_units && units_error && !retrying_with_units)
        {
            units = units_error[1]!
            model_component.units = units
            const second_attempt = run_model(model, initial_units, model_component, true)
            ;({ value, units } = second_attempt)
            if (second_attempt.error) error = second_attempt.error
        }
        else
        {
            // Defensive approach to ensure there's always some content in the
            // error in case err.message is ever undefined or an empty string
            error = `${err.message || "Unknown calculation error"}`
        }
    }

    const calculation_result: CalculationResult = { value, error, units }
    if (!calculation_result.error) delete calculation_result.error
    if (calculation_result.units === "Unitless") calculation_result.units = ""

    return calculation_result
}

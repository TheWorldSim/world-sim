import { Model, ModelVariableConfig } from "simulation"

import { WComponentsById } from "../wcomponent/interfaces/SpecialisedObjects"
import { CalculationResult, PlainCalculationObject } from "./interfaces"
import { normalise_calculation_ids } from "./normalise_calculation_ids"
import { get_double_at_mentioned_uuids_from_text } from "../sharedf/rich_text/replace_normal_ids"
import { normalise_calculation_numbers } from "./normalise_calculation_numbers"
import { convert_percentages } from "./convert_percentages"
import { hide_currency_symbols } from "./hide_currency_symbols"
import { apply_units_from_component } from "./apply_units_from_component"
import { WComponentsValueById, get_wcomponents_values_by_id } from "../../../data/get_wcomponents_values_by_id"



export function prepare_model (calculations: PlainCalculationObject[], wcomponents_by_id: WComponentsById)
{
    const wcomponents_state_by_id: WComponentsValueById = get_wcomponents_values_by_id(wcomponents_by_id)

    const values: { [id: string]: CalculationResult } = {}

    const model = new Model({
        timeStart: 2020,
        timeLength: 1,
        timeUnits: "Years"
    })

    calculations.forEach(calculation =>
    {
        const uuid_v4s = get_double_at_mentioned_uuids_from_text(calculation.value)
        let converted_calculation = normalise_calculation_numbers(calculation.value)
        converted_calculation = convert_percentages(converted_calculation)
        converted_calculation = hide_currency_symbols(converted_calculation)

        let units: string | undefined = calculation.units
        units = apply_units_from_component(converted_calculation, units, wcomponents_by_id)
        units = hide_currency_symbols(units || "")

        converted_calculation = normalise_calculation_ids(converted_calculation, uuid_v4s)

        const model_config: ModelVariableConfig = {
            name: calculation.name,
            value: converted_calculation,
        }
        if (units !== undefined) model_config.units = units
        const model_component = model.Variable(model_config)

        prepare_other_components(model, model_component, values, uuid_v4s, wcomponents_state_by_id)
    })

    return model
}



function prepare_other_components (model: Model, model_component: SimulationComponent, values: { [id: string]: CalculationResult }, uuids: string[], wcomponents_state_by_id: WComponentsValueById)
{
    const other_components: { [id: string]: SimulationComponent } = {}

    Object.entries(values).forEach(([name, calc_result]) =>
    {
        if (calc_result.value !== undefined) {
            const component = model.Variable({ name, value: calc_result.value, units: calc_result.units })
            other_components[name] = component
        }
    })

    uuids.forEach(uuid =>
    {
        const state = wcomponents_state_by_id[uuid]
        if (state === undefined) return // type guard

        const component = model.Variable({ name: uuid, value: state, units: "" })
        other_components[uuid] = component
    })

    Object.values(other_components).forEach((other_component) =>
    {
        model.Link(other_component, model_component)
    })
}

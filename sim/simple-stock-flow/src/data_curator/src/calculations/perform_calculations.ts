import { Model, ModelVariableConfig } from "simulation"

import { WComponentsById } from "../wcomponent/interfaces/SpecialisedObjects"
import { CalculationResult, PlainCalculationObject } from "./interfaces"
import { normalise_calculation_ids } from "./normalise_calculation_ids"
import { get_wcomponent_state_value_and_probabilities } from "../wcomponent_derived/get_wcomponent_state_value_and_probabilities"
import { get_double_at_mentioned_uuids_from_text } from "../sharedf/rich_text/replace_normal_ids"
import { normalise_calculation_numbers } from "./normalise_calculation_numbers"
import { convert_percentages } from "./convert_percentages"
import { hide_currency_symbols, unhide_currency_symbols } from "./hide_currency_symbols"
import { apply_units_from_component } from "./apply_units_from_component"
import { WComponentsStateById } from "./get_wcomponents_state_by_id"
import { run_model } from "./run_model"



export function perform_calculations (calculations: PlainCalculationObject[], wcomponents_state_by_id: WComponentsStateById, wcomponents_by_id: WComponentsById)
{
    const values: { [id: string]: CalculationResult } = {}

    const calculation_results: CalculationResult[] = calculations.map(calculation =>
    {
        const model = new Model({
            timeStart: 2020,
            timeLength: 1,
            timeUnits: "Years"
        })

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

        const calculation_result = run_model(model, calculation.units, model_component)

        // Store calculation value for use in future calculations
        values[calculation.name] = calculation_result

        return calculation_result
    })
    .map(calculation_result =>
    {
        calculation_result.units = unhide_currency_symbols(calculation_result.units)
        if (calculation_result.error)
        {
            calculation_result.error = unhide_currency_symbols(calculation_result.error)
        }

        return calculation_result
    })

    return calculation_results
}



function prepare_other_components (model: Model, model_component: SimulationComponent, values: { [id: string]: CalculationResult }, uuids: string[], wcomponents_state_by_id: WComponentsStateById)
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
        const wcomponent = wcomponents_by_id[uuid]
        if (!wcomponent) return

        const VAP_sets = get_wcomponent_state_value_and_probabilities({
            wcomponent,
            VAP_set_id_to_counterfactual_v2_map: {},
            created_at_ms: now_ms,
            sim_ms: now_ms,
        }).most_probable_VAP_set_values
        if (VAP_sets.length === 0) return

        const value = VAP_sets[0]!.parsed_value

        if (value === undefined || value === null) return

        // Note that currently the value of boolean's is a string of "True" or "False"
        if (typeof value === "boolean") return

        const component = model.Variable({ name: uuid, value, units: "" })
        other_components[uuid] = component
    })

    Object.values(other_components).forEach((other_component) =>
    {
        model.Link(other_component, model_component)
    })
}

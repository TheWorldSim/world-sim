import { normalise_calculation_ids } from "../data_curator/src/calculations/normalise_calculation_ids"
import { get_double_at_mentioned_uuids_from_text } from "../data_curator/src/sharedf/rich_text/replace_normal_ids"
import { wcomponent_is_action, wcomponent_is_causal_link, WComponentsById } from "../data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { get_wcomponent_state_value_and_probabilities } from "../data_curator/src/wcomponent_derived/get_wcomponent_state_value_and_probabilities"
import { get_calculation_string_from_calculation_rows } from "./get_calculation_string_from_calculation_rows"


interface SimplifiedWComponentStateV2
{
    state: number | string
}
interface SimplifiedWComponentCausalLink
{
    effect: string
}
interface SimplifiedWComponentAction {
    calculation: string
    // trigger_type: "condition"
    // trigger_calculation: string
}

export interface SimplifiedWComponentsValueById
{
    statev2: {[id: string]: SimplifiedWComponentStateV2}
    causal_link: {[id: string]: SimplifiedWComponentCausalLink}
    action: {[id: string]: SimplifiedWComponentAction}
}

export function get_wcomponents_values_by_id (wcomponents_by_id: WComponentsById): SimplifiedWComponentsValueById
{
    const value_by_id: SimplifiedWComponentsValueById = {
        statev2: {},
        causal_link: {},
        action: {},
    }

    const now_ms = new Date().getTime()

    Object.entries(wcomponents_by_id).forEach(([uuid, wcomponent]) =>
    {
        if (wcomponent_is_causal_link(wcomponent))
        {
            let effect = wcomponent.effect_string || ""
            const effect_uuids = get_double_at_mentioned_uuids_from_text(effect)
            effect = normalise_calculation_ids(effect, effect_uuids)

            value_by_id.causal_link[uuid] = { effect }
            return
        }

        if (wcomponent_is_action(wcomponent))
        {
            const calculation = get_calculation_string_from_calculation_rows(wcomponent.calculations)
            // const trigger_calculation = wcomponent.trigger_calculations.join("\n") || ""
            // const trigger_type = "condition" //wcomponent.trigger_type
            value_by_id.action[uuid] = {
                calculation,
                // trigger_type,
                // trigger_calculation,
            }
            return
        }

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

        value_by_id.statev2[uuid] = {
            state: value,
        }
    })

    return value_by_id
}
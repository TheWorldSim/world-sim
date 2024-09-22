import { WComponentsById } from "../data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { get_wcomponent_state_value_and_probabilities } from "../data_curator/src/wcomponent_derived/get_wcomponent_state_value_and_probabilities"


export interface WComponentsValueById
{
    [id: string]: number | string
}

export function get_wcomponents_values_by_id (wcomponents_by_id: WComponentsById): WComponentsValueById
{
    const initial_state_by_id: {[id: string]: number | string} = {}

    const now_ms = new Date().getTime()

    Object.entries(wcomponents_by_id).forEach(([uuid, wcomponent]) =>
    {
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

        initial_state_by_id[uuid] = value
    })

    return initial_state_by_id
}

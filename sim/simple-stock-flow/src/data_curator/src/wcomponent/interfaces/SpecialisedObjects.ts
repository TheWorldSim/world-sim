// Copied from: https://github.com/AJamesPhillips/DataCurator/blob/main/app/frontend/src/wcomponent/interfaces/SpecialisedObjects.ts

import { WComponentNodeAction } from "./actions"
import { HasVAPSetsAndMaybeValuePossibilities, WComponentNodeStateV2, WComponentStateValue } from "./state"
import { WComponentNodeBase, WComponentType } from "./wcomponent_base"


export type WComponent = WComponentNode // | WComponentConnection | WComponentCausalConnection | WComponentJudgement | WComponentPrioritisation


export type WComponentsById = { [id: string]: WComponent /*| undefined*/ }


export interface WComponentNodeProcess extends WComponentNodeBase, WComponentNodeProcessBase
{
    type: "process"
}
interface WComponentNodeProcessBase
{
    // active: ProcessActiveStatus[]
    // end: TemporalUncertainty
}


export type WComponentNode = (// WComponentNodeEvent
    WComponentNodeStateV2
    //| WComponentSubState
    | WComponentStateValue
    | WComponentNodeProcess
    | WComponentNodeAction
    //| WComponentNodeGoal
)

function wcomponent_is_a (type: WComponentType, wcomponent: WComponent | undefined, log_error_id: number | string = "")
{
    let yes = false
    log_error_id = (typeof log_error_id === "string") ? log_error_id : ""

    if (!wcomponent)
    {
        if (log_error_id)
        {
            console.error(`wcomponent with id "${log_error_id}" does not exist`)
        }
    }
    else if (wcomponent.type !== type)
    {
        if (log_error_id)
        {
            console.error(`wcomponent with id "${log_error_id}" is not a ${type}`)
        }
    }
    else yes = true

    return yes
}


export function wcomponent_is_statev2 (wcomponent: WComponent | undefined): wcomponent is WComponentNodeStateV2
{
    return wcomponent_is_a("statev2", wcomponent)
}

export function wcomponent_is_state_value (wcomponent: WComponent | undefined, log_error_id = ""): wcomponent is WComponentStateValue
{
    return wcomponent_is_a("state_value", wcomponent, log_error_id)
}


export function wcomponent_is_action (wcomponent: WComponent | undefined, log_error_id: number | string = ""): wcomponent is WComponentNodeAction
{
    return wcomponent_is_a("action", wcomponent, log_error_id)
}



// Need to keep in sync with wc_ids_by_type.any_state_VAPs
export type WComponentIsAllowedToHaveStateVAPSets = WComponent & HasVAPSetsAndMaybeValuePossibilities
export function wcomponent_is_allowed_to_have_state_VAP_sets (wcomponent: WComponent | undefined): wcomponent is (WComponentIsAllowedToHaveStateVAPSets)
{
    return wcomponent_is_statev2(wcomponent)
        || wcomponent_is_state_value(wcomponent)
        // Removing ability to edit causal link state as:
        // 1. I do not remember editing the VAP sets of a causal link -- if it is used we should put a description
        //    here of the scenario that needs it
        // 2. There is already the "Effect when true/false" data
        // 3. We already know we need to support some kind of equation like InsightMaker / OpenModelica and not a
        //    static value which does not tell the receiving node how to handle and combine multiple values
        //    together
        // || wcomponent_is_causal_link(wcomponent)
        || wcomponent_is_action(wcomponent)
}

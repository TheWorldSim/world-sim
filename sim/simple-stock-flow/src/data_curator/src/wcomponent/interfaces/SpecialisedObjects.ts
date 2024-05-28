// Copied from: https://github.com/AJamesPhillips/DataCurator/blob/main/app/frontend/src/wcomponent/interfaces/SpecialisedObjects.ts

import { WComponentNodeAction } from "./actions"
import { WComponentNodeStateV2 } from "./state"
import { WComponentType } from "./wcomponent_base"


export type WComponent = WComponentNode // | WComponentConnection | WComponentCausalConnection | WComponentJudgement | WComponentPrioritisation


export type WComponentNode = (// WComponentNodeEvent
    WComponentNodeStateV2
    //| WComponentSubState
    //| WComponentStateValue
    //| WComponentNodeProcess
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


export function wcomponent_is_action (wcomponent: WComponent | undefined, log_error_id: number | string = ""): wcomponent is WComponentNodeAction
{
    return wcomponent_is_a("action", wcomponent, log_error_id)
}

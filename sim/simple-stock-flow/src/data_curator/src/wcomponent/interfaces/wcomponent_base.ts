// Copied from: https://github.com/AJamesPhillips/DataCurator/blob/main/app/frontend/src/wcomponent/interfaces/wcomponent_base.ts

import { PlainCalculationObject } from "../../calculations/interfaces"
import { Base } from "../../shared/interfaces/base"
import { ValidityPredictions } from "../../shared/uncertainty/validity"
import { HasVAPSetsAndMaybeValuePossibilities } from "./state"


export type WComponentNodeType = "event" | "statev2" | "state_value" | "sub_state" | "multidimensional_state" | "process" | "action" | "actor" | "counterfactualv2" | "goal" | "judgement" | "objective" | "prioritisation"
export type WComponentConnectionType = "causal_link" | "relation_link"
export type WComponentType = WComponentNodeType | WComponentConnectionType


export interface WComponentBase extends Base
{
    type: WComponentType

    // Explainable
    title: string
    hide_title?: boolean
    hide_state?: boolean
    description: string
}


// TODO: Judgments are also nodes on the canvas so we should rename `WComponentNodeBase` to
// something else.
export interface WComponentNodeBase extends WComponentBase, Partial<ValidityPredictions>, Partial<HasVAPSetsAndMaybeValuePossibilities>
{
    type: WComponentNodeType
    // encompassed_by: string
}


export interface WComponentCalculations
{
    calculations: PlainCalculationObject[]
}

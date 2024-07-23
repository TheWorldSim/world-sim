import { NumberDisplayType } from "../shared/types"
import { WComponentNodeStateV2 } from "../wcomponent/interfaces/state"


/**
 * Version 2 of PlainCalculationObject
 */
export interface PlainCalculationObject
{
    // The value for this can be reused so does not guarantee it will remain
    // unique across time, but at any one time it should be unique.
    id: number
    name: string
    value: string  // strings might be an @@<uuid v4>
    units?: string
    result_sig_figs?: number
    result_display_type?: NumberDisplayType
    result_description?: string
}

export interface CalculationResult
{
    value: number | undefined
    units: string
    error?: string
}


export interface WComponentNodeStateV2ById {
    [id: string]: WComponentNodeStateV2
}

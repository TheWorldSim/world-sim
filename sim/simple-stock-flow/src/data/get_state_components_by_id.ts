import { WComponentNodeStateV2ById } from "../data_curator/src/calculations/interfaces"
import { wcomponent_is_statev2 } from "../data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { WComponentNodeStateV2 } from "../data_curator/src/wcomponent/interfaces/state"

import { get_data } from "./get_data"


function get_state_components (): WComponentNodeStateV2[]
{
    const data = get_data()
    const states = data.filter(wcomponent_is_statev2)
    return states
}


export function get_state_components_by_id (): WComponentNodeStateV2ById
{
    const states_by_id = get_state_components().reduce((acc, state) => ({ ...acc, [state.id]: state }), {} as {[id: string]: WComponentNodeStateV2})
    return states_by_id
}

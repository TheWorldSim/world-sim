import { wcomponent_is_statev2 } from "./data_curator/wcomponent/interfaces/SpecialisedObjects"
import { WComponentNodeStateV2 } from "./data_curator/wcomponent/interfaces/state"

import { get_data } from "./get_data"


function get_state_components (): WComponentNodeStateV2[]
{
    const data = get_data()
    const states = data.filter(wcomponent_is_statev2)
    return states
}


export function get_state_component_by_id (id: string): WComponentNodeStateV2 | undefined
{
    const state = get_state_components().find(state => state.id === id)
    return state
}

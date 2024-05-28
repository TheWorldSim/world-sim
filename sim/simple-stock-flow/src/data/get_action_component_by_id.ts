import { wcomponent_is_action } from "../data_curator/src/wcomponent/interfaces/SpecialisedObjects"
import { WComponentNodeAction } from "../data_curator/src/wcomponent/interfaces/actions"
import { get_data } from "./get_data"


function get_action_components (): WComponentNodeAction[]
{
    const data = get_data()
    const actions = data.filter(wcomponent_is_action)
    return actions
}


export function get_action_component_by_id (id: string): WComponentNodeAction | undefined
{
    const action = get_action_components().find(action => action.id === id)
    return action
}

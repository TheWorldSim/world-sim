import { describe, test } from "../data_curator/src/shared/utils/test.ts"
import { WComponentNodeStateV2 } from "../data_curator/src/wcomponent/interfaces/state"
import { WComponentsById } from "../data_curator/src/wcomponent/interfaces/SpecialisedObjects.ts"
import { get_wcomponents_values_by_id } from "./get_wcomponents_values_by_id.ts"


export const test_get_wcomponents_values_by_id = describe.delay("get_wcomponents_values_by_id", () => {
    const uuid1 = "10000000-0000-4000-a000-000000000000"
    const uuid2 = "20000000-0000-4000-a000-000000000000"

    const wc1: WComponentNodeStateV2 = {
        id: uuid1,
        type: "statev2",
        base_id: -1,
        created_at: new Date(),
        subtype: "number",
        title: "State 1",
        description: "",
        calculations: [
            { id: 1, name: `A`, value: "1 + 1" },
            { id: 2, name: `@@${uuid1}`, value: "A + 2" },
        ]
    }

    const wc2: WComponentNodeStateV2 = {
        id: uuid2,
        type: "statev2",
        base_id: -1,
        created_at: new Date(),
        subtype: "number",
        title: "State 1",
        description: "",
        calculations: [
            { id: 1, name: `A`, value: "1 + 1" },
        ]
    }

    const wcomponents_by_id: WComponentsById = {
        [uuid1]: wc1,
        [uuid2]: wc2,
    }
    let result = get_wcomponents_values_by_id(wcomponents_by_id)
    test("A <- 1 + 1\nA + 2", result.statev2[uuid1]?.calculation, "Should remove the assignment when the last calculation assigns to the component id")
    test("A <- 1 + 1", result.statev2[uuid2]?.calculation, "Should leave the assignment when the last calculation is assigning to something other than the component id")
})

import { describe, test } from "../data_curator/src/shared/utils/test.ts"
import { WComponentNodeStateV2 } from "../data_curator/src/wcomponent/interfaces/state"
import { WComponentsById } from "../data_curator/src/wcomponent/interfaces/SpecialisedObjects.ts"
import { get_wcomponents_values_by_id } from "./get_wcomponents_values_by_id.ts"
import { uuid_v4_for_tests } from "../data_curator/src/utils/uuid_v4_for_tests.ts"
import { WComponentNodeAction } from "../data_curator/src/wcomponent/interfaces/action.ts"


export const test_get_wcomponents_values_by_id = describe.delay("get_wcomponents_values_by_id", () => {
    const uuid1 = uuid_v4_for_tests(1)
    const uuid2 = uuid_v4_for_tests(2)
    const uuid3 = uuid_v4_for_tests(3)

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

    const wc3: WComponentNodeAction = {
        id: uuid3,
        type: "action",
        base_id: -1,
        created_at: new Date(),
        title: "An action",
        description: "",
        calculations: [
            { id: 1, name: uuid2, value: `@@${uuid2} + @@${uuid3}` },
            { id: 2, name: uuid1, value: `@@${uuid2} - @@${uuid3}` },
        ],
        // These could be removed
        depends_on_action_ids: [],
        reason_for_status: "",
    }

    const wcomponents_by_id: WComponentsById = {
        [wc1.id]: wc1,
        [wc2.id]: wc2,
        [wc3.id]: wc3,
    }
    let result = get_wcomponents_values_by_id(wcomponents_by_id)
    test(result.statev2[wc1.id]?.calculation, "A <- 1 + 1\nA + 2", "Should remove the assignment when the last calculation assigns to the component id")
    test(result.statev2[wc2.id]?.calculation, "A <- 1 + 1", "Should leave the assignment when the last calculation is assigning to something other than the component id")
    test(result.action[wc3.id]?.calculation, `${uuid2} <- [${uuid2}] + [${uuid3}]\n${uuid1} <- [${uuid2}] - [${uuid3}]`, "Should get correct calculation string for action")
    test(result.action[wc3.id]?.linked_ids, [uuid2, uuid3, uuid1], "Should get correct calculation string for action")
})

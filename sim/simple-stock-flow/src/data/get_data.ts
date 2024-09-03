import { WComponent } from "../data_curator/src/wcomponent/interfaces/SpecialisedObjects"


export const IDS = {
    state_component__stock_a: "17edbf36-ad5b-4936-b3c5-7d803741c678",
    state_component__stock_b: "b644e33f-c00f-4a50-acc4-f158e4e11be5",

    variable_component__action_move_a_to_b: "action-move-a-to-b",
    variable_component__action_increase_a: "action-increase-a",

    flow_component__flow_into_a: "flow-into-a",
    flow_component__flow_a_to_b: "flow-a-to-b",

    // action_component__increase_stock_a: "8ecb8d21-4803-4028-9341-b7cd59b56cda",
    // action_component__move_a_to_b: "2dc650ae-8458-47b6-be0e-6ad1cab3cd4d",
}


// For now we manually copied over the data from http://localhost:5173/app/#wcomponents/17edbf36-ad5b-4936-b3c5-7d803741c678/&storage_location=1&subview_id=57721b40-5b26-4587-9cc3-614c6c366cae&view=knowledge&x=1218&y=-1538&zoom=68&sdate=2024-03-24&stime=22:42:19&cdate=2024-05-24&ctime=11:22:59
const _get_data = (): WComponent[] => [
    {
        id: "17edbf36-ad5b-4936-b3c5-7d803741c678",
        created_at: new Date("2024-05-03T15:49:53.649Z"),
        title: "Stock A",
        description: "",
        type: "statev2",
        values_and_prediction_sets: [
            {
                id: "vpsbe224cf2-9956-4174-86d7-ce4a0859a65d",
                created_at: new Date("2024-05-03T15:50:08.702Z"),
                base_id: 1,
                datetime: {},
                entries: [
                    {
                        id: "VAPc61f1e72-1873-4646-9286-efa8da3f4134",
                        explanation: "",
                        probability: 1,
                        conviction: 1,
                        value: "3",
                        description: "",
                        value_id: "e1aace4d-4dde-43d5-886e-c9b21b53d6f6"
                    }
                ]
            }
        ],
        label_ids: [],
        modified_by_username: "ajp",
        modified_at: new Date("2024-05-03T16:06:42.486Z"),
        subtype: "number",
        value_possibilities: {
            "e1aace4d-4dde-43d5-886e-c9b21b53d6f6": {
                id: "e1aace4d-4dde-43d5-886e-c9b21b53d6f6",
                value: "3",
                description: "",
                order: 0
            }
        },
        calculations: [
            {
                id: 0,
                name: "@@17edbf36-ad5b-4936-b3c5-7d803741c678",
                value: "max(@@17edbf36-ad5b-4936-b3c5-7d803741c678, 0)",
                result_description: "Validation calculation; ensures the value is always valid",
                result_sig_figs: 2
            }
        ],
        base_id: 1
    },
    {
        id: "b644e33f-c00f-4a50-acc4-f158e4e11be5",
        created_at: new Date("2024-05-03T15:59:42.459Z"),
        title: "Stock B",
        description: "",
        type: "statev2",
        values_and_prediction_sets: [
            {
                id: "vps2083b32a-3ea0-4d9c-af56-048075306db1",
                created_at: new Date("2024-05-03T16:00:02.021Z"),
                base_id: 1,
                datetime: {},
                entries: [
                    {
                        id: "VAPca079704-06a1-4bd8-b5f7-0feb256a430a",
                        explanation: "",
                        probability: 1,
                        conviction: 1,
                        value: "0",
                        description: "",
                        value_id: "cda07232-ed96-40a5-81b2-552927e9e9e4"
                    }
                ]
            }
        ],
        label_ids: [],
        modified_by_username: "ajp",
        modified_at: new Date("2024-05-03T16:07:58.663Z"),
        subtype: "number",
        value_possibilities: {
            "cda07232-ed96-40a5-81b2-552927e9e9e4": {
                id: "cda07232-ed96-40a5-81b2-552927e9e9e4",
                value: "0",
                description: "",
                order: 0
            }
        },
        calculations: [
            {
                id: 0,
                name: "@@b644e33f-c00f-4a50-acc4-f158e4e11be5",
                value: "max(@@b644e33f-c00f-4a50-acc4-f158e4e11be5, 0)",
                result_description: "Validation calculation; ensures the value is always valid"
            }
        ],
        base_id: 1
    },
    {
        id: "8ecb8d21-4803-4028-9341-b7cd59b56cda",
        created_at: new Date("2024-05-03T15:50:17.175Z"),
        title: "Increase stock A",
        description: "",
        type: "action",
        values_and_prediction_sets: [],
        label_ids: [],
        modified_by_username: "ajp",
        modified_at: new Date("2024-05-03T16:57:17.084Z"),
        reason_for_status: "",
        depends_on_action_ids: [],
        calculations: [
            {
                id: 1,
                name: "@@17edbf36-ad5b-4936-b3c5-7d803741c678",
                value: "@@17edbf36-ad5b-4936-b3c5-7d803741c678 + 1"
            }
        ],
        base_id: 1
    },
    {
        id: "2dc650ae-8458-47b6-be0e-6ad1cab3cd4d",
        created_at: new Date("2024-05-03T16:00:22.385Z"),
        title: "Move A to B",
        description: "@@26ed80ff-1043-43bc-bdd4-8bf06cd8ebc0",
        type: "action",
        values_and_prediction_sets: [],
        label_ids: [],
        modified_by_username: "ajp",
        modified_at: new Date("2024-05-24T16:51:25.392Z"),
        calculations: [
            {
                id: 0,
                name: "Stock A available",
                value: "If @@17edbf36-ad5b-4936-b3c5-7d803741c678 > 0 Then \\n  1\\nEnd If"
            },
            {
                id: 1,
                name: "@@b644e33f-c00f-4a50-acc4-f158e4e11be5",
                value: "@@b644e33f-c00f-4a50-acc4-f158e4e11be5 + [Stock A available]"
            },
            {
                id: 2,
                name: "@@17edbf36-ad5b-4936-b3c5-7d803741c678",
                value: "@@17edbf36-ad5b-4936-b3c5-7d803741c678 - [Stock A available]"
            }
        ],
        reason_for_status: "",
        depends_on_action_ids: [],
        base_id: 1
    },
]


export function get_data()
{
    return _get_data()
}

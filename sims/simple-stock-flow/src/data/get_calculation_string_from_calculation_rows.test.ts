import { describe, test } from "../data_curator/src/shared/utils/test"
import { get_calculation_string_from_calculation_rows } from "./get_calculation_string_from_calculation_rows"


export const test_get_calculation_string_from_calculation_rows = describe.delay("get_calculation_string_from_calculation_rows", () =>
{
    let result = get_calculation_string_from_calculation_rows([{
        name: "Some variable name",
        value: "1 + 2",
        id: -1,
    }, {
        name: "B",
        value: "1 + [Some variable name]",
        id: -1,
    }], true)
    test(result, "Some_variable_name <- 1 + 2\nB <- 1 + Some_variable_name", "convert spaces in variable names to underscores without square brackets")

    result = get_calculation_string_from_calculation_rows([{
        name: "A",
        value: "1 + 2",
        id: -1,
    }, {
        name: "@@10000000-0000-4000-a000-000000000000",
        value: "1 + [A]",
        id: -1,
    }], true)
    test(result, "A <- 1 + 2\n1 + A", "Removes uuid if last name in the calculation and flag is set to true for last_assigned_uuid_removed.  We do this otherwise simulationJS errors with \"You cannot set the value for that primitive.\"")

    result = get_calculation_string_from_calculation_rows([{
        name: "A",
        value: "1 + 2",
        id: -1,
    }, {
        name: "@@10000000-0000-4000-a000-000000000000",
        value: "1 + [A]",
        id: -1,
    }], false)
    test(result, "A <- 1 + 2\n[10000000-0000-4000-a000-000000000000] <- 1 + A", "Leaves uuid if last name in the calculation and flag is set to false for last_assigned_uuid_removed.  We leave the UUID so that simulationJS works correctly in assigning the value to the variable in an action.")
})

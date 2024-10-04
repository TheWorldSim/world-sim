import { test_get_calculation_string_from_calculation_rows } from "./data/get_calculation_string_from_calculation_rows.test"
import { test_get_wcomponents_values_by_id } from "./data/get_wcomponents_values_by_id.test"


export function run_all_tests()
{
    test_get_calculation_string_from_calculation_rows()
    test_get_wcomponents_values_by_id()
}

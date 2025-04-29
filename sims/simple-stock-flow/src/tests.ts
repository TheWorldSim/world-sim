import { test_get_calculation_string_from_calculation_rows } from "./data/get_calculation_string_from_calculation_rows.test"
import { test_get_wcomponents_values_by_id } from "./data/get_wcomponents_values_by_id.test"
import { tests_stats } from "./data_curator/src/shared/utils/test"
import { test_id_regexs } from "./data_curator/src/sharedf/rich_text/id_regexs.test"
import { test_make_model_stepper } from "./make_model_stepper.test"


export async function run_all_tests()
{
    tests_stats.reset()

    test_id_regexs()
    test_get_calculation_string_from_calculation_rows()
    test_get_wcomponents_values_by_id()

    ;await ((await test_make_model_stepper)())

    tests_stats.print()
}

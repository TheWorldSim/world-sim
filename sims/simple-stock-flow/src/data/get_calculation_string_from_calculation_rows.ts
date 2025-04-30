import { PlainCalculationObject } from "../data_curator/src/calculations/interfaces"
import { normalise_calculation_ids } from "../data_curator/src/calculations/normalise_calculation_ids"
import { get_double_at_mentioned_uuids_from_text } from "../data_curator/src/sharedf/rich_text/id_regexs"


type VariableNameMap = {unsafe_name: string, safe_name: string}[]

export function get_calculation_string_from_calculation_rows(calculations: PlainCalculationObject[] | undefined): string
{
    if (!calculations) return ""

    // We have to map names like "Some variable" to "Some_variable" because in DataCurator
    // the "Some variable" is made into a complete model variable as each calculation row
    // is treated like a new model so that the value of each calculation row can be shown
    // to the user.  However for our purposes here we only care about the whole set of
    // calculation rows being run at once and so we do not make a variable for each
    // calculation row.  So we need to ensure that the variable names are safe for
    // simulation.js to handle without error.
    const variable_name_map: VariableNameMap = []

    let calculation_strs: string[] = []
    calculations.forEach((c, i) =>
    {
        // Previously if the last calculation started with "@@" we would remove
        // it.  It's not clear why we did this.  This has been removed for now.
        // const is_last = i === calculations.length - 1
        // if (is_last && c.name.startsWith("@@"))
        // {
        //     calculation_strs.push(replace_unsafe_names(c.value, variable_name_map))
        //     return
        // }

        calculation_strs.push(`${ensure_name_is_safe(c.name, variable_name_map)} <- ${replace_unsafe_names(c.value, variable_name_map)}`)
    })
    let calculation = calculation_strs.join("\n")

    const calculation_uuids = get_double_at_mentioned_uuids_from_text(calculation)
    calculation = normalise_calculation_ids(calculation, calculation_uuids)

    return calculation
}


function replace_unsafe_names (text: string, variable_name_map: VariableNameMap)
{
    return variable_name_map.reduce((text, {unsafe_name, safe_name}) =>
    {
        return text.replaceAll(unsafe_name, safe_name)
    }, text)
}


function ensure_name_is_safe(name: string, variable_name_map: VariableNameMap)
{
    name = name.trim()

    if (name.startsWith("@@")) return name
    const existing_entry = variable_name_map.find(({unsafe_name}) => unsafe_name === name)
    if (existing_entry) return existing_entry.safe_name

    const safe_name = name.replace(/\s/g, "_")
    variable_name_map.push({safe_name, unsafe_name: name})
    // Additionally we also want to map from "[Some variable]" to "Some_variable" because
    // in DataCurator the calculation rows are treated as separate models and so the
    // valid variable names are "[Some variable]" in DataCurator when referenced on
    // subsequent calculation rows, but we need to use "Some_variable" here.
    // And we unshift so that it is used before the normal variable name
    variable_name_map.unshift({safe_name, unsafe_name: `[${name}]`})
    return safe_name
}

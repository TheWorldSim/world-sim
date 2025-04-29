// Copied from world-sim/sims/nature-sim/src/utils/string.ts

export function dedent(multiline_string: string): string
{
    const raw_lines = multiline_string.split("\n")
    const first_line = raw_lines[0] || ""  // Type guard
    const ignore_first = raw_lines.slice(1)
    const non_empty_lines = ignore_first.filter(line => line.trim().length > 0)
    const min_indent = Math.min(...non_empty_lines.map(line => line.match(/^(\s*)/)?.[0].length || 0))

    const lines: string[] = []
    const potential_lines: string[] = []
    let have_a_line_with_content = first_line.trim().length > 0
    if (have_a_line_with_content) lines.push(first_line)

    ignore_first.forEach(line =>
    {
        const trimmed = line.slice(min_indent)
        if (trimmed.trim().length > 0)
        {
            if (potential_lines.length)
            {
                lines.push(...potential_lines)
                potential_lines.length = 0
            }
            have_a_line_with_content = true
            lines.push(trimmed)
        }
        else if (have_a_line_with_content)
        {
            potential_lines.push("")
        }
    })

    return lines.length ? lines.join("\n") : ""
}


export function stringify_with_fixed(obj: unknown, spaces: number = 4, decimal_places: number = 2): string {
    return JSON.stringify(obj, (key, value) =>
        // eslint-disable-next-line @typescript-eslint/no-unsafe-return
        typeof value === "number" ? Number(value.toFixed(decimal_places)) : value,
        spaces
    );
}

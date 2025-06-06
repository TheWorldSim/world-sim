

export function is_number(value: unknown): value is number
{
    return typeof value === "number" && !Number.isNaN(value)
}

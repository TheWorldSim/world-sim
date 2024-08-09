

export interface SolverGenericState
{
    [key: string]: number
}


function update_state<S extends SolverGenericState>(state: S, diff: S, time_step: number): S
{
    const new_state: S = {...state}
    Object.entries(diff).forEach(([key, value]) => {
        if (new_state[key] === undefined) throw new Error(`Key ${key} not found in state object`)
        // @ts-ignore -- giving error: "Type 'S' is generic and can only be indexed for reading.ts(2862)"
        new_state[key] += (value * time_step)
    })

    return new_state
}


export function rk4_step_solver<S extends SolverGenericState>(func: (time: number, state: S) => S, time: number, initial_state: S, time_step: number): S
{
    const k1 = func(time, initial_state);

    const k2 = func(time + time_step/2, update_state(initial_state, k1, time_step/2));
    const k3 = func(time + time_step/2, update_state(initial_state, k2, time_step/2));
    const k4 = func(time + time_step, update_state(initial_state, k3, time_step));

    const new_state: S = {...initial_state}
    Object.keys(initial_state).forEach(key =>
    {
        if (k1[key] === undefined || k2[key] === undefined || k3[key] === undefined || k4[key] === undefined) throw new Error(`Key ${key} not found in k1, k2, k3, or k4 object`)
        const value_diff = (k1[key] + 2*k2[key] + 2*k3[key] + k4[key]) * time_step/6

        // @ts-ignore -- giving error: "Type 'S' is generic and can only be indexed for reading.ts(2862)"
        new_state[key] += value_diff
    })

    return new_state
}


export function euler_step_solver<S extends SolverGenericState>(func: (time: number, state: S) => S, time: number, initial_state: S, time_step: number): S
{
    const state_diff = func(time, initial_state)

    const new_state: S = {...initial_state}
    Object.entries(state_diff).forEach(([key, value]) =>
    {
        const value_diff = value * time_step

        // @ts-ignore -- giving error: "Type 'S' is generic and can only be indexed for reading.ts(2862)"
        new_state[key] += value_diff
    })

    return new_state
}

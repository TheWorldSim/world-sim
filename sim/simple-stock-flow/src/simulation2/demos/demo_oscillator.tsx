import { DataSeries, GraphSingleData } from "../graph";
import { euler_step_solver, rk4_step_solver, SolverGenericState } from "../solvers"


export function DemoOscillator ()
{
    interface SpecificState{
        position: number
        velocity: number
    }


    interface State extends SolverGenericState, SpecificState {}

    // Define the angular frequency (ω)
    const omega = 1; // You can adjust this value as needed

    /**
     * Function representing the system of ODEs for a simple harmonic oscillator
     * @param {number} time - Current time (not used in this case, but included for generality)
     * @param {Array} y - Array containing [position, velocity]
     * @returns {Array} Array containing rates of change [dx/dt, dv/dt]
     */
    function func(time: number, y: State): State
    {
        const x = y.position; // Position
        const v = y.velocity; // Velocity

        // dx/dt = v
        const dxdt = v;

        // dv/dt = -ω^2 * x
        const dvdt = -Math.pow(omega, 2) * x;

        const diff: State = {position: dxdt, velocity: dvdt}

        return diff
    }

    // Initial conditions
    let t = 0;

     // Initial position = 1, initial velocity = 0
    // let y_naive: State = {position: 1, velocity: 0 };
    const y_euler_: SpecificState = {position: 1, velocity: 0 }  // type assertions
    const y_rk4_: SpecificState = {position: 1, velocity: 0 }  // type assertions
    let y_euler: State = {...y_euler_}
    let y_rk4: State = {...y_rk4_}

    function as_percent(v1: number, v2: number)
    {
        const value = ((v1 - v2) / v1) * 100
        return `(${value.toFixed(2)}%)`;
    }


    const euler_position_data: DataSeries = {
        name: "Euler",
        colour: "blue",
        data: [{x: 0, y: y_euler.position}]
    }
    const euler_velocity_data: DataSeries = {
        name: "Euler",
        colour: "green",
        data: [{x: 0, y: y_euler.velocity}]
    }

    const rk4_position_data: DataSeries = {
        name: "RK4",
        colour: "blue",
        data: [{x: 0, y: y_rk4.position}]
    }
    const rk4_velocity_data: DataSeries = {
        name: "RK4",
        colour: "green",
        data: [{x: 0, y: y_rk4.velocity}]
    }


    const time_step = 0.3; // Step size

    for (let i = 0; i < 200; i++)
    {
        console.log(`t: ${t.toFixed(2)}, position: ${y_rk4.position.toFixed(4)} ${as_percent(y_rk4.position, y_euler.position)}, velocity: ${y_rk4.velocity.toFixed(4)}  ${as_percent(y_rk4.velocity, y_euler.velocity)}`);
        y_euler = euler_step_solver<State>(func, t, y_euler, time_step);
        y_rk4 = rk4_step_solver<State>(func, t, y_rk4, time_step);
        t += time_step;

        const rounded_t = Math.round(t * 100) / 100

        euler_position_data.data.push({x: rounded_t, y: y_euler.position})
        euler_velocity_data.data.push({x: rounded_t, y: y_euler.velocity})

        rk4_position_data.data.push({x: rounded_t, y: y_rk4.position})
        rk4_velocity_data.data.push({x: rounded_t, y: y_rk4.velocity})
    }



    return <>
        <GraphSingleData data_series={euler_position_data} />
        <GraphSingleData data_series={euler_velocity_data} />
        <GraphSingleData data_series={rk4_position_data} />
        <GraphSingleData data_series={rk4_velocity_data} />
    </>
}

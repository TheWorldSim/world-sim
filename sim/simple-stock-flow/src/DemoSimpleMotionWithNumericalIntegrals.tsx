import { DataSeries, GraphMotionData, GraphSingleData } from "./simulation2/graph"


export function DemoSimpleMotionWithNumericalIntegrals ()
{
    return <>
        {/* <GraphMotionData {...simulate_simple_motion()} />
        <GraphMotionData {...simulate_complex_motion()} /> */}
        <GraphMotionData {...simulate_motion_with_feedback(1.9, true)} />
        <GraphMotionData {...simulate_motion_with_feedback(1.8, true)} />
        <GraphMotionData {...simulate_motion_with_feedback(1, true)} />
        <GraphMotionData {...simulate_motion_with_feedback(0.1, true)} />
        {/* <GraphSingleMotionData data_series={simulate_motion_with_feedback(0.1, true).acceleration_debug_data_hill} />
        <GraphSingleMotionData data_series={simulate_motion_with_feedback(0.1, true).acceleration_debug_data_drag} /> */}
    </>
}


function simulate_simple_motion ()
{
    const acceleration_data: DataSeries = {
        name: "Acceleration",
        colour: "red",
        data: []
    }

    let t = 0
    while (t < 10)
    {
        let a = t > 1 && t < 8 ? 1 : 0
        acceleration_data.data.push({ y: a, x: t })
        t += 1
    }

    const velocity_data = calculate_velocity(acceleration_data)
    const position_data = calculate_position(velocity_data)

    return {
        acceleration_data,
        velocity_data,
        position_data,
    }
}


function calculate_velocity (acceleration_data: DataSeries, time_step_seconds=1): DataSeries
{
    const velocity_data: DataSeries = {
        name: "Velocity",
        colour: "green",
        data: []
    }

    let v = 0
    Object.values(acceleration_data.data)
    .forEach(({y: x, x: y}, index) =>
    {
        const acceleration = x
        const t = y
        // const t = y

        v += acceleration
        velocity_data.data.push({ y: v, x: t })
    })

    return velocity_data
}


function calculate_position (velocity_data: DataSeries, time_step_seconds=1): DataSeries
{
    const position_data: DataSeries = {
        name: "Position",
        colour: "blue",
        data: []
    }

    let v = 0
    Object.values(velocity_data.data)
    .forEach(({y: x, x: y}, index) =>
    {
        const velocity = x
        const t = y
        // const t = y

        v += velocity
        position_data.data.push({ y: v, x: t })
    })

    return position_data
}


function simulate_complex_motion ()
{
    const acceleration_data: DataSeries = {
        name: "Acceleration",
        colour: "red",
        data: [
            { y: 0,   x: 0  },
            { y: 1,   x: 1  },
            { y: 0.5, x: 2  },
            { y: 0.2, x: 3  },
            { y: 2,   x: 4  },
            { y: 3,   x: 5  },
            { y: 4.5, x: 6  },
            { y: 5,   x: 7  },
            { y: 5.2, x: 8  },
            { y: 4.9, x: 9  },
            { y: 4.3, x: 10 },
        ]
    }

    let t = 11
    while (t < 20)
    {
        let a = t < 18 ? -7 : 0
        acceleration_data.data.push({ y: a, x: t })
        t += 1
    }

    const velocity_data = calculate_velocity(acceleration_data)
    const position_data = calculate_position(velocity_data)

    return {
        acceleration_data,
        velocity_data,
        position_data,
    }
}


function simulate_motion_with_feedback (time_step_seconds: number, include_drag: boolean = false)
{
    const acceleration_data: DataSeries = { name: "Acceleration", colour: "red", data: [] }
    const velocity_data: DataSeries = { name: "Velocity", colour: "green", data: [] }
    const position_data: DataSeries = { name: "Position", colour: "blue", data: [] }


    const acceleration_debug_data_hill: DataSeries = { name: "Acceleration", colour: "red", data: [] }
    const acceleration_debug_data_drag: DataSeries = { name: "Acceleration", colour: "red", data: [] }


    function acceleration_for_position_and_velocity (position: number, velocity: number)
    {
        if (position < 0) throw new Error(`Position out of range: ${position}`)

        let position_as_ratio = Math.min(position, 20) / 20
        position_as_ratio *= 2
        position_as_ratio *= Math.PI

        const acceleration_from_engine = 1
        // Simulate deceleration from a hill gradient at different positions, i.e. going from flat, to up a hill, to onto the plateau at the top
        const acceleration_from_hill = Math.cos(position_as_ratio)

        const drag_from_velocity = include_drag ? -0.2 * Math.pow(velocity, 2) : 0

        const acceleration = acceleration_from_engine + acceleration_from_hill + drag_from_velocity

        return {
            acceleration,
            hill: acceleration_from_hill,
            drag: drag_from_velocity,
        }
    }

    let last_position = 0
    let last_velocity = 0
    let last_acceleration = 0
    position_data.data.push({ y: last_position, x: 0 })
    velocity_data.data.push({ y: last_velocity, x: 0 })
    acceleration_data.data.push({ y: last_acceleration, x: 0 })

    let t = 0
    while (t < 17)
    {
        const acceleration_results = acceleration_for_position_and_velocity(last_position, last_velocity)
        last_acceleration = acceleration_results.acceleration
        last_velocity = last_velocity + last_acceleration * time_step_seconds
        last_position = last_position + last_velocity * time_step_seconds

        t += time_step_seconds

        acceleration_data.data.push({ y: last_acceleration, x: t })
        velocity_data.data.push({ y: last_velocity, x: t })
        position_data.data.push({ y: last_position, x: t })


        acceleration_debug_data_hill.data.push({ y: acceleration_results.hill, x: t })
        acceleration_debug_data_drag.data.push({ y: acceleration_results.drag, x: t })
    }

    return {
        time_step_seconds,
        acceleration_data,
        velocity_data,
        position_data,

        acceleration_debug_data_hill,
        acceleration_debug_data_drag,
    }
}

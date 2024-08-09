import { useEffect, useState } from "preact/hooks"


export function DemoAppDoublePendulum ()
{
    return <canvas
        id="canvas"
        width="800"
        height="800"
        ref={canvas =>
        {
            if (!canvas) return

            const ctx = canvas.getContext("2d")!
            const width = canvas.width
            const height = canvas.height
            const g = 9.81
            const l1 = 100
            const l2 = 100
            const m1 = 10
            const m2 = 10
            let theta1 = Math.PI / 2
            let theta2 = Math.PI / 2
            let omega1 = 0
            let omega2 = 0
            const time_step = 0.1

            let t = 0
            let x1 = 0
            let y1 = 0
            let x2 = 0
            let y2 = 0

            function update (
                theta1: number,
                theta2: number,
                omega1: number,
                omega2: number,
                time_step: number
            )
            {
                const num1 = -g * (2 * m1 + m2) * Math.sin(theta1)
                const num2 = -m2 * g * Math.sin(theta1 - 2 * theta2)
                const num3 = -2 * Math.sin(theta1 - theta2) * m2
                const num4 = omega2 * omega2 * l2 + omega1 * omega1 * l1 * Math.cos(theta1 - theta2)
                const den = l1 * (2 * m1 + m2 - m2 * Math.cos(2 * theta1 - 2 * theta2))
                const omega1_diff = (num1 + num2 + num3 * num4) / den

                const num5 = 2 * Math.sin(theta1 - theta2)
                const num6 = omega1 * omega1 * l1 * (m1 + m2)
                const num7 = g * (m1 + m2) * Math.cos(theta1)
                const num8 = omega2 * omega2 * l2 * m2 * Math.cos(theta1 - theta2)
                const den2 = l2 * (2 * m1 + m2 - m2 * Math.cos(2 * theta1 - 2 * theta2))
                const omega2_diff = (num5 * (num6 + num7 + num8)) / den2

                return {
                    omega1: omega1 + omega1_diff * time_step,
                    omega2: omega2 + omega2_diff * time_step,
                    theta1: theta1 + omega1 * time_step,
                    theta2: theta2 + omega2 * time_step,
                }
            }

            function calculate_energy_in_system (theta1: number, theta2: number, omega1: number, omega2: number)
            {
                const potential_energy = -m1 * g * l1 * Math.cos(theta1) - m2 * g * (l1 * Math.cos(theta1) + l2 * Math.cos(theta2))
                const kinetic_energy = 0.5 * m1 * l1 * l1 * omega1 * omega1 + 0.5 * m2 * (l1 * l1 * omega1 * omega1 + l2 * l2 * omega2 * omega2 + 2 * l1 * l2 * omega1 * omega2 * Math.cos(theta1 - theta2))
                return {
                    potential_energy,
                    kinetic_energy,
                    total_energy: potential_energy + kinetic_energy,
                }
            }

            function draw ()
            {
                const x1 = l1 * Math.sin(theta1)
                const y1 = l1 * Math.cos(theta1)

                const x2 = x1 + l2 * Math.sin(theta2)
                const y2 = y1 + l2 * Math.cos(theta2)

                ctx.clearRect(0, 0, width, height)
                ctx.beginPath()
                ctx.moveTo(width / 2, height / 2)
                ctx.lineTo(x1 + width / 2, y1 + height / 2)
                ctx.lineTo(x2 + width / 2, y2 + height / 2)
                ctx.stroke()
            }

            function animate ()
            {
                const new_state = update(theta1, theta2, omega1, omega2, time_step)
                theta1 = new_state.theta1
                theta2 = new_state.theta2
                omega1 = new_state.omega1
                omega2 = new_state.omega2
                draw()

                const energy = calculate_energy_in_system(theta1, theta2, omega1, omega2)
                // Write energy to screen
                ctx.font = "30px Arial"
                ctx.fillText(`Potential energy: ${energy.potential_energy.toFixed(2)}`, 10, 50)
                ctx.fillText(`Kinetic energy: ${energy.kinetic_energy.toFixed(2)}`, 10, 100)
                ctx.fillText(`Total energy: ${energy.total_energy.toFixed(2)}`, 10, 150)

                requestAnimationFrame(animate)
            }

            animate()

        }}
    />
}
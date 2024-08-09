import { Chart, registerables } from "chart.js"
Chart.register(...registerables)


export interface DataSeries
{
    name: string
    colour: "red" | "green" | "blue"
    data: {y: number, x: number}[]
}


export function GraphMotionData (props: {acceleration_data: DataSeries, velocity_data: DataSeries, position_data: DataSeries})
{
    const {
        acceleration_data,
        velocity_data,
        position_data,
    } = props

    return <div style={{ display: "flex", height: 400, width: 800 }}>
        <GraphSingleData data_series={acceleration_data} />
        <GraphSingleData data_series={velocity_data} />
        <GraphSingleData data_series={position_data} />
    </div>
}


export function GraphSingleData (props: {data_series: DataSeries})
{
    const { data_series } = props

    let borderColor = "rgba(192, 75, 75, 1)"
    if (data_series.colour === "green") borderColor = "rgba(75, 192, 75, 1)"
    if (data_series.colour === "blue") borderColor = "rgba(75, 75, 192, 1)"

    const labels = data_series.data.map(d => d.x)//Math.round(d.y * 1000) / 1000)

    return <canvas
        style={{ maxWidth: "400px" }}
        ref={canvas => {
            if (!canvas) return;
            const chart = new Chart(canvas, {
                type: "line",
                data: {
                    labels,
                    datasets: [
                        {
                            label: data_series.name,
                            data: data_series.data.map(d => d.y),
                            borderColor,
                            borderWidth: 1,
                            fill: false,
                        },
                    ]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true,
                            // title: {
                            //     display: true,
                            //     text: data.name
                            // }
                        }
                    }
                }
            });
        }}
    />
}

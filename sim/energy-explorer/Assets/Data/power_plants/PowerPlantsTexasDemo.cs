using System;
using System.Collections.Generic;

public static class PowerPlantsTexasDemo
{
	public static List<PowerPlantData> power_plants = new List<PowerPlantData>
	{
		new PowerPlantData_ThermalCombustion(
			id: "13245",
			name: "Plant Coal 1",
			lon_lat: new LonLat(-96.644743f, 28.895486f),
			outline: new List<LonLat> {
				new LonLat(-96.644743f, 28.895486f)
			},
			construction_start_date: DateTime.Parse("2018-01-01"),
			generation_start_date: DateTime.Parse("2019-01-01"),
			power_mw: 1600,
			operator_name: "Blob ex",
			mdollar_capex: 1e7f,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal()
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "2",
			name: "Plant Coal;Oil 2",
			lon_lat: new LonLat(-93.644743f, 28.895486f),
			outline: new List<LonLat> {
				new LonLat(-93.644743f, 28.895486f)
			},
			construction_start_date: DateTime.Parse("2018-01-01"),
			generation_start_date: DateTime.Parse("2019-01-01"),
			power_mw: 1800,
			operator_name: "Blob ex",
			mdollar_capex: 1.8e7f,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Coal(),
				new PowerSource_Combustable_Oil(),
			}
		),
		new PowerPlantData_ThermalCombustion(
			id: "3",
			name: "Plant Gas 3",
			lon_lat: new LonLat(-90.644743f, 28.895486f),
			outline: new List<LonLat> {
				new LonLat(-90.644743f, 28.895486f)
			},
			construction_start_date: DateTime.Parse("2018-01-01"),
			generation_start_date: DateTime.Parse("2019-01-01"),
			power_mw: 1200,
			operator_name: "Blob ex",
			mdollar_capex: 1.8e7f,
			power_sources: new PowerSource_Combustable[]
			{
				new PowerSource_Combustable_Gas(),
			}
		),
		new PowerPlantData_WindFarm(
			id: "4",
			name: "Plant Wind 4",
			lon_lat: new LonLat(-106.644743f, 31.895486f),
			outline: new List<LonLat> {
				new LonLat(-106.644743f, 31.895486f),
				new LonLat(-106.544743f, 31.795486f),
				new LonLat(-106.744743f, 31.995486f)
			},
			construction_start_date: DateTime.Parse("2018-01-01"),
			generation_start_date: DateTime.Parse("2019-01-01"),
			power_mw: 400,
			operator_name: "Blob ex",
			mdollar_capex: 1.8e7f
		),
		new PowerPlantData_SolarFarm(
			id: "5",
			name: "Plant Solar 5",
			lon_lat: new LonLat(-103.644743f, 31.895486f),
			outline: new List<LonLat> {
				new LonLat(-103.644743f, 31.895486f)
			},
			construction_start_date: DateTime.Parse("2018-01-01"),
			generation_start_date: DateTime.Parse("2019-01-01"),
			power_mw: 200,
			operator_name: "Blob ex",
			mdollar_capex: 1.8e7f
		),
		new PowerPlantData_NuclearFission(
			id: "6",
			name: "Plant Nuclear 6",
			lon_lat: new LonLat(-100.644743f, 31.895486f),
			outline: new List<LonLat> {
				new LonLat(-100.644743f, 31.895486f)
			},
			construction_start_date: DateTime.Parse("2018-01-01"),
			generation_start_date: DateTime.Parse("2019-01-01"),
			power_mw: 3000,
			operator_name: "Blob ex",
			mdollar_capex: 1.8e7f
		)
	};
}

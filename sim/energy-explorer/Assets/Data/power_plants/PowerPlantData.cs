using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CombustiblePowerSource
{
	none = 0,
	biomass = 1,
	coal = 2,
	oil = 3,
	gas = 4
}

public static class CompareCombustiblePowerSource
{
	public static bool same_combustible_power_sources(List<CombustiblePowerSource> first, List<CombustiblePowerSource> other)
	{
		var first_set = new HashSet<CombustiblePowerSource>(first);
		var other_set = new HashSet<CombustiblePowerSource>(other);

		if (first_set.Count != other_set.Count) return false;

		var all_first = first.Aggregate(true, (bool accum, CombustiblePowerSource value) =>
		{
			return accum && other_set.Contains(value);
		});

		if (!all_first) return false;

		return other.Aggregate(true, (bool accum, CombustiblePowerSource value) =>
		{
			return accum && first_set.Contains(value);
		});
	}
}

public class PowerPlantData
{
	public BuildingType building_type { get; private set; }
	public string id { get; private set; }
	public string name { get; private set; }
	public LonLat lon_lat { get; private set; }

	public List<LonLat> outline { get; private set; }
	public DateTime? construction_start_date { get; private set; }
	public DateTime? generation_start_date { get; private set; }
	// max == nameplate
	public float power_nameplate_mw { get; private set; }
	public float generation_nameplate_GW { get; private set; }
	public float storage_max_GJ { get; private set; }
	public float storage_current_GJ { get; private set; }
	public float storage_charging_efficiency { get; private set; }
	public float storage_discharging_efficiency { get; private set; }
	public float storage_efficiency { get; private set; }
	public string operator_name { get; private set; }
	public float capex_mdollar { get; private set; }
	public float opex_mdollar_per_year { get; private set; }
	public float dollar_opex_per_GJ { get; private set; }
	public float facility_life_time_y { get; private set; }
	public bool is_renewable { get; private set; }
	public bool is_nuclear { get; private set; }
	public List<CombustiblePowerSource> combustible_power_sources { get; private set; }
	public bool is_fossil { get; private set; }
	public bool is_storage { get; private set; }
	public bool is_primary_generation { get; private set; }

	public PowerPlantData(
		BuildingType building_type,
		string id,
		string name,
		LonLat lon_lat,
		List<LonLat> outline,
		DateTime? construction_start_date,
		DateTime? generation_start_date,
		float power_nameplate_mw,
		string operator_name,
		float mdollar_capex,
		float storage_max_GJ = 0,
		float storage_current_GJ = 0,
		float storage_charging_efficiency = 1,
		float storage_discharging_efficiency = 1,
		List<CombustiblePowerSource> combustible_power_sources = null,
		float mdollar_opex_per_year = 0,
		float dollar_opex_per_GJ = 0,
		float facility_life_time_y = 20)
	{
		this.building_type = building_type;
		this.id = id;
		this.name = name;
		this.lon_lat = lon_lat;
		this.outline = outline;
		this.construction_start_date = construction_start_date;
		this.generation_start_date = generation_start_date;
		this.power_nameplate_mw = power_nameplate_mw;
		this.generation_nameplate_GW = power_nameplate_mw / 1000;
		this.storage_max_GJ = storage_max_GJ;
		this.storage_current_GJ = storage_current_GJ;
		this.storage_charging_efficiency = storage_charging_efficiency;
		this.storage_discharging_efficiency = storage_discharging_efficiency;
		this.storage_efficiency = storage_charging_efficiency * storage_discharging_efficiency;
		this.operator_name = operator_name;
		this.capex_mdollar = mdollar_capex;
		this.opex_mdollar_per_year = mdollar_opex_per_year;
		this.dollar_opex_per_GJ = dollar_opex_per_GJ;
		this.facility_life_time_y = facility_life_time_y;
		this.combustible_power_sources = combustible_power_sources == null ? new List<CombustiblePowerSource>() : combustible_power_sources;
		this.is_renewable = building_type == BuildingType.wind_farm
				|| building_type == BuildingType.solar_panels
				|| building_type == BuildingType.hydro
				|| (building_type == BuildingType.thermal_combustion && combustible_power_sources.Contains(CombustiblePowerSource.biomass));
		this.is_nuclear = building_type == BuildingType.nuclear_fission;
		this.is_fossil = building_type == BuildingType.thermal_combustion && !combustible_power_sources.Contains(CombustiblePowerSource.biomass);
		this.is_storage = building_type == BuildingType.tier1_energy_storage
				|| building_type == BuildingType.tier2_energy_storage
				//|| building_type == BuildingType.hydro
				|| building_type == BuildingType.pumped_hydro;
		this.is_primary_generation = is_renewable || is_nuclear || is_fossil;
	}

	internal bool same_combustible_power_sources(List<CombustiblePowerSource> combustible_power_sources)
	{
		return CompareCombustiblePowerSource.same_combustible_power_sources(this.combustible_power_sources, combustible_power_sources);
	}

	internal void set_lon_lat(Vector2 get_lat_lon_of_cursor)
	{
		this.lon_lat = LonLat.from_vec2(get_lat_lon_of_cursor);
	}
}

public abstract class PowerSource_Combustable { }

public class PowerSource_Combustable_Biomass : PowerSource_Combustable { }

public class PowerSource_Combustable_Coal : PowerSource_Combustable { }

public class PowerSource_Combustable_Oil : PowerSource_Combustable { }

public class PowerSource_Combustable_Gas : PowerSource_Combustable { }

public class PowerPlantData_ThermalCombustion : PowerPlantData
{
	public PowerSource_Combustable[] power_sources { get; protected set; }

	public PowerPlantData_ThermalCombustion(
		string id,
		string name,
		LonLat lon_lat,
		List<LonLat> outline,
		DateTime? construction_start_date,
		DateTime? generation_start_date,
		float power_mw,
		string operator_name,
		float mdollar_capex,
		PowerSource_Combustable[] power_sources
	) : base(
		BuildingType.thermal_combustion,
		id,
		name,
		lon_lat,
		outline,
		construction_start_date,
		generation_start_date,
		power_mw,
		operator_name,
		mdollar_capex
	)
	{
		this.power_sources = power_sources;
	}
}

public class PowerPlantData_NuclearFission : PowerPlantData
{
	public PowerPlantData_NuclearFission(
		string name,
		string id,
		LonLat lon_lat,
		List<LonLat> outline,
		DateTime? construction_start_date,
		DateTime? generation_start_date,
		float power_mw,
		string operator_name,
		float mdollar_capex
	) : base(
		BuildingType.nuclear_fission,
		id,
		name,
		lon_lat,
		outline,
		construction_start_date,
		generation_start_date,
		power_mw,
		operator_name,
		mdollar_capex
	)
	{
		// pass
	}
}

public class PowerPlantData_SolarFarm : PowerPlantData
{
	//public PowerSource_SolarPanel[] solar_panels { get; protected set; }
	public PowerPlantData_SolarFarm(
		string id,
		string name,
		LonLat lon_lat,
		List<LonLat> outline,
		DateTime? construction_start_date,
		DateTime? generation_start_date,
		float power_mw,
		string operator_name,
		float mdollar_capex
	) : base(
		BuildingType.solar_panels,
		id,
		name,
		lon_lat,
		outline,
		construction_start_date,
		generation_start_date,
		power_mw,
		operator_name,
		mdollar_capex
	)
	{
		// pass
	}
}

public class PowerPlantData_WindFarm : PowerPlantData
{
	//public PowerSource_WindTurbine[] wind_turbines { get; protected set; }

	public PowerPlantData_WindFarm(
		string id,
		string name,
		LonLat lon_lat,
		List<LonLat> outline,
		DateTime? construction_start_date,
		DateTime? generation_start_date,
		float power_mw,
		string operator_name,
		float mdollar_capex
	) : base(
		BuildingType.wind_farm,
		id,
		name,
		lon_lat,
		outline,
		construction_start_date,
		generation_start_date,
		power_mw,
		operator_name,
		mdollar_capex
	)
	{
		// pass
	}

	//public PowerPlantData_WindFarm(
	//	LonLat lon_lat,
	//	List<LonLat> outline,
	//	DateTime? construction_start_date,
	//	float power_mw
	//) : base(
	//	osm_id: "-1",
	//	name: "Wind farm group",
	//	lon_lat,
	//	outline,
	//	construction_start_date,
	//	generation_start_date: default,
	//	power_mw,
	//	operator_name: "you",
	//	dollar_capex: 0
	//)
	//{

	//	dollar_capex = ;
	//}
}

public class PowerPlantData_Hydro : PowerPlantData
{
	public PowerPlantData_Hydro(
		string id,
		string name,
		LonLat lon_lat,
		List<LonLat> outline,
		DateTime? construction_start_date,
		DateTime? generation_start_date,
		float power_mw,
		string operator_name,
		float mdollar_capex
	) : base(
		BuildingType.hydro,
		id,
		name,
		lon_lat,
		outline,
		construction_start_date,
		generation_start_date,
		power_mw,
		operator_name,
		mdollar_capex
	)
	{
		// pass
	}
}


public static class TestPowerPlantData
{
	private static int i = 0;
	public static PowerPlantData generate_power_plant_data()
	{
		++i;

		return new PowerPlantData(
			building_type: BuildingType.wind_farm,
			id: "test " + i,
			name: "test name " + i,
			lon_lat: new LonLat(new Vector2(0, 0)),
			outline: new List<LonLat>() { LonLat.from_vec2(new Vector2(0,0)), LonLat.from_vec2(new Vector2(1, 1)) },
			construction_start_date: DateTime.Parse("2020-02-10 19:01"),
			generation_start_date: DateTime.Parse("2020-02-10 19:01"),
			power_nameplate_mw: 1000,
			operator_name: "test operator " + i,
			mdollar_capex: 1000,
			storage_max_GJ: 0,
			storage_current_GJ: 0,
			storage_charging_efficiency: 1,
			storage_discharging_efficiency: 1,
			combustible_power_sources: null,
			mdollar_opex_per_year: 0,
			dollar_opex_per_GJ: 0,
			facility_life_time_y: 20
		);
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using WPM;


public class NewBuildingManager : MonoBehaviour
{
	public static NewBuildingManager instance { get; private set; }

	public delegate void ChangedBuildingActive(BuildingType building_type, List<CombustiblePowerSource> combustible_power_sources);
	public event ChangedBuildingActive on_changed_building_active;
	public delegate void ChangedBuildings(PowerPlantData data);
	public event ChangedBuildings on_changed_buildings;
	public delegate void ChangedPowerSizeGW(PowerSizeGW power_size_GW);
	public event ChangedPowerSizeGW on_changed_power_size_GW;

	public GameObject wind_turbine_prefab;
	public GameObject solar_panel_prefab;
	public GameObject tier1_energy_storage_prefab;
	public GameObject tier2_energy_storage_prefab;
	public GameObject forest_prefab;
	public GameObject thermal_combustion_prefab;
	public GameObject nuclear_fission_prefab;
	public GameObject hydro_prefab;
	private Dictionary<BuildingType, GameObject> _building_type_to_prefab = new Dictionary<BuildingType, GameObject>();
	private BuildingType last_active_building_type;
	private List<CombustiblePowerSource> last_active_combustible_power_sources;

	public PowerSizeGW power_size_GW { get; private set; }

	void Awake()
    {
		instance = this;

		set_prefab(BuildingType.wind_farm, wind_turbine_prefab);
		set_prefab(BuildingType.solar_panels, solar_panel_prefab);
		set_prefab(BuildingType.tier1_energy_storage, tier1_energy_storage_prefab);
		set_prefab(BuildingType.tier2_energy_storage, tier2_energy_storage_prefab);
		set_prefab(BuildingType.forest, forest_prefab);
		set_prefab(BuildingType.thermal_combustion, thermal_combustion_prefab);
		set_prefab(BuildingType.nuclear_fission, nuclear_fission_prefab);
		set_prefab(BuildingType.hydro, hydro_prefab);
		set_prefab(BuildingType.pumped_hydro, hydro_prefab);

		power_size_GW = PowerSizeGW.twenty;
	}

	public void set_build_active(BuildingType building_type, List<CombustiblePowerSource> combustible_power_sources)
	{
		last_active_building_type = building_type;
		last_active_combustible_power_sources = combustible_power_sources;

		GameManagerWorldSim.cell_visuals.clear_nominal_region_of_capacity_factors();
		on_changed_building_active?.Invoke(building_type, combustible_power_sources);
		add_potential_building_to_scene(building_type, combustible_power_sources);
	}

	public void set_build_inactive()
	{
		on_changed_building_active?.Invoke(default, null);

		// This should be coordinated on a CursorManager
		Cursor.visible = true;
		WorldMapGlobe.instance.showCursor = false;

		GameManagerWorldSim.cell_visuals.clear_nominal_region_of_capacity_factors();
	}

	internal void built_building(PowerPlantData data)
	{
		on_changed_buildings?.Invoke(data);
		// Add next potential building of same type as one just built
		add_potential_building_to_scene(data.building_type, data.combustible_power_sources);
	}

	private void set_prefab(BuildingType building_type, GameObject prefab)
	{
		if (prefab == default(GameObject))
		{
			Debug.LogError("No prefab set for building type: " + building_type + " in NewBuildingManager");
		}
		_building_type_to_prefab[building_type] = prefab;
	}

	private void add_potential_building_to_scene(BuildingType building_type, List<CombustiblePowerSource> combustible_power_sources)
	{
		// Debug.Log("add_potential_building_to_scene " + building_type);

		// This should be coordinated on a CursorManager
		Cursor.visible = false;
		WorldMapGlobe.instance.showCursor = true;

		var giga_watts = (float)power_size_GW;
		var data = NewBuildingData.power_plant_data(giga_watts, building_type, combustible_power_sources);
		var new_building = add_gameobject_building(building_type);

		var power_plant = new_building.GetComponent<PowerPlant>();
		power_plant.Init(data);

		var potential_building = new_building.AddComponent<PotentialBuilding>();
		var roi = GameManagerWorldSim.instance.selected_region_of_interest;
		potential_building.Init(data, roi);
	}

	public GameObject add_gameobject_building(BuildingType building_type)
	{
		var prefab = _building_type_to_prefab[building_type];
		var new_building = Instantiate(prefab, transform);
		return new_building;
	}

	public void change_power_size_GW(PowerSizeGW power_size_GW)
	{
		if (this.power_size_GW != power_size_GW)
		{
			// This approach for refreshing the potential building parameters feels too hacky
			var building_type = last_active_building_type;
			var combustible_power_sources = last_active_combustible_power_sources;
			set_build_inactive();

			this.power_size_GW = power_size_GW;
			on_changed_power_size_GW?.Invoke(this.power_size_GW);

			// again more hacks.  TODO: refactor this
			if (building_type != BuildingType.none)
			{
				set_build_active(building_type, combustible_power_sources);
			}
		}
	}
}


public static class NewBuildingData
{
	private static int next_id = 1;

	public static PowerPlantData power_plant_data(float giga_watts, BuildingType building_type, List<CombustiblePowerSource> combustible_power_sources)
	{
		// Assuming at equator
		float earth_radius_m = 6371000;
		var circumference = Mathf.PI * earth_radius_m * 2;
		float _1_km = 1000;
		var ratio = _1_km / circumference;
		var degrees = ratio * 360;

		LonLat dimensions_1km_square = new LonLat(degrees, degrees);

		float watts = giga_watts * 1e9f;

		var name = "New " + building_type;
		var date_time_value = GameManagerWorldSim.date_time.game_date_time;
		var lon_lat = new LonLat(0, 0);
		float km_of_square;
		float dollar_capex_per_kw;
		float dollar_opex_per_kW_per_year;
		float dollar_opex_per_GJ;
		float storage_max_GJ = 0;
		float storage_efficiency = 1;
		float facility_life_time_y;

		if (building_type == BuildingType.wind_farm)
		{
			// Based off https://en.wikipedia.org/wiki/Wildorado_Wind_Ranch
			// 6.5km x 10km, 70 turbines, 2.3 MW each, 161 MW total, 2.5 W m-2
			// Per 1 GW:
			// 1,000,000,000 / 2.5 W m-2 == 400,000,000 m2
			// 400,000,000 m2 / (1,000,000 m2 km-2) == 400 km2
			// sqrt(400 km2) == 20 km
			// Per 1 GW, that's 20 x 20 km,
			// In a square, for N GW, one side of the square is 20 km * sqrt(N)
			// e.g. per 2 GW is 28.3 km^2
			var area_m2 = watts / DataManager.instance.root_data.data.wind_farms.nameplate_watts_per_square_meter;
			var area_km2 = area_m2 / 1e6f;
			km_of_square = Mathf.Sqrt(area_km2);

			// onshore
			// ref: Lazards v13 page 17
			dollar_capex_per_kw = 1100;
			dollar_opex_per_kW_per_year = 32;
			dollar_opex_per_GJ = 0;
			// offshore
			// ref: Lazards v13 page 17
			dollar_capex_per_kw = 2350;
			dollar_opex_per_kW_per_year = 95;
			dollar_opex_per_GJ = 0;

			facility_life_time_y = 20;
		}
		else if (building_type == BuildingType.solar_panels)
		{
			// Based off http://www.solardesigntool.com/components/module-panel-solar/Trina-Solar/2935/TSM-315PD14/specification-data-sheet.html
			// Installed at: https://www.openstreetmap.org/edit?relation=6804128#map=16/32.7227/-101.9314
			// Regarding: https://www.bnbrenewables.com/lamesa-solar
			//
			// Each panel is 162 W m^-2 but about 2/3 of space is empty TODO: more exactly data needed
			// If we want 1GW, that's 1GW/(162/3) = 18,518,518 so let's call it 4.3km x 4.3km
			// so 2Gw is 6.1 km^2
			km_of_square = 4.3f * Mathf.Sqrt(giga_watts);

			// ref: Lazards v13 page 15
			dollar_capex_per_kw = 900;
			// ref: Lazards v13 page 16
			dollar_opex_per_kW_per_year = 10.5f;
			dollar_opex_per_GJ = 0;

			facility_life_time_y = 30;
		}
		else if (building_type == BuildingType.thermal_combustion)
		{
			km_of_square = 1;

			if (combustible_power_sources.Count != 1)
			{
				Debug.LogError("Only supports 1 combustible power sources.  Not mixed fuel at present.  Got " + combustible_power_sources.Count + " combustible power sources");
			}
			var combustible_power_source = combustible_power_sources[0];

			if (combustible_power_source == CombustiblePowerSource.coal)
			{
				// ref: Lazards v13 page 15
				dollar_capex_per_kw = 2975;
				// ref: Lazards v13 page 18
				dollar_opex_per_kW_per_year = 60;
				// ref: Lazards v13 page 18
				// ~3.9 $ kWh-1 / (3.6 GJ kWh-1) == 1.08 $ GJ-1
				dollar_opex_per_GJ = 1.08f;

				facility_life_time_y = 40;
			}
			else if (combustible_power_source == CombustiblePowerSource.gas)
			{
				// ref: Lazards v13 page 15
				dollar_capex_per_kw = (650f + 1200f + 50f + 100f) / 2; // == 1000
				// ref: Lazards v13 page 18
				dollar_opex_per_kW_per_year = (11f + 13.5f) / 2f;

				// ref: Lazards v13 page 18
				var variable_OM_dollar_per_Mwh = (3.00f + 3.75f) / 2;
				var variable_OM_dollar_per_GJ = variable_OM_dollar_per_Mwh / DataConversionsManager.GJ_per_MWh; // == 0.9375
				// ref: Lazards v13 page 18
				var heat_rate_Btu_per_kWh = (6133f + 6900f) / 2;
				var heat_rate_MBtu_per_kWh = heat_rate_Btu_per_kWh / 1e6f;
				var heat_rate_MBtu_per_GJ = heat_rate_MBtu_per_kWh / DataConversionsManager.GJ_per_kWh; // == 1.81
				// ref: Lazards v13 page 18
				var fuel_price_dollar_per_MBtu = 3.45f;
				var fuel_price_dollar_per_GJ = fuel_price_dollar_per_MBtu * heat_rate_MBtu_per_GJ; // == 6.24

				dollar_opex_per_GJ = variable_OM_dollar_per_GJ + fuel_price_dollar_per_GJ; // == 7.182

				facility_life_time_y = 20;
			}
			else if (combustible_power_source == CombustiblePowerSource.biomass)
			{
				// complete guess
				dollar_capex_per_kw = 1500;
				// complete guess
				dollar_opex_per_kW_per_year = 40;
				// complete guess
				dollar_opex_per_GJ = 10;

				facility_life_time_y = 30;
			}
			else
			{
				Debug.LogError("Unsupported combustible_power_source: " + combustible_power_source);
				return null;
			}

		}
		else if (building_type == BuildingType.nuclear_fission)
		{
			km_of_square = 1;
			// ref: Lazards v13 page 15
			dollar_capex_per_kw = 6900;
			// ref: Lazards v13 page 18
			dollar_opex_per_kW_per_year = 120f;
			// ref: Lazards v13 page 18
			// ~3.85 $ kWh-1 / (3.6 GJ kWh-1) == 1.07 $ GJ-1
			dollar_opex_per_GJ = 1.07f;

			facility_life_time_y = 40;
		}
		else if (building_type == BuildingType.pumped_hydro)
		{
			// rough measurement of: https://earth.google.com/web/@53.12973359,-4.07555578,861.1100934a,4558.85134d,35y,0h,0t,0r
			km_of_square = 3;
			/**
			 * ref: https://en.wikipedia.org/wiki/Dinorwig_Power_Station
			 * £425m in 1984 == £1368m in 2018 ref: http://inflation.iamkate.com/
			 * £1368m  ~= $1778m
			 * 1.8 GW == 1.8 million kW
			 * $1778m / 1.8 million kW == 988 $ kW-1
			 */
			dollar_capex_per_kw = 988;
			// complete guess
			dollar_opex_per_kW_per_year = 40f;
			// complete guess
			dollar_opex_per_GJ = 0;
			/**
			 * ref: https://en.wikipedia.org/wiki/Dinorwig_Power_Station
			 * 33 TJ == 33000 GJ
			 * 1.8 GW, scale it down to 1GW, this is a fairly abitrary decision but is at least consistent
			 * 33000/1.8 == 18300 GJ
			 */
			storage_max_GJ = 18300;
			// ref: https://en.wikipedia.org/wiki/Dinorwig_Power_Station
			storage_efficiency = 0.75f;

			facility_life_time_y = 100;
		}
		else if (building_type == BuildingType.tier1_energy_storage)
		{
			// ref: https://earth.google.com/web/@-33.08603552,138.51846393,465.95949837a,489.95391511d,35y,-1.96949283h,0.14607726t,0r
			km_of_square = 0.1f;
			/**
			 * ref: https://en.wikipedia.org/wiki/Hornsdale_Power_Reserve
			 * AUD $90 million ~= USD $60 million
			 * 100 MW == 0.1 million kW
			 * $60 million / 0.1 million kW == 600 $ kW-1
			 */
			dollar_capex_per_kw = 600;
			// complete guess
			dollar_opex_per_kW_per_year = 10f;
			// complete guess
			dollar_opex_per_GJ = 0;
			/**
			 * ref: https://en.wikipedia.org/wiki/Hornsdale_Power_Reserve
			 * 	129 MWh * 3600s h-1 == 464,400 MJ 
			 */
			storage_max_GJ = 464;
			/**
			 * ref: https://en.wikipedia.org/wiki/Lithium-ion_battery
			 * average of 0.8-0.9
			 */
			storage_efficiency = 0.85f;

			facility_life_time_y = 10;
		}
		else if (building_type == BuildingType.tier2_energy_storage)
		{
			// complete guess
			km_of_square = 2;
			// complete guess
			dollar_capex_per_kw = 1200;
			// complete guess - copied from gas turbine + 200% for electryolysers
			dollar_opex_per_kW_per_year = 12.5f * 3;
			// complete guess
			dollar_opex_per_GJ = 0;
			// complete guess
			storage_max_GJ = 40000;
			// complete guess
			storage_efficiency = 0.4f;

			facility_life_time_y = 100;
		}
		else
		{
			Debug.LogError("Unsupported potential building: " + building_type);
			return null;
		}

		LonLat half_square_dimensions_km = dimensions_1km_square.multiple(km_of_square * 0.5f);
		var lon_lat_min = lon_lat.minus(half_square_dimensions_km);
		var lon_lat_max = lon_lat.plus(half_square_dimensions_km);
		var outline = new List<LonLat>() { lon_lat_min, lon_lat_max };

		return new PowerPlantData(
			building_type: building_type,
			id: next_id++.ToString(),
			name: name,
			lon_lat,
			outline: outline,
			construction_start_date: date_time_value,
			generation_start_date: date_time_value,
			power_nameplate_mw: 1000 * giga_watts,
			operator_name: "",
			mdollar_capex: dollar_capex_per_kw * giga_watts,
			storage_max_GJ: storage_max_GJ * giga_watts,
			storage_charging_efficiency: storage_efficiency,
			combustible_power_sources: combustible_power_sources,
			mdollar_opex_per_year: dollar_opex_per_kW_per_year * giga_watts,
			dollar_opex_per_GJ: dollar_opex_per_GJ,
			facility_life_time_y: facility_life_time_y
		);
	}
}

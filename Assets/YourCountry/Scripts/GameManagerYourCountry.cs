using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum BuildingType
{
	none = 0,
	wind_farm = 1,
	solar_panels = 2,
	tier1_energy_storage = 3,
	tier2_energy_storage = 4,
	forest = 5,
	thermal_combustion = 6,
	nuclear_fission = 7,
	hydro = 8,
	pumped_hydro = 9,
}

/*
public class GameManagerYourCountry : MonoBehaviour
{
	public static GameManagerYourCountry singleton;

	#region Camera
	public MainCamera_YourCountry main_camera;
	public static float max_altitude = 210;
	public static float min_altitude = 5;

	public static void CameraAltitudeChangedInvoke (object obj, float altitude)
	{
		GameManagerYourCountry.CameraAltitudeChanged.Invoke(obj, altitude);
	}
	#endregion

	private static void handle_datetime_change (object obj, float _)
	{
		update_power_demand();
		update_power_supply();
	}

	#region Building new buildings

	public static void set_build_active (BuildingType building_type)
	{
		BuildingActiveChanged.Invoke(-1, building_type);
	}
	public static void set_build_inactive()
	{
		BuildingActiveChanged.Invoke(-1, default);
	}

	public static void add_power_plant(PowerPlantData new_power_plant_data)
	{
		power_plants_data.Add(new_power_plant_data);
		update_power_supply();
		money -= new_power_plant_data.dollar_capex;
	}

	#endregion

	#region Existing buildings
	public static List<PowerPlantData> power_plants_data = new List<PowerPlantData>();
	#endregion

	#region Power
	public static PowerDemandDataAggregateProvider power_demand_data = new PowerDemandTexasPeterDavies2017();

	private static float _power_generation_level = 0;
	public static float power_generation_level { get { return _power_generation_level; } internal set {
			if (_power_generation_level != value)
			{
				_power_generation_level = value;
				GridPowerChanged.Invoke(-1, grid_power_status());
			}
		} }

	private static float _power_demand_level = 0;
	public static float power_demand_level { get { return _power_demand_level; } internal set {
			if (_power_demand_level != value)
			{
				_power_demand_level = value;
				GridPowerChanged.Invoke(-1, grid_power_status());
			}
		} }

	public static GridPowerStatus grid_power_status () { return power_generation_level >= power_demand_level ? GridPowerStatus.is_okay : GridPowerStatus.is_low; }
	public static bool power_is_charging () { return grid_power_status() == GridPowerStatus.is_charging; }
	public static bool power_is_okay() { return grid_power_status() == GridPowerStatus.is_okay; }
	public static bool power_is_low() { return grid_power_status() == GridPowerStatus.is_low; }

	private static void update_power_demand ()
	{
		//DateTime end_datetime = game_datetime.AddMonths(3).AddSeconds(-1);
		//power_demand_level = power_demand_data.demand_GW_average(GameManagerYourCountry.game_datetime, end_datetime);
	}

	private static void update_power_supply ()
	{
		power_generation_level = power_plants_data.Aggregate(0f, (accum, p) => accum + (p.power_max_mw / 1000f));
	}
	#endregion

	#region Money
	private static double _money;
	private static double money
	{
		get { return _money; }
		set { _money = value; MoneyChanged.Invoke(-1, new MoneyChange(_money)); }
	}

	#endregion

	#region EventHandlers
	public static event EventHandler<float> CameraAltitudeChanged = delegate { };
	public static event EventHandler<float> GameDatetimeChanged = delegate { };
	public static event EventHandler<GameDatetimeStep> GameDatetimeStepChanged = delegate { };
	public static event EventHandler<BuildingType> BuildingActiveChanged = delegate { };
	public static event EventHandler<GridPowerStatus> GridPowerChanged = delegate { };
	public static event EventHandler<MoneyChange> MoneyChanged = delegate { };
	#endregion

	// Start is called before the first frame update
	void Awake ()
	{
		singleton = this;
		GameDatetimeChanged += handle_datetime_change;
	}

	private void Start ()
	{
		
		money = 0;

		// Add buildings appropriate for region and game datetime
		Power_Plants_In_USA_Texas
		//PowerPlantsTexasDemo
			.power_plants
			.ForEach(data =>
			{
				add_power_plant(data);	
				PowerPlant.Init(data);
			});
	}

	public Vector3 get_position_of_mouse()
	{
		var vec = main_camera.get_position_of_mouse();
		// TODO, use mesh collider on terrain when we get elevation
		vec.y = 0;
		return vec;
	}
}
*/

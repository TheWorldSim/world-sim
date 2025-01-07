using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PowerManager : MonoBehaviour
{
	public delegate void ChangedGridPower();
	public event ChangedGridPower on_changed_grid_power;

	// check power demand data from ERCOT, ELEXON, etc does not include demand from storage
	public float demand_base_GW { get; private set; }
	public float demand_stored_GW { get; private set; }
	public float generation_max_from_renewables_GW { get; private set; }
	public float generation_from_storage_GW { get; private set; }
	public float generation_from_nuclear_GW { get; private set; }
	public float generation_from_fossil_GW { get; private set; }
	public float generation_base_GW { get {
			return generation_max_from_renewables_GW
			+ generation_from_nuclear_GW
			+ generation_from_fossil_GW;
		} }
	public float generation_GW { get {
			return generation_base_GW
			+ generation_from_storage_GW;
		} }

	public float stored_GJ { get; private set; }
	public float max_storage_GJ { get; private set; }

	private HistoryNumerical demand_base_GJ_fulfilled_by_date_time = new HistoryNumerical();
	public float total_demand_base_fulfilled_GJ { get {
			return demand_base_GJ_fulfilled_by_date_time.total_value;
			//return demand_base_GJ_fulfilled_by_date_time.history.Aggregate(0f, (accum, p) => accum + p.value);
			//return power_plants.Aggregate(0f, (accum, pp) => accum + pp.total_generated_GJ);
		} }
	public float total_demand_base_fulfilled_GJ_per_year { get {
			var years = demand_base_GJ_fulfilled_by_date_time.total_time_years();
			if (years == 0) return 0;
			return total_demand_base_fulfilled_GJ / years;
		} }

	public float total_cost_mdollar_per_year { get {
			return power_plants.Aggregate(0f, (accum, pp) => accum + pp.cost_mdollar_per_year());
		} }

	private PowerDemandDataAggregateProvider power_demand_data;
	private RegionOfInterest selected_region;
	private WorldSimRegionData region_data_onshore;
	private WorldSimRegionData region_data_offshore;
	private CellsInfoManager onshore_wind_capacity_factor_manager;
	private CellsInfoManager offshore_wind_capacity_factor_manager;
	private CellsInfoManager solarpv_capacity_factor_manager;

	private List<PowerPlantOperating> power_plants = new List<PowerPlantOperating>();

	public bool is_charging() { return demand_stored_GW >= 0; }
	public bool is_discharging() { return generation_from_storage_GW > 0; }
	// hack to deal with floating point error
	public bool is_low() { return generation_GW < (demand_base_GW - 0.00003f); }


	private void Awake()
	{
		demand_base_GW = 0;
		demand_stored_GW = 0;
		generation_max_from_renewables_GW = 0;
		generation_from_storage_GW = 0;
		generation_from_nuclear_GW = 0;
		generation_from_fossil_GW = 0;

		stored_GJ = 0;
		max_storage_GJ = 0;
	}

	private void Start()
	{
		DataManager.instance.on_changed_region_data_fetch_progress += handle_region_data_fetch_progress;
		GameManagerWorldSim.instance.on_changed_selected_region += handle_changed_selected_region;
		GameManagerWorldSim.date_time.on_changed_sim_date_time += handle_changed_sim_date_time;
		GameManagerWorldSim.levels.on_changed_level_active += handle_changed_level_active;
		NewBuildingManager.instance.on_changed_buildings += handle_changed_buildings;
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest roi, object source)
	{
		selected_region = roi;
		update_region_and_level_specific_data();

		if (roi.country_name == RegionOfInterest.united_kingdom)
		{
			power_demand_data = new PowerDemandUK2018();
		}
		else if (roi.province_name == RegionOfInterest.texas)
		{
			power_demand_data = new PowerDemandTexas2016To2018InclusiveHack();
		}
	}

	private void handle_region_data_fetch_progress(MultipleFetchersProgress progress)
	{
		update_region_and_level_specific_data();
	}

	private void update_region_and_level_specific_data()
	{
		if (selected_region == null) return;

		var region_data_ready = DataManager.instance.region_data_ready(selected_region);
		if (!region_data_ready) return;

		var level = GameManagerWorldSim.levels.get_level_config();
		var data_time_window = level.data_time_window;
		region_data_onshore = DataManager.instance.region_data(selected_region.clone(offshore: false));
		region_data_offshore = DataManager.instance.region_data(selected_region.clone(offshore: true));
		onshore_wind_capacity_factor_manager = CellsInfoManager.init_wind_capacity_factor(region_data_onshore, data_time_window);
		offshore_wind_capacity_factor_manager = CellsInfoManager.init_wind_capacity_factor(region_data_offshore, data_time_window);
		solarpv_capacity_factor_manager = CellsInfoManager.init_solarpv_capacity_factor(region_data_onshore, data_time_window);
	}

	private void handle_changed_sim_date_time(ChangedDateTime change)
	{
		update_demand_gw(change.new_date_time);
		update_generation_gw(change);
		update_stored_GJ();
		update_demand_base_GJ_fulfilled(change);
		on_changed_grid_power?.Invoke();
	}

	private void update_demand_gw(DateTime new_date_time)
	{
		if (selected_region == null) return;
		demand_base_GW = power_demand_data.demand_GW(new_date_time);
	}

	private bool already_low_power_debug = false;
	private void update_generation_gw(ChangedDateTime change)
	{
		var have_data_needed_to_calculate_generation_GW = (
			onshore_wind_capacity_factor_manager != null
			&& solarpv_capacity_factor_manager != null
		);
		if (!have_data_needed_to_calculate_generation_GW) return;

		generation_max_from_renewables_GW = generate_from_renewables(change);

		generation_from_storage_GW = 0;
		generation_from_nuclear_GW = 0;
		generation_from_fossil_GW = 0;

		if (generation_max_from_renewables_GW >= demand_base_GW)
		{
			var GW_to_store = generation_max_from_renewables_GW - demand_base_GW;
			demand_stored_GW = store_power(change, GW_to_store);
		}
		else
		{
			demand_stored_GW = 0;

			var GW_needed_from_store = demand_base_GW - generation_max_from_renewables_GW;
			generation_from_storage_GW = use_stored_power(
				change,
				GW_needed_from_store);

			var GW_needed_from_nuclear = demand_base_GW - generation_GW;
			generation_from_nuclear_GW = generate_from_nuclear(
				change,
				GW_needed_from_nuclear);

			var GW_needed_from_fossil = demand_base_GW - generation_GW;
			generation_from_fossil_GW = generate_from_fossil(
				change,
				GW_needed_from_fossil);
		}

		if (is_low())
		{
			if (!already_low_power_debug)
			{
				var precision = "0.00000000000000000";
				Debug.Log("Low power." +
					"\n  Demand from base:    " + demand_base_GW.ToString(precision) +
					"\n              storage: " + demand_stored_GW.ToString(precision) +
					"\n  Generation " + generation_GW.ToString(precision) +
					"\n             from renewables_GW (max): " + generation_max_from_renewables_GW.ToString(precision) +
					"\n             storage_GW:               " + generation_from_storage_GW.ToString(precision) +
					"\n             nuclear:                  " + generation_from_nuclear_GW.ToString(precision) +
					"\n             fossil:                   " + generation_from_fossil_GW.ToString(precision));
				already_low_power_debug = true;
			}
		}
		else already_low_power_debug = false;
	}

	private float generate_from_renewables(ChangedDateTime change)
	{
		float max_from_renewables_GW = 0;

		power_plants.Where(p => p.data.is_renewable)
			.ToList()
			.ForEach(power_plant =>
			{
				var power_max_variable_GW = power_plant.generate_max_variable_GW(
					change,
					offshore_wind_capacity_factor_manager,
					onshore_wind_capacity_factor_manager,
					solarpv_capacity_factor_manager
				);
				max_from_renewables_GW += power_max_variable_GW;
			});

		return max_from_renewables_GW;
	}

	private float store_power(ChangedDateTime change, float GW_to_store)
	{
		if (GW_to_store < 0) throw new Exception("GW_to_store must be >= 0 but was " + GW_to_store);

		float total_stored_GW = 0;

		power_plants.Where(p => p.data.is_storage)
			// most efficient should store first.  TODO: Check this is reasonable
			.OrderByDescending(p => p.data.storage_efficiency) 
			.ToList()
			.ForEach(power_plant =>
		{
			var stored_GW = power_plant.store(change, GW_to_store);
			GW_to_store -= stored_GW;
			total_stored_GW += stored_GW;
		});

		return total_stored_GW;
	}

	private float use_stored_power(ChangedDateTime change, float GW_needed_from_store)
	{
		var plants = power_plants.Where(p => p.data.is_storage)
			// most efficient should supply first.  TODO: Check this is reasonable
			.OrderByDescending(p => p.data.storage_efficiency)
			.ToList();
		return generate(plants, change, GW_needed_from_store);
	}

	private float generate_from_nuclear(ChangedDateTime change, float GW_needed_from_nuclear)
	{
		var plants = power_plants.Where(p => p.data.is_nuclear).ToList();
		// Does not factor in ramp up and down times
		return generate(plants, change, GW_needed_from_nuclear);
	}

	private float generate_from_fossil(ChangedDateTime change, float GW_needed_from_fossil)
	{
		var plants = power_plants.Where(p => p.data.is_fossil).ToList();
		// Does not factor in ramp up and down times (particularly for coal)
		return generate(plants, change, GW_needed_from_fossil);
	}

	private float generate(List<PowerPlantOperating> plants, ChangedDateTime change, float GW_needed)
	{
		float generated_GW = 0;

		plants.ForEach(power_plant =>
			{
				var single_generator_GW = power_plant.generate_GW(change, GW_needed);
				GW_needed -= single_generator_GW;
				generated_GW += single_generator_GW;
			});

		return generated_GW;
	}

	private void update_stored_GJ()
	{
		max_storage_GJ = 0;
		stored_GJ = 0;
		
		power_plants.Where(p => p.data.is_storage)
			.ToList()
			.ForEach(power_plant =>
			{
				max_storage_GJ += power_plant.data.storage_max_GJ;
				stored_GJ += power_plant.storage_current_GJ;
			});
	}

	private void update_demand_base_GJ_fulfilled(ChangedDateTime change)
	{
		var demand_base_GJ = demand_base_GW * change.elapsed_time_s;
		var generation_GJ = generation_GW * change.elapsed_time_s;
		var GJ_fulfilled = Math.Min(demand_base_GJ, generation_GJ);
		var data_point = new HistoryDataPoint<float>(change, GJ_fulfilled);
		demand_base_GJ_fulfilled_by_date_time.update(change, data_point);
	}

	private void handle_changed_buildings(PowerPlantData power_plant_data)
	{
		var ws_cell = get_ws_cell_for_power_plant(power_plant_data);
		power_plants.Add(new PowerPlantOperating(power_plant_data, ws_cell));
	}

	private WSCell get_ws_cell_for_power_plant(PowerPlantData power_plant_data)
	{
		var latlon = power_plant_data.lon_lat.to_vec2();
		var ws_cell = WorldMapGlobeWrapper.latlon_to_ws_cell(latlon, region_data_onshore);
		if (ws_cell == null) ws_cell = WorldMapGlobeWrapper.latlon_to_ws_cell(latlon, region_data_offshore);
		if (ws_cell == null) throw new Exception("Can not find ws_cell for power_plant_data " + latlon.ToString());

		return ws_cell;
	}

	private void handle_changed_level_active(bool level_active, LevelConfig level_config)
	{
		if (level_active)
		{
			update_region_and_level_specific_data();
			power_plants.ForEach(power_plant =>
			{
				power_plant.clear_GJ_history();
				power_plant.clear_stored_GJ();
			});
		}
	}
}

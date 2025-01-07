using System;
using System.Collections.Generic;
using UnityEngine;


public class GJByTime : HistoryDataPoint<float>
{
	public float GJ { get { return value; } }

	public GJByTime(ChangedDateTime change, float GJ): base(change, GJ) {}

	public override string ToString()
	{
		return change.ToString() + "   GJ: " + GJ;
	}
}


public class PowerPlantOperating
{
	public PowerPlantData data { get; private set; }
	private WSCell ws_cell;
	private HistoryNumerical generated_GJ_by_date_time = new HistoryNumerical();
	public float storage_current_GJ { get; private set; }
	public float total_generated_GJ { get; private set; }

	public float cost_mdollar_per_year()
	{
		var c = amortised_capex_mdollar_per_year;
		var ov = opex_variable_mdollar_per_year();
		var of = data.opex_mdollar_per_year;
		return c + of + ov;
	}
	private float amortised_capex_mdollar_per_year;
	private float opex_variable_mdollar_per_year()
	{
		var years = total_time_years();
		if (years == 0) return 0;
		var generated_GJ_per_year = total_generated_GJ / years;
		return (data.dollar_opex_per_GJ * generated_GJ_per_year) / 1e6f;
	}

	private float total_time_years()
	{
		var total_s = generated_GJ_by_date_time.total_time_s();
		return total_s / DataConversionsManager.s_per_year;
	}

	public PowerPlantOperating(PowerPlantData data, WSCell ws_cell)
	{
		this.data = data;
		storage_current_GJ = 0;
		this.ws_cell = ws_cell;
		// Simple (too simple?) amoritisation
		amortised_capex_mdollar_per_year = data.capex_mdollar / data.facility_life_time_y;
	}

	public float generate_max_variable_GW(ChangedDateTime change, CellsInfoManager offshore_wind_capacity_factor_manager, CellsInfoManager onshore_wind_capacity_factor_manager, CellsInfoManager solarpv_capacity_factor_manager)
	{
		CellsInfoManager cells_info_manager;

		if (data.building_type == BuildingType.wind_farm)
		{
			if (offshore_wind_capacity_factor_manager.supports_cell(ws_cell))
			{
				cells_info_manager = offshore_wind_capacity_factor_manager;
			}
			else if (onshore_wind_capacity_factor_manager.supports_cell(ws_cell))
			{
				cells_info_manager = onshore_wind_capacity_factor_manager;
			}
			else throw new Exception("Neither onshore nor offshore capacity factor manager supports ws_cell " + ws_cell.latlon.ToString());
		}
		else if (data.building_type == BuildingType.solar_panels)
		{
			cells_info_manager = solarpv_capacity_factor_manager;
		}
		else throw new Exception("unsupported power plant type " + data.building_type);

		var accessor = cells_info_manager.get_date_time_accessor(change.new_date_time);
		var capacity_factor = accessor.get_capacity_factor(ws_cell);

		var max_GW = data.generation_nameplate_GW * capacity_factor;

		update_generated_GJ(change, max_GW);

		return max_GW;
	}

	public float generate_GW(ChangedDateTime change, float GW_needed)
	{

		// Due to floating point error (confirm this) GW_needed can some times
		// be negative, like -9.536743E-07
		GW_needed = Math.Max(0, GW_needed);

		float generated_GW;

		if (data.is_renewable)
		{
			throw new Exception("Can not use generate_gw with renewables, must use generate_max_variable_gw");
		}
		else if (data.is_storage)
		{
			generated_GW = generate_from_store(change, GW_needed);
		}
		else
		{
			generated_GW = Mathf.Min(GW_needed, data.generation_nameplate_GW);
		}

		update_generated_GJ(change, generated_GW);

		return generated_GW;
	}

	public float store(ChangedDateTime change, float available_GW)
	{
		// TODO make storage_nameplate_GW a seperate parameter from generation_nameplate_GW
		var max_store_GW = Mathf.Min(available_GW, data.generation_nameplate_GW);

		var available_GJ = max_store_GW * change.elapsed_time_s;
		var possible_storage_GJ = (data.storage_max_GJ - storage_current_GJ) / data.storage_charging_efficiency;
		var GJ_to_store = Mathf.Min(possible_storage_GJ, available_GJ);

		storage_current_GJ += (GJ_to_store * data.storage_charging_efficiency);

		var store_GW = GJ_to_store / change.elapsed_time_s;
		return store_GW;
	}

	private float generate_from_store(ChangedDateTime change, float GW_needed)
	{
		var max_supply_GW = Mathf.Min(GW_needed, data.generation_nameplate_GW);

		var needed_GJ = max_supply_GW * change.elapsed_time_s;
		var possible_stored_GJ = storage_current_GJ * data.storage_discharging_efficiency;
		var GJ_to_supply = Mathf.Min(possible_stored_GJ, needed_GJ);

		storage_current_GJ -= (GJ_to_supply / data.storage_discharging_efficiency);

		var supply_GW = GJ_to_supply / change.elapsed_time_s;
		return supply_GW;
	}

	public void update_generated_GJ(ChangedDateTime change, float generated_GW)
	{
		var data_point = new GJByTime(change, generated_GW);
		generated_GJ_by_date_time.update(change, data_point);
	}

	private float sum_GJ(int start_index, int count)
	{
		var total_GJ = 0f;
		for (int i = start_index; i < (start_index + count); ++i)
		{
			total_GJ += generated_GJ_by_date_time.history[i].value;
		}
		return total_GJ;
	}

	public void clear_GJ_history()
	{
		generated_GJ_by_date_time.clear();
	}

	public void clear_stored_GJ()
	{
		storage_current_GJ = 0;
	}


	public static class TestPowerPlantOperating
	{
		private static PowerPlantData data;
		private static WSCell ws_cell;
		private static DateTime dt1 = DateTime.Parse("2020-02-10 18:51");
		private static DateTime dt2 = DateTime.Parse("2020-02-10 18:52");
		private static DateTime dt3 = DateTime.Parse("2020-02-10 18:53");
		private static DateTime dt4 = DateTime.Parse("2020-02-10 18:54");
		private static DateTime dt5 = DateTime.Parse("2020-02-10 18:55");
		private static DateTime dt6 = DateTime.Parse("2020-02-10 18:56");
		private static DateTime dt7 = DateTime.Parse("2020-02-10 18:57");

		public static void test()
		{
			data = TestPowerPlantData.generate_power_plant_data();
			ws_cell = TestWSCell.generate_ws_cell();

			no_overlapping_changed_date_time();
			overlapping_changed_date_time1();
			overlapping_beginning_changed_date_time1();
			overlapping_beginning_changed_date_time2();
			overlapping_end_changed_date_time();
			overlapping_middle_changed_date_time1();
			overlapping_middle_changed_date_time2();

			performance_test();
		}

		public static void assert(bool condition, string test_description, string actual)
		{
			if (!condition) Debug.LogError(test_description + " failed. actual: " + actual);
		}

		public static void assert(PowerPlantOperating pp, int expected_count, string test_description)
		{
			assert(pp.generated_GJ_by_date_time.history.Count == expected_count, test_description, pp.generated_GJ_by_date_time.history.Count.ToString());
		}

		private static void no_overlapping_changed_date_time()
		{
			var pp = new PowerPlantOperating(data, ws_cell);

			pp.update_generated_GJ(new ChangedDateTime(dt3, dt4), 1);
			assert(pp, 1, "Should have 1");

			pp.update_generated_GJ(new ChangedDateTime(dt1, dt2), 1);
			assert(pp, 2, "Should have 2");

			pp.update_generated_GJ(new ChangedDateTime(dt2, dt3), 1);
			assert(pp, 3, "Should have 3");
		}
		private static void overlapping_changed_date_time1()
		{
			var pp = new PowerPlantOperating(data, ws_cell);
			pp.update_generated_GJ(new ChangedDateTime(dt2, dt4), 1);
			assert(pp, 1, "Should have 1");

			pp.update_generated_GJ(new ChangedDateTime(dt2, dt4), 1);
			assert(pp, 1, "Should still have 1");
		}

		private static void overlapping_beginning_changed_date_time1()
		{
			var pp = new PowerPlantOperating(data, ws_cell);
			pp.update_generated_GJ(new ChangedDateTime(dt2, dt4), 1);
			assert(pp, 1, "Should have 1");

			pp.update_generated_GJ(new ChangedDateTime(dt1, dt2), 1);
			assert(pp, 2, "Should have 2");

			pp.update_generated_GJ(new ChangedDateTime(dt2, dt3), 1);
			assert(pp, 2, "Should still have 2");
		}

		private static void overlapping_beginning_changed_date_time2()
		{
			var pp = new PowerPlantOperating(data, ws_cell);

			pp.update_generated_GJ(new ChangedDateTime(dt2, dt4), 1);
			assert(pp, 1, "Should have 1");

			pp.update_generated_GJ(new ChangedDateTime(dt1, dt3), 1);
			assert(pp, 1, "Should still have 1");
		}

		private static void overlapping_end_changed_date_time()
		{
			var pp = new PowerPlantOperating(data, ws_cell);
			pp.update_generated_GJ(new ChangedDateTime(dt1, dt2), 1);
			assert(pp, 1, "Should have 1");

			pp.update_generated_GJ(new ChangedDateTime(dt2, dt3), 1);
			assert(pp, 2, "Should have 2");
		}

		private static void overlapping_middle_changed_date_time1()
		{
			var pp = new PowerPlantOperating(data, ws_cell);

			pp.update_generated_GJ(new ChangedDateTime(dt2, dt5), 1);
			assert(pp, 1, "Should have 1");

			pp.update_generated_GJ(new ChangedDateTime(dt3, dt4), 1);
			assert(pp, 1, "Should still have 1");
		}

		private static void overlapping_middle_changed_date_time2()
		{
			var pp = new PowerPlantOperating(data, ws_cell);

			pp.update_generated_GJ(new ChangedDateTime(dt1, dt3), 1);
			assert(pp, 1, "Should have 1");

			pp.update_generated_GJ(new ChangedDateTime(dt3, dt5), 1);
			assert(pp, 2, "Should have 2");

			pp.update_generated_GJ(new ChangedDateTime(dt5, dt7), 1);
			assert(pp, 3, "Should have 3");

			pp.update_generated_GJ(new ChangedDateTime(dt2, dt6), 1);
			assert(pp, 1, "Should have 1");
		}

		private static void performance_test()
		{
			var pps = new List<PowerPlantOperating>();

			for (int i = 0; i < 10; i++)
			{
				var pp = new PowerPlantOperating(data, ws_cell);
				pps.Add(pp);
			}

			var total_date_time_changes = 600; // 365 * 24 * 50;
			var dt_start = DateTime.Parse("2020-01-01 00:00:00");

			var start = DateTime.UtcNow;

			pps.ForEach(pp =>
			{
				for (int i = 0; i < total_date_time_changes; i++)
				{
					// AddSeconds but we could have chosen Add Hours, or Days,
					// etc.  Just need to change it a bit
					var dt_previous = dt_start.AddSeconds(i);
					var dt_next = dt_start.AddSeconds(i + 1);
					pp.update_generated_GJ(new ChangedDateTime(dt_previous, dt_next), 1);
				}
			});

			var end = DateTime.UtcNow;
			Debug.Log("Took " + (end - start).TotalSeconds);
			var start2 = DateTime.UtcNow;

			pps.ForEach(pp =>
			{
				for (int i = 0; i < total_date_time_changes; i++)
				{
					// AddSeconds but we could have chosen Add Hours, or Days,
					// etc.  Just need to change it a bit
					var dt_previous = dt_start.AddSeconds(i);
					var dt_next = dt_start.AddSeconds(i + 1);
					pp.update_generated_GJ(new ChangedDateTime(dt_previous, dt_next), 1);
				}
			});

			var end2 = DateTime.UtcNow;
			Debug.Log("Took " + (end2 - start2).TotalSeconds.ToString("0.00000000000"));
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;


public class CellsInfoManager
{
	public static CellsInfoManager init_wind_capacity_factor(WorldSimRegionData region_data, DataTimeWindow data_time_window)
	{
		CapacityFactorData capacity_factors;

		if (data_time_window == DataTimeWindow.year_average)
		{
			capacity_factors = region_data.wind_capacity_factor_year_average;
		}
		else if (data_time_window == DataTimeWindow.month_hourly_average)
		{
			capacity_factors = region_data.wind_capacity_factor_month_hourly_average;
		}
		else if (data_time_window == DataTimeWindow.hourly)
		{
			capacity_factors = region_data.wind_capacity_factor_hourly;
		}
		else throw new Exception("Unsupported data_time_window " + data_time_window);

		return new CellsInfoManager(capacity_factors, data_time_window);
	}

	public static CellsInfoManager init_solarpv_capacity_factor(WorldSimRegionData region_data, DataTimeWindow data_time_window)
	{
		CapacityFactorData capacity_factors;

		if (data_time_window == DataTimeWindow.year_average)
		{
			capacity_factors = region_data.solarpv_capacity_factor_year_average;
		}
		else if (data_time_window == DataTimeWindow.month_hourly_average)
		{
			capacity_factors = region_data.solarpv_capacity_factor_month_hourly_average;
		}
		else if (data_time_window == DataTimeWindow.hourly)
		{
			capacity_factors = region_data.solarpv_capacity_factor_hourly;
		}
		else throw new Exception("Unsupported data_time_window " + data_time_window);

		return new CellsInfoManager(capacity_factors, data_time_window);
	}

	private CapacityFactorData capacity_factors;
	private DataTimeWindow data_time_window;
	private CellsInfoManager(CapacityFactorData capacity_factors, DataTimeWindow data_time_window)
	{
		this.capacity_factors = capacity_factors;
		this.data_time_window = data_time_window;
	}

	public bool supports_cell(WSCell cell) => capacity_factors.cell_latlon_to_index.ContainsKey(cell.latlon);

	public CapacityFactorAccessor get_date_time_accessor(DateTime date_time)
	{
		var start_date_time = get_start_date_time(date_time);
		var end_date_time = get_end_date_time(date_time); // There will be a bug here.  Need to bound this to end of year to prevent bug
		var date_time_t = get_date_time_t(date_time, start_date_time);

		return new CapacityFactorAccessor(capacity_factors, start_date_time, end_date_time, date_time_t);
	}

	private DateTime get_coerced_date_time(DateTime date_time, int add, bool rounded)
	{
		var minute = rounded ? 0 : date_time.Minute;
		var second = rounded ? 0 : date_time.Second;

		if (data_time_window == DataTimeWindow.year_average)
		{
			date_time = new DateTime(date_time.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		}
		else if (data_time_window == DataTimeWindow.month_hourly_average)
		{
			var hour = (date_time.Hour + add) % 24;
			date_time = new DateTime(date_time.Year, date_time.Month, 1, hour, minute, second, DateTimeKind.Utc);
		}
		else if (data_time_window == DataTimeWindow.hourly)
		{
			date_time = new DateTime(date_time.Year, date_time.Month, date_time.Day, date_time.Hour, minute, second, DateTimeKind.Utc);
			date_time = date_time.AddHours(add);
		}

		return date_time;
	}

	private DateTime get_bucketed_date_time(DateTime date_time) => get_coerced_date_time(date_time, 0, false);
	private DateTime get_start_date_time(DateTime date_time) => get_coerced_date_time(date_time, 0, true);
	private DateTime get_end_date_time(DateTime date_time) => get_coerced_date_time(date_time, 1, true);

	private float get_date_time_t(DateTime date_time, DateTime start_date_time)
	{
		var bucketed_date_time = get_bucketed_date_time(date_time);
		float diff_start = (float)(bucketed_date_time - start_date_time).TotalSeconds;
		float total = (data_time_window == DataTimeWindow.year_average) ? 1 : 3600; // 3600 for month_hourly_average and hourly
		return diff_start / total;
	}
}


// Specific for a date time and region of cells
public class CapacityFactorAccessor
{
	CapacityFactorData capacity_factors;
	private List<float> start_date_time_specific_capacity_factors;
	private List<float> end_date_time_specific_capacity_factors;
	private float date_time_t;

	public CapacityFactorAccessor(CapacityFactorData capacity_factors, DateTime start_date_time, DateTime end_date_time, float date_time_t)
	{
		this.capacity_factors = capacity_factors;
		this.start_date_time_specific_capacity_factors = capacity_factors.get_capacities_by_datetime(start_date_time);
		this.end_date_time_specific_capacity_factors = capacity_factors.get_capacities_by_datetime(end_date_time);
		this.date_time_t = date_time_t;
	}

	public float get_capacity_factor(WSCell cell)
	{
		var index = capacity_factors.cell_latlon_to_index[cell.latlon];
		var start_capacity_factor = start_date_time_specific_capacity_factors[index];
		var end_capacity_factor = end_date_time_specific_capacity_factors[index];
		var capacity_factor = Mathf.Lerp(start_capacity_factor, end_capacity_factor, date_time_t);
		return capacity_factor;
	}
}

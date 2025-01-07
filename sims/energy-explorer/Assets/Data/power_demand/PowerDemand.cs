using System;
using UnityEngine;

public class SeriesStats
{
	public float average;
	public float min;
	public float max;

	public SeriesStats()
	{ }

	public SeriesStats(float[] data, int from_index, int to_index)
	{
		float total = 0;
		float min = float.MaxValue;
		float max = float.MinValue;

		for (int i = from_index; i < to_index; ++i)
		{
			var d = data[i];
			total += d;
			min = Mathf.Min(min, d);
			max = Mathf.Min(max, d);
		}

		average = total / (to_index - from_index);
		this.min = min;
		this.max = max;
	}
}

public interface PowerDemandDataAggregateProvider
{
	DateTime start_datetime();
	float demand_GW (DateTime datetime);
	float demand_GW_average (DateTime from_inclusive_datetime, DateTime to_exclusive_datetime);
}

interface PowerDemandDataContainer
{
	DateTime start_datetime();
	float time_step_ms ();
	float[] data();
	bool error_when_out_of_range();
}

public class PowerDemandTexas2010To2012Inclusive : PowerDemandDataAggregateProvider, PowerDemandDataContainer
{
	public float demand_GW (DateTime datetime) => PowerDemand.demand(datetime, this);

	public float demand_GW_average (DateTime from_inclusive_datetime, DateTime to_exclusive_datetime)
	{
		return PowerDemand.demand_average(from_inclusive_datetime, to_exclusive_datetime, this);
	}
	
	public virtual DateTime start_datetime () => DateTime.Parse("2010-01-01 06:00");

	public float time_step_ms () => 3600000; // 1 hour in milliseconds

	public float[] data () => PowerDemandTexasPeterDavies2017Data.demand_gw_by_hour;

	public bool error_when_out_of_range () => false;
}

// TODO: remove and replace with real data
public class PowerDemandTexas2016To2018InclusiveHack : PowerDemandTexas2010To2012Inclusive
{
	public override DateTime start_datetime() => DateTime.Parse("2016-01-01 06:00");
}

static class PowerDemand
{
	public static float demand (DateTime datetime, PowerDemandDataContainer data_container)
	{
		var index = datetime_to_index_float(datetime, data_container);
		var index_start = Mathf.FloorToInt(index);
		var index_end = Mathf.CeilToInt(index);

		var start = get_data_point(index_start, data_container);
		var end = get_data_point(index_end, data_container);

		var t = index - index_start;
		var value = Mathf.Lerp(start, end, t);

		return value;
	}

	public static float demand_average (DateTime from_datetime, DateTime to_datetime, PowerDemandDataContainer data_container)
	{
		var start_index = datetime_to_index(from_datetime, data_container);
		var end_index = datetime_to_index(to_datetime, data_container);
		return get_stats(start_index, end_index, data_container).average;
	}

	private static int datetime_to_index (DateTime datetime, PowerDemandDataContainer data_container)
	{
		var index = Mathf.RoundToInt(((float)(datetime - data_container.start_datetime()).TotalMilliseconds) / data_container.time_step_ms());
		if (!index_in_range(index, data_container))
		{
			if (data_container.error_when_out_of_range()) Debug.LogError("Index out of range: " + index);
			return -1;
		}
		return index;
	}

	private static float datetime_to_index_float(DateTime datetime, PowerDemandDataContainer data_container)
	{
		var index = ((float)(datetime - data_container.start_datetime()).TotalMilliseconds) / data_container.time_step_ms();
		return index;
	}

	private static bool index_in_range(int index, PowerDemandDataContainer data_container)
	{
		return index >= 0 && index < data_container.data().Length;
	}

	private static float get_data_point (int index, PowerDemandDataContainer data_container)
	{
		if (!index_in_range(index, data_container))
		{
			if (data_container.error_when_out_of_range()) Debug.LogError("Index out of range: " + index);
			return -1;
		}
		return data_container.data()[index];
	}

	/**
	 * to_index is exclusive
	 */
	private static SeriesStats get_stats (int from_index, int to_index, PowerDemandDataContainer data_container)
	{
		if (!index_in_range(from_index, data_container))
		{
			if (data_container.error_when_out_of_range()) Debug.LogError("from_index out of range: " + from_index);
			return new SeriesStats();
		}

		if (!index_in_range(to_index - 1, data_container))
		{
			if (data_container.error_when_out_of_range()) Debug.LogError("to_index out of range: " + to_index);
			return new SeriesStats();
		}

		return new SeriesStats(data_container.data(), from_index, to_index);
	}
}

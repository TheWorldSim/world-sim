using System;
using System.Collections.Generic;
using UnityEngine;


public class CapacityFactorData
{
	public Dictionary<Vector2, int> cell_latlon_to_index { get; private set; }
	private Dictionary<DateTime, int> datetime_to_index;
	private List<List<float>> date_time_latlon_capacity;

	public CapacityFactorData(Dictionary<Vector2, int> cell_latlon_to_index, Dictionary<DateTime, int> datetime_to_index, List<List<float>> date_time_latlon_capacity)
	{
		this.cell_latlon_to_index = cell_latlon_to_index;
		this.datetime_to_index = datetime_to_index;
		this.date_time_latlon_capacity = date_time_latlon_capacity;
	}

	public List<float> get_capacities_by_datetime(DateTime date_time)
	{
		if (!datetime_to_index.ContainsKey(date_time))
		{
			Debug.Log(" datetime " + date_time + " not in dictionary with keys: " + string.Join(", ", datetime_to_index.Keys));
		}
		var index = datetime_to_index[date_time];
		return date_time_latlon_capacity[index];
	}
}

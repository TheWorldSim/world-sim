using System;
using System.Collections.Generic;

public class HistoryNumerical : History<float>
{
	public float total_value { get; private set; }

	public HistoryNumerical() : base() {
		total_value = 0;
	}

	public void update(ChangedDateTime change, HistoryDataPoint<float> history_data_point)
	{
		var (start_index, count) = find_overlapping(change);
		var value_to_remove = sum(start_index, count);
		total_value -= value_to_remove;

		// Debug.Log("Before:\n" + string.Join("\n", history.Select(d => d.ToString())));
		history.RemoveRange(start_index, count);
		// Debug.Log("After:\n" + string.Join("\n", history.Select(d => d.ToString())));

		total_value += history_data_point.value;

		history.Insert(start_index, history_data_point);
	}

	private float sum(int start_index, int count)
	{
		var total = 0f;
		for (int i = start_index; i < (start_index + count); ++i)
		{
			total += history[i].value;
		}
		return total;
	}
}

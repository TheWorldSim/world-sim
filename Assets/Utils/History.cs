using System;
using System.Collections.Generic;

public class History<V>
{
	public List<HistoryDataPoint<V>> history { get; private set; }

	public History() {
		history = new List<HistoryDataPoint<V>>();
	}

	public Accessor<HistoryDataPoint<V>, DateTime> access_previous_date_time = new AccessPreviousDateTime();
	public Accessor<HistoryDataPoint<V>, DateTime> access_new_date_time = new AccessNewDateTime();

	public int search_new(DateTime date_time)
	{
		// This version takes 0.45 seconds with 10 power plants each with 600 GJ_by_date_times
		//return history.Select(v => v.change.new_date_time).ToList().BinarySearch(date_time);
		// versus 0.03 for this version
		return BinarySearch<HistoryDataPoint<V>, DateTime>.search(history, date_time, access_new_date_time);
	}

	public int search_previous(DateTime date_time)
	{
		//return history.Select(v => v.change.previous_date_time).ToList().BinarySearch(date_time);
		return BinarySearch<HistoryDataPoint<V>, DateTime>.search(history, date_time, access_previous_date_time);
	}

	public float total_time_s()
	{
		if (history.Count == 0) return 0;
		var first_dt = history[0].change.previous_date_time;
		var last_dt = history[history.Count - 1].change.new_date_time;
		return (float)(last_dt - first_dt).TotalSeconds;
	}

	public float total_time_years()
	{
		return total_time_s() / DataConversionsManager.s_per_year;
	}

	public void clear()
	{
		history = new List<HistoryDataPoint<V>>();
	}

	public (int start_index, int count) find_overlapping(ChangedDateTime change)
	{
		var start_index = search_new(change.previous_date_time);
		start_index = (start_index < 0) ? ~start_index : start_index + 1;

		var end_index = search_previous(change.new_date_time);
		end_index = (end_index < 0) ? ~end_index : end_index;

		var count = Math.Max(0, end_index - start_index);
		// Debug.Log("start_index " + start_index + " end_index " + end_index + " count " + count);
		return (start_index, count);
	}

	// Accessor classes
	public class AccessPreviousDateTime : Accessor<HistoryDataPoint<V>, DateTime>
	{
		public override DateTime transform(HistoryDataPoint<V> element)
		{
			return element.change.previous_date_time;
		}
	}

	public class AccessNewDateTime : Accessor<HistoryDataPoint<V>, DateTime>
	{
		public override DateTime transform(HistoryDataPoint<V> element)
		{
			return element.change.new_date_time;
		}
	}
}

public abstract class Accessor<E, V>
{
	public abstract V transform(E element);
}

public static class BinarySearch<E, V> where V : IComparable
{
	public static int search(List<E> sorted_list, V search_value, Accessor<E, V> accessor)
	{
		int min = 0;
		int max = sorted_list.Count - 1;
		while (min <= max)
		{
			int mid = (min + max) / 2;
			V value = accessor.transform(sorted_list[mid]);

			var result = search_value.CompareTo(value);

			if (result == 0)
			{
				return mid;
			}
			else if (result < 0)
			{
				max = mid - 1;
			}
			else
			{
				min = mid + 1;
			}
		}
		return ~min;
	}
}

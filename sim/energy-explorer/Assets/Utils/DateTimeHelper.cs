using System;
using UnityEngine;

public static class DateTimeHelper
{
	private static DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
	// https://stackoverflow.com/a/3354915/539490
	public static DateTime convert_from_unix_time_stamp(double timestamp)
	{
		return origin.AddSeconds(timestamp);
	}

	public static DateTime to_year(DateTime date_time)
	{
		if (date_time.Kind != DateTimeKind.Utc) throw new Exception("Datetime must have kind: UTC");
		var copied_date_time = new DateTime(year: date_time.Year, month: 1, day: 1);
		return copied_date_time;
	}

	public static DateTime to_month(DateTime date_time)
	{
		if (date_time.Kind != DateTimeKind.Utc) throw new Exception("Datetime must have kind: UTC");
		var copied_date_time = new DateTime(year: date_time.Year, month: date_time.Month, day: 1);
		return copied_date_time;
	}

	public static DateTime to_year_hourly(DateTime date_time)
	{
		if (date_time.Kind != DateTimeKind.Utc) throw new Exception("Datetime must have kind: UTC");
		var copied_date_time = new DateTime(year: date_time.Year, month: 1, day: 1, hour: date_time.Hour, minute: 0, second: 0);
		return copied_date_time;
	}

	public static DateTime to_month_hourly(DateTime date_time)
	{
		if (date_time.Kind != DateTimeKind.Utc) throw new Exception("Datetime must have kind: UTC");
		var copied_date_time = new DateTime(year: date_time.Year, month: date_time.Month, day: 1, hour: date_time.Hour, minute: 0, second: 0);
		return copied_date_time;
	}
}

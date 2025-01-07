using UnityEngine;


[System.Serializable]
public class WIND_CAPACITY_FACTOR_SUMMARY
{
	public WIND_CAPACITY_FACTOR_SUMMARY_INSTANCES instances;

	[System.Serializable]
	public class WIND_CAPACITY_FACTOR_SUMMARY_INSTANCES
	{
		public WIND_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE _2018_texas__offshore_80_Vestas_V90_2000;
		public WIND_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE _2018_texas_80_Vestas_V90_2000;
		public WIND_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE _2018_united_kingdom__offshore_80_Vestas_V90_2000;
		public WIND_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE _2018_united_kingdom_80_Vestas_V90_2000;

		[System.Serializable]
		public class WIND_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE
		{
			public WIND_CAPACITY_FACTOR_SUMMARY_ATTRIBUTES attributes;

			[System.Serializable]
			public class WIND_CAPACITY_FACTOR_SUMMARY_ATTRIBUTES
			{
				public ATTRIBUTE year_average;
				public ATTRIBUTE month_average;
				public ATTRIBUTE year_hourly_average;
				public ATTRIBUTE month_hourly_average;
			}
		}
	}

	public VALUE_REF get_value_ref(RegionOfInterest roi, DataTimeWindow time_window)
	{
		ATTRIBUTE wind_capacity_attribute = null;

		if (roi.province_name == RegionOfInterest.texas)
		{
			var texas = roi.offshore ? instances._2018_texas__offshore_80_Vestas_V90_2000 : instances._2018_texas_80_Vestas_V90_2000;

			if (time_window == DataTimeWindow.year_average) wind_capacity_attribute = texas.attributes.year_average;
			else if (time_window == DataTimeWindow.month_average) wind_capacity_attribute = texas.attributes.month_average;
			else if (time_window == DataTimeWindow.year_hourly_average) wind_capacity_attribute = texas.attributes.year_hourly_average;
			else if (time_window == DataTimeWindow.month_hourly_average) wind_capacity_attribute = texas.attributes.month_hourly_average;

		}
		else if (roi.country_name == RegionOfInterest.united_kingdom)
		{
			var uk = roi.offshore ? instances._2018_united_kingdom__offshore_80_Vestas_V90_2000 : instances._2018_united_kingdom_80_Vestas_V90_2000;

			if (time_window == DataTimeWindow.year_average) wind_capacity_attribute = uk.attributes.year_average;
			else if (time_window == DataTimeWindow.month_average) wind_capacity_attribute = uk.attributes.month_average;
			else if (time_window == DataTimeWindow.year_hourly_average) wind_capacity_attribute = uk.attributes.year_hourly_average;
			else if (time_window == DataTimeWindow.month_hourly_average) wind_capacity_attribute = uk.attributes.month_hourly_average;

		}
		
		if (wind_capacity_attribute == null)
		{
			Debug.LogWarning("Unsupported wind_capacity for roi: " + roi + "  and time_window: " + time_window);
			return null;
		}

		return wind_capacity_attribute.value_ref;
	}
}

using UnityEngine;


[System.Serializable]
public class SOLARPV_CAPACITY_FACTOR_SUMMARY
{
	public SOLARPV_CAPACITY_FACTOR_SUMMARY_INSTANCES instances;

	[System.Serializable]
	public class SOLARPV_CAPACITY_FACTOR_SUMMARY_INSTANCES
	{
		public SOLARPV_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE _2017_texas_loss10percent_tracking1_tilt35_azim180;
		public SOLARPV_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE _2018_texas_loss10percent_tracking1_tilt35_azim180;
		public SOLARPV_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE _2017_united_kingdom_loss10percent_tracking1_tilt35_azim180;
		public SOLARPV_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE _2018_united_kingdom_loss10percent_tracking1_tilt35_azim180;

		[System.Serializable]
		public class SOLARPV_CAPACITY_FACTOR_SUMMARY_PARENT_ATTRIBUTE
		{
			public SOLARPV_CAPACITY_FACTOR_SUMMARY_ATTRIBUTES attributes;

			[System.Serializable]
			public class SOLARPV_CAPACITY_FACTOR_SUMMARY_ATTRIBUTES
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
		ATTRIBUTE solarpv_capacity_attribute = null;

		if (roi.province_name == RegionOfInterest.texas)
		{
			var texas = instances._2018_texas_loss10percent_tracking1_tilt35_azim180;

			if (time_window == DataTimeWindow.year_average) solarpv_capacity_attribute = texas.attributes.year_average;
			else if (time_window == DataTimeWindow.month_average) solarpv_capacity_attribute = texas.attributes.month_average;
			else if (time_window == DataTimeWindow.year_hourly_average) solarpv_capacity_attribute = texas.attributes.year_hourly_average;
			else if (time_window == DataTimeWindow.month_hourly_average) solarpv_capacity_attribute = texas.attributes.month_hourly_average;

		}
		else if (roi.country_name == RegionOfInterest.united_kingdom)
		{
			var uk = instances._2018_united_kingdom_loss10percent_tracking1_tilt35_azim180;

			if (time_window == DataTimeWindow.year_average) solarpv_capacity_attribute = uk.attributes.year_average;
			else if (time_window == DataTimeWindow.month_average) solarpv_capacity_attribute = uk.attributes.month_average;
			else if (time_window == DataTimeWindow.year_hourly_average) solarpv_capacity_attribute = uk.attributes.year_hourly_average;
			else if (time_window == DataTimeWindow.month_hourly_average) solarpv_capacity_attribute = uk.attributes.month_hourly_average;

		}
		
		if (solarpv_capacity_attribute == null)
		{
			Debug.LogWarning("Unsupported solarpv_capacity for roi: " + roi + "  and time_window: " + time_window);
			return null;
		}

		return solarpv_capacity_attribute.value_ref;
	}
}

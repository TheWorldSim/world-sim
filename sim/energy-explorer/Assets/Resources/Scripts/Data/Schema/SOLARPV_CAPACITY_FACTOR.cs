using UnityEngine;


[System.Serializable]
public class SOLARPV_CAPACITY_FACTOR
{
	public SOLARPV_CAPACITY_FACTOR_INSTANCES instances;

	[System.Serializable]
	public class SOLARPV_CAPACITY_FACTOR_INSTANCES
	{
		public ATTRIBUTE _2017_texas_loss10percent_tracking1_tilt35_azim180;
		public ATTRIBUTE _2018_texas_loss10percent_tracking1_tilt35_azim180;
		public ATTRIBUTE _2017_united_kingdom_loss10percent_tracking1_tilt35_azim180;
		public ATTRIBUTE _2018_united_kingdom_loss10percent_tracking1_tilt35_azim180;
	}

	public VALUE_REF get_value_ref(RegionOfInterest roi)
	{
		ATTRIBUTE solarpv_capacity_attribute = null;

		if (roi.province_name == RegionOfInterest.texas)
		{
			solarpv_capacity_attribute = instances._2018_texas_loss10percent_tracking1_tilt35_azim180;
		}
		else if (roi.country_name == RegionOfInterest.united_kingdom)
		{
			solarpv_capacity_attribute = instances._2018_united_kingdom_loss10percent_tracking1_tilt35_azim180;
		}
		
		if (solarpv_capacity_attribute == null)
		{
			Debug.LogWarning("Unsupported solarpv_capacity for roi: " + roi);
			return null;
		}

		return solarpv_capacity_attribute.value_ref;
	}
}

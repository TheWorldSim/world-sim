using UnityEngine;


[System.Serializable]
public class WIND_CAPACITY_FACTOR
{
	public WIND_CAPACITY_FACTOR_INSTANCES instances;

	[System.Serializable]
	public class WIND_CAPACITY_FACTOR_INSTANCES
	{
		public ATTRIBUTE _2018_texas__offshore_80_Vestas_V90_2000;
		public ATTRIBUTE _2018_texas_80_Vestas_V90_2000;
		public ATTRIBUTE _2018_united_kingdom__offshore_80_Vestas_V90_2000;
		public ATTRIBUTE _2018_united_kingdom_80_Vestas_V90_2000;
	}

	public VALUE_REF get_value_ref(RegionOfInterest roi)
	{
		ATTRIBUTE wind_capacity_attribute = null;

		if (roi.province_name == RegionOfInterest.texas)
		{
			wind_capacity_attribute = roi.offshore ? instances._2018_texas__offshore_80_Vestas_V90_2000 : instances._2018_texas_80_Vestas_V90_2000;
		}
		else if (roi.country_name == RegionOfInterest.united_kingdom)
		{
			wind_capacity_attribute = roi.offshore ? instances._2018_united_kingdom__offshore_80_Vestas_V90_2000 : instances._2018_united_kingdom_80_Vestas_V90_2000;
		}
		
		if (wind_capacity_attribute == null)
		{
			Debug.LogWarning("Unsupported wind_capacity for roi: " + roi);
			return null;
		}

		return wind_capacity_attribute.value_ref;
	}
}

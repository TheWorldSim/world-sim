using UnityEngine;


[System.Serializable]
public class REGIONS_DATA
{
	public REGIONS_INSTANCES instances;

	[System.Serializable]
	public class REGIONS_INSTANCES
	{
		public REGION_PARENT_ATTRIBUTE texas;
		public REGION_PARENT_ATTRIBUTE texas__offshore;
		public REGION_PARENT_ATTRIBUTE united_kingdom;
		public REGION_PARENT_ATTRIBUTE united_kingdom__offshore;

		[System.Serializable]
		public class REGION_PARENT_ATTRIBUTE
		{
			public REGION_ATTRIBUTES attributes;

			[System.Serializable]
			public class REGION_ATTRIBUTES
			{
				public ATTRIBUTE latlons;
			}
		}
	}

	public VALUE_REF get_latlons_value_ref(RegionOfInterest roi)
	{
		VALUE_REF latlons_value_ref;

		if (roi.province_name == RegionOfInterest.texas)
		{
			latlons_value_ref = roi.offshore ? texas__offshore_latlons : texas_latlons;
		}
		else if (roi.country_name == RegionOfInterest.united_kingdom)
		{
			latlons_value_ref = roi.offshore ? united_kingdom__offshore_latlons : united_kingdom_latlons;
		}
		else
		{
			Debug.LogWarning("Unsupported roi for latlons: " + roi.country_name + " " + roi.province_name);
			return null;
		}

		return latlons_value_ref;
	}

	private VALUE_REF texas_latlons { get { return instances.texas.attributes.latlons.value_ref; } }
	private VALUE_REF texas__offshore_latlons { get { return instances.texas__offshore.attributes.latlons.value_ref; } }
	private VALUE_REF united_kingdom_latlons { get { return instances.united_kingdom.attributes.latlons.value_ref; } }
	private VALUE_REF united_kingdom__offshore_latlons { get { return instances.united_kingdom__offshore.attributes.latlons.value_ref; } }
}

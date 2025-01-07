using UnityEngine;
using System.Collections.Generic;
using System;


public enum DataTimeWindow
{
	hourly = 1,
	year_average = 2,
	month_average = 3,
	year_hourly_average = 4,
	month_hourly_average = 5
}


[System.Serializable]
public class WORLD_SIM_ROOT_DATA
{
	public string schema_version;
	public WORLD_SIM_DATA data;
	public Dictionary<string, UNIT> units;
	public List<DATA_SET_CONFIG> data_set_configs;
}


[System.Serializable]
public class WORLD_SIM_DATA
{
	public REGIONS_DATA regions;
	public SOLARPV_CAPACITY_FACTOR solarpv_capacity;
	public SOLARPV_CAPACITY_FACTOR_SUMMARY solarpv_capacity_summary;
	public WIND_FARMS_DATA wind_farms;
	public WIND_CAPACITY_FACTOR wind_turbine_capacity;
	public WIND_CAPACITY_FACTOR_SUMMARY wind_turbine_capacity_summary;
}


[System.Serializable]
public class WIND_FARMS_DATA
{
	public WIND_FARMS_ATTRIBUTES attributes;

	[System.Serializable]
	public class WIND_FARMS_ATTRIBUTES
	{
		public ATTRIBUTE nameplate_watts_per_square_meter;
	}

	public float nameplate_watts_per_square_meter { get { return attributes.nameplate_watts_per_square_meter.value_float; } }
}

[System.Serializable]
public class ATTRIBUTE
{
	public List<VALUE_REF> value_refs;
	public List<string> labels;
	public string description;

	public VALUE_REF value_ref { get { return DataManager.instance.value_ref_for_chosen_data_set(value_refs); } }
	public float value_float { get { return value_ref.value_float(0); } }
}

[System.Serializable]
public class VALUE_REF
{
	public List<float> values;
	public string value_file;
	public List<string> columns;
	public string reference;
	public string sub_ref;
	public string comment;
	public string calculation;
	public List<string> data_sets;

	public float value_float(int index) {
		if (index >= values.Count) throw new ArgumentOutOfRangeException("Max allowed index: " + (values.Count - 1) + " but index: " + index);
		return values[index];
	}
	public List<float> value1d_float { get { return values; } }
}

[System.Serializable]
public class UNIT
{

}

[System.Serializable]
public class DATA_SET_CONFIG
{
	public string name;
	public string draft_version;
	public string release_version;
	public List<string> versions;
}


public class WorldSimRegionData
{
	public RegionOfInterest region;
	public List<WSCell> cells;

	public CapacityFactorData wind_capacity_factor_hourly;
	public CapacityFactorData wind_capacity_factor_year_average;
	public CapacityFactorData wind_capacity_factor_month_average;
	public CapacityFactorData wind_capacity_factor_year_hourly_average;
	public CapacityFactorData wind_capacity_factor_month_hourly_average;

	public CapacityFactorData solarpv_capacity_factor_hourly;
	public CapacityFactorData solarpv_capacity_factor_year_average;
	public CapacityFactorData solarpv_capacity_factor_month_average;
	public CapacityFactorData solarpv_capacity_factor_year_hourly_average;
	public CapacityFactorData solarpv_capacity_factor_month_hourly_average;

	internal bool complete()
	{
		return (
			region != null
			&& cells != null
			&& wind_capacity_factor_hourly != null
			&& wind_capacity_factor_year_average != null
			&& wind_capacity_factor_month_average != null
			&& wind_capacity_factor_year_hourly_average != null
			&& wind_capacity_factor_month_hourly_average != null
			&&
			(
				region.offshore ||
				(
				solarpv_capacity_factor_hourly != null
				&& solarpv_capacity_factor_year_average != null
				&& solarpv_capacity_factor_month_average != null
				&& solarpv_capacity_factor_year_hourly_average != null
				&& solarpv_capacity_factor_month_hourly_average != null
				)
			)
		);
	}
}

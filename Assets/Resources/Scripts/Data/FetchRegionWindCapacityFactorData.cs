
public class FetchRegionWindCapacityFactorData: FetchRegionCapacityFactorData
{
	public static FetchRegionWindCapacityFactorData Init(RegionOfInterest roi, DataTimeWindow time_window)
	{
		var data_root = DataManager.instance.root_data;
		VALUE_REF capacity_factor_value_ref;

		if (time_window == DataTimeWindow.hourly)
		{
			capacity_factor_value_ref = data_root.data.wind_turbine_capacity.get_value_ref(roi);
		}
		else
		{
			capacity_factor_value_ref = data_root.data.wind_turbine_capacity_summary.get_value_ref(roi, time_window);
		}

		var value_file = capacity_factor_value_ref.value_file;
		var name = roi.offshore_str + " Wind Capacity Factor " + time_window;

		return new FetchRegionWindCapacityFactorData(value_file, name, roi, time_window);
	}

	protected FetchRegionWindCapacityFactorData(string resource, string name, RegionOfInterest roi, DataTimeWindow time_window) : base(resource, name, roi, time_window)
	{
	}
}

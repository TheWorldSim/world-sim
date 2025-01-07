
public class FetchRegionSolarPVCapacityFactorData : FetchRegionCapacityFactorData
{
	public static FetchRegionSolarPVCapacityFactorData Init(RegionOfInterest roi, DataTimeWindow time_window)
	{
		var data_root = DataManager.instance.root_data;
		VALUE_REF capacity_factor_value_ref;

		if (time_window == DataTimeWindow.hourly)
		{
			capacity_factor_value_ref = data_root.data.solarpv_capacity.get_value_ref(roi);
		}
		else
		{
			capacity_factor_value_ref = data_root.data.solarpv_capacity_summary.get_value_ref(roi, time_window);
		}

		var value_file = capacity_factor_value_ref.value_file;
		var name = "SolarPV Capacity Factor " + time_window;

		return new FetchRegionSolarPVCapacityFactorData(value_file, name, roi, time_window);
	}

	protected FetchRegionSolarPVCapacityFactorData(string resource, string name, RegionOfInterest roi, DataTimeWindow time_window) : base(resource, name, roi, time_window)
	{
	}
}

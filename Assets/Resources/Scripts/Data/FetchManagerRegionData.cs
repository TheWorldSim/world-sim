using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class FetchManagerRegionData : MultipleFetchersManager
{
	public static FetchManagerRegionData instance { get; private set; }

	private Dictionary<string, MultipleFetchersProgress> roi_id_to_progress = new Dictionary<string, MultipleFetchersProgress>();
	private Dictionary<MultipleFetchersProgress, List<string>> progress_to_roi_ids = new Dictionary<MultipleFetchersProgress, List<string>>();

	public delegate void RegionDataFetchProgress(MultipleFetchersProgress progress, List<WorldSimRegionData> datas);
	public event RegionDataFetchProgress on_regions_data_fetch_progress;

	void Awake()
	{
		instance = this;
	}

	internal MultipleFetchersProgress request(RegionOfInterest roi)
	{
		var progress = new MultipleFetchersProgress();
		return request(roi, progress, true);
	}

	internal MultipleFetchersProgress request(RegionOfInterest roi, MultipleFetchersProgress progress, bool is_first_call = false)
	{
		var roi_string_id = roi.string_id;
		if (roi_id_to_progress.ContainsKey(roi_string_id))
		{
			progress = roi_id_to_progress[roi_string_id];
			StartCoroutine(async_check_return_data(roi));
		}
		else
		{
			// Debug.Log("Requesting region data for " + roi_string_id);
			roi_id_to_progress[roi_string_id] = progress;

			if (!progress_to_roi_ids.ContainsKey(progress))
			{
				progress_to_roi_ids[progress] = new List<string>();
			}
			progress_to_roi_ids[progress].Add(roi_string_id);

			var latlons_data_fetcher = FetchRegionLatlonsData.Init(roi);
			latlons_data_fetcher.on_latlon_data += handle_on_latlon_data;
			progress.add_data_fetcher(latlons_data_fetcher);

			var time_windows = Enum.GetValues(typeof(DataTimeWindow)).OfType<DataTimeWindow>().ToList();
			time_windows.ForEach(time_window =>
			{
				var wind_capacity_data_fetcher = FetchRegionWindCapacityFactorData.Init(roi, time_window);
				wind_capacity_data_fetcher.on_capacity_factor_data += handle_on_wind_capacity_factor;
				progress.add_data_fetcher(wind_capacity_data_fetcher);
			});

			// hack to check we're not already fetching this resource
			if (is_first_call)
			{
				time_windows.ForEach(time_window =>
				{
					var solarpv_capacity_data_fetcher = FetchRegionSolarPVCapacityFactorData.Init(roi, time_window);
					solarpv_capacity_data_fetcher.on_capacity_factor_data += handle_on_solarpv_capacity_factor;
					progress.add_data_fetcher(solarpv_capacity_data_fetcher);
				});
			}

			start_requesting_data(progress);
		}

		return progress;
	}

	private Dictionary<string, WorldSimRegionData> region_data = new Dictionary<string, WorldSimRegionData>();
	private WorldSimRegionData get_or_create_region_data(RegionOfInterest roi)
	{
		var roi_string_id = roi.string_id;

		if (!region_data.ContainsKey(roi_string_id))
		{
			region_data[roi_string_id] = new WorldSimRegionData();
			region_data[roi_string_id].region = roi;
		}

		return get_region_data(roi_string_id);
	}
	private WorldSimRegionData get_region_data(string roi_string_id)
	{
		return region_data[roi_string_id];
	}

	private void handle_on_latlon_data(RegionOfInterest roi, List<WSCell> cells)
	{
		var data = get_or_create_region_data(roi);
		data.cells = cells;

		check_for_completion(roi);
	}

	private void handle_on_wind_capacity_factor(RegionOfInterest roi, DataTimeWindow time_window, CapacityFactorData capacity_factors)
	{
		// Debug.Log("Handling " + roi.offshore_str + " wind capacity factors for " + time_window);
		var data = get_or_create_region_data(roi);

		if (time_window == DataTimeWindow.hourly) data.wind_capacity_factor_hourly = capacity_factors;
		else if (time_window == DataTimeWindow.year_average) data.wind_capacity_factor_year_average = capacity_factors;
		else if (time_window == DataTimeWindow.month_average) data.wind_capacity_factor_month_average = capacity_factors;
		else if (time_window == DataTimeWindow.year_hourly_average) data.wind_capacity_factor_year_hourly_average = capacity_factors;
		else if (time_window == DataTimeWindow.month_hourly_average) data.wind_capacity_factor_month_hourly_average = capacity_factors;
		else throw new Exception("Unsupported DataTimeWindow " + time_window);

		check_for_completion(roi);
	}

	private void handle_on_solarpv_capacity_factor(RegionOfInterest roi, DataTimeWindow time_window, CapacityFactorData capacity_factors)
	{
		// Debug.Log("Handling solarpv capacity factors for " + time_window);
		var data = get_or_create_region_data(roi);

		if (time_window == DataTimeWindow.hourly) data.solarpv_capacity_factor_hourly = capacity_factors;
		else if (time_window == DataTimeWindow.year_average) data.solarpv_capacity_factor_year_average = capacity_factors;
		else if (time_window == DataTimeWindow.month_average) data.solarpv_capacity_factor_month_average = capacity_factors;
		else if (time_window == DataTimeWindow.year_hourly_average) data.solarpv_capacity_factor_year_hourly_average = capacity_factors;
		else if (time_window == DataTimeWindow.month_hourly_average) data.solarpv_capacity_factor_month_hourly_average = capacity_factors;
		else throw new Exception("Unsupported DataTimeWindow " + time_window);

		check_for_completion(roi);
	}

	private IEnumerator async_check_return_data(RegionOfInterest roi)
	{
		yield return new WaitForSeconds(0);
		check_for_completion(roi);
	}

	private void check_for_completion(RegionOfInterest roi)
	{
		var progress = roi_id_to_progress[roi.string_id];

		List<WorldSimRegionData> datas = null;

		if (progress.complete)
		{
			var roi_string_ids = progress_to_roi_ids[progress];
			datas = roi_string_ids.Select(roi_string_id => get_region_data(roi_string_id)).ToList();
		}

		on_regions_data_fetch_progress?.Invoke(progress, datas);
	}
}

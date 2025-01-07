using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using WPM;
using System;


public partial class DataManager : MonoBehaviour
{
	public WorldMapGlobe map;

	public delegate void RegionDataFetchProgress(MultipleFetchersProgress progress);
	public event RegionDataFetchProgress on_changed_region_data_fetch_progress;

	public bool region_data_ready(RegionOfInterest region) => data_root_ready && data_region_fetched(region);
	public bool region_datas_ready(RegionOfInterest region)
	{
		var other_region = region.clone(offshore: !region.offshore);
		return region_data_ready(region) && region_data_ready(other_region);
	}
	public WorldSimRegionData region_data(RegionOfInterest region) => regions[region.string_id];

	private void Start_data_manager_region_data()
	{
		GameManagerWorldSim.instance.on_changed_selected_region += handle_changed_selected_region;
		FetchManagerRegionData.instance.on_regions_data_fetch_progress += handle_regions_data_fetch_progress;
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		if (!data_region_not_been_fetched(region)) fetch_data(region);
	}

	#region data region status
	private AsyncStatusMultiple data_region_status(RegionOfInterest region)
	{
		try
		{
			return regions_progresses.ContainsKey(region.string_id) ? regions_progresses[region.string_id].status : AsyncStatusMultiple.not_fetching;
		}
		catch (Exception err)
		{
			throw err;
		}
	}
	private bool data_region_not_been_fetched(RegionOfInterest region) => data_region_status(region) != AsyncStatusMultiple.not_fetching;
	private bool data_region_fetched(RegionOfInterest region) => data_region_status(region) == AsyncStatusMultiple.all_finished_all_succeeded;
	#endregion

	// internal state
	private Dictionary<string, MultipleFetchersProgress> regions_progresses = new Dictionary<string, MultipleFetchersProgress >();
	private Dictionary<string, WorldSimRegionData> regions = new Dictionary<string, WorldSimRegionData>();

	// fetching
	private void fetch_data (RegionOfInterest region)
	{
		if (!data_root_ready) throw new Exception("Currently we want to get data_root before doing anything else.");
		var progress = FetchManagerRegionData.instance.request(region);
		regions_progresses[region.string_id] = progress;
		var other_region_part = region.clone(offshore: !region.offshore);
		progress = FetchManagerRegionData.instance.request(roi: other_region_part, progress);
		regions_progresses[other_region_part.string_id] = progress;
	}

	private void handle_regions_data_fetch_progress(MultipleFetchersProgress progress, List<WorldSimRegionData> datas)
	{
		if (progress.complete)
		{
			datas.ForEach(data =>
			{
				var roi = data.region;
				regions[roi.string_id] = data;

				data.cells.ForEach(c => WorldMapGlobeWrapper.add_WPM_cell_index(c));
			});
		}

		on_changed_region_data_fetch_progress?.Invoke(progress);
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using WPM;


public enum GridCellColourInfoType
{
	none = 0,
	wind_capacity_factor = 1,
	solarpv_capacity_factor = 2
}


public class CellsVisualsManager : MonoBehaviour
{
	private WorldMapGlobe map;

	private GridCellColourInfoType grid_cell_colour_info_type;
	private DataTimeWindow data_time_window = DataTimeWindow.hourly;
	private RegionOfInterest onshore_roi;
	private RegionOfInterest offshore_roi;
	private Dictionary<RegionOfInterest, CellsInfoManager> grid_cells_info_managers = new Dictionary<RegionOfInterest, CellsInfoManager>();

	private void Start()
	{
		map = WorldMapGlobe.instance;
		GameManagerWorldSim.instance.on_changed_selected_region += handle_changed_selected_region;
		GameManagerWorldSim.levels.on_changed_level_active += handle_changed_level_active;
	}

	private void Update()
	{
		update_grid_cell_info_colours();
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		show_cells_outline();
	}

	private void handle_changed_level_active(bool level_active, LevelConfig level_config)
	{
		if (level_config != null)
		{
			grid_cells_info_managers = new Dictionary<RegionOfInterest, CellsInfoManager>();
		}
	}

	//private void handle_changed_region_data_fetch_progress(MultipleFetchersProgress progress)
	//{
	//	if (progress.complete)
	//	{
	//		grid_cells_info_manager = GridCellsInfoManager.get_wind_capacity_factor_accessor(region_data, data_time_window);
	//	}
	//}

	public void show_cells_outline()
	{
		if (map.showHexagonalGrid) return;

		map.hexaGridDivisions = 200;
		map.showHexagonalGrid = true;
		// setting colour has to come after enabling the grid :/
		map.hexaGridColor = Colours.hex_grid_colour;
	}

	public void colour_nominal_region_with_capacity_factors(RegionOfInterest roi, BuildingType building_type)
	{
		if (building_type == BuildingType.wind_farm)
		{
			colour_nominal_region_with_wind_capacity_factors(roi);
		}
		else if (building_type == BuildingType.solar_panels)
		{
			colour_nominal_region_with_solarpv_capacity_factors(roi);
		}
	}

	public void colour_nominal_region_with_wind_capacity_factors(RegionOfInterest roi)
	{
		grid_cell_colour_info_type = GridCellColourInfoType.wind_capacity_factor;
		onshore_roi = roi.clone(offshore: false);
		offshore_roi = roi.clone(offshore: true);
	}

	public void colour_nominal_region_with_solarpv_capacity_factors(RegionOfInterest roi)
	{
		grid_cell_colour_info_type = GridCellColourInfoType.solarpv_capacity_factor;
		onshore_roi = roi.clone(offshore: false);
		offshore_roi = roi.clone(offshore: true);
	}

	public void clear_nominal_region_of_capacity_factors()
	{
		if (onshore_roi != null) clear_region_of_capacity_factors(onshore_roi);
		if (offshore_roi != null) clear_region_of_capacity_factors(offshore_roi);

		grid_cell_colour_info_type = GridCellColourInfoType.none;
		onshore_roi = null;
		offshore_roi = null;
	}

	private void update_grid_cell_info_colours()
	{
		if (onshore_roi == null || offshore_roi == null || grid_cell_colour_info_type == GridCellColourInfoType.none) return;

		var date_time = GameManagerWorldSim.date_time.game_date_time;

		if (grid_cell_colour_info_type == GridCellColourInfoType.wind_capacity_factor)
		{
			colour_region_with_wind_capacity_factors(onshore_roi, date_time);
			colour_region_with_wind_capacity_factors(offshore_roi, date_time);
		}
		else if (grid_cell_colour_info_type == GridCellColourInfoType.solarpv_capacity_factor)
		{
			colour_region_with_solarpv_capacity_factors(onshore_roi, date_time);
		}
	}

	private void colour_region_with_wind_capacity_factors(RegionOfInterest roi, DateTime date_time)
	{
		WorldSimRegionData region_data;

		if (DataManager.instance.region_data_ready(roi))
		{
			region_data = DataManager.instance.region_data(roi);
			if (!grid_cells_info_managers.ContainsKey(roi))
			{
				// Debug.Log("roi wind... " + roi.ToString() + " " + roi.GetHashCode());
				data_time_window = GameManagerWorldSim.levels.get_level_config().data_time_window;
				grid_cells_info_managers[roi] = CellsInfoManager.init_wind_capacity_factor(region_data, data_time_window);
			}
		}
		else throw new Exception("wind Data not ready for " + roi);

		var capacity_factor_accessor = grid_cells_info_managers[roi].get_date_time_accessor(date_time);

		region_data.cells.ForEach(cell =>
		{
			var capacity_factor = capacity_factor_accessor.get_capacity_factor(cell);
			map.SetCellColor(cell.get_wpm_cell_index(), Colours.bakers_blue8(capacity_factor, 0.25f, 0.65f));
		});
	}

	private void colour_region_with_solarpv_capacity_factors(RegionOfInterest roi, DateTime date_time)
	{
		WorldSimRegionData region_data;

		if (DataManager.instance.region_data_ready(roi))
		{
			region_data = DataManager.instance.region_data(roi);
			if (!grid_cells_info_managers.ContainsKey(roi))
			{
				// Debug.Log("roi solar... " + roi.ToString() + " " + roi.GetHashCode());
				data_time_window = GameManagerWorldSim.levels.get_level_config().data_time_window;
				grid_cells_info_managers[roi] = CellsInfoManager.init_solarpv_capacity_factor(region_data, data_time_window);
			}
		}
		else throw new Exception("solarpv Data not ready for " + roi);

		var capacity_factor_accessor = grid_cells_info_managers[roi].get_date_time_accessor(date_time);

		region_data.cells.ForEach(cell =>
		{
			var capacity_factor = capacity_factor_accessor.get_capacity_factor(cell);
			map.SetCellColor(cell.get_wpm_cell_index(), Colours.solar(capacity_factor, 0.1f, 0.4f));
		});
	}

	private void clear_region_of_capacity_factors(RegionOfInterest roi)
	{
		if (!DataManager.instance.region_data_ready(roi)) return; // Don't like this racyness . was called when setting build_active for building new turbine

		var region_data = DataManager.instance.region_data(roi);

		for (int i = 0; i < region_data.cells.Count; ++i)
		{
			var cell = region_data.cells[i];
			map.SetCellColor(cell.get_wpm_cell_index(), Colours.default_province_fill_colour);
		}
	}
}

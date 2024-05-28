using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WPM;
using System.Linq;
using System.Text;
using System;
using UnityEngine.Events;

public enum ResearchMode
{
	none = 0,
	editing_cells = 1,
}

public class ResearchUIManager : MonoBehaviour
{
	// Dependencies on other game objects
	private GameObject calculate_cells_in_region_button;
	private GameObject load_cells_in_region_button;
	private GameObject edit_cells;
	private GameObject show_boundary_of_region_button;
	private GameObject get_lat_lon_of_cells;
	private GameObject show_wind_capacity;

	// internal state
	private ResearchMode mode = ResearchMode.none;
	private HashSet<int> region_cell_indexes = new HashSet<int>();
	private string text_for_clipboard_area = "";

	//
	private GameManagerWorldSim game_state;
	private RegionOfInterest roi { get { return game_state.selected_region_of_interest; } }
	private WorldMapGlobe map;

	void Start()
    {
		game_state = GameManagerWorldSim.instance;
		map = WorldMapGlobe.instance;

		// Set up buttons
		calculate_cells_in_region_button = setup_button("FindCellsInRegionButton", handle_click_calculate_cells_in_region_button);
		load_cells_in_region_button = setup_button("LoadCellsInRegionButton", handle_click_load_cells_in_region_button);
		edit_cells = setup_button("EditCells", handle_click_edit_cells);
		show_boundary_of_region_button = setup_button("ShowBoundaryRegionButton", handle_click_show_boundary_of_region_button);
		get_lat_lon_of_cells = setup_button("GetLatLonOfCells", handle_click_get_lat_lon_of_cells);
		show_wind_capacity = setup_button("ShowWindCapacity", handle_click_show_wind_capacity);

		// add listener for editing ROI cells
		map.OnCellClick += handle_click_cell;

		// add listener for changing region
		game_state.on_changed_selected_region += handle_changed_selected_region;
	}

	void OnGUI()
	{
		if (text_for_clipboard_area != "")
		{
			GUILayout.TextArea(text_for_clipboard_area, GUILayout.Height(20), GUILayout.Width(100));
		}
	}

	private GameObject setup_button(string button_container_name, UnityAction handle_click)
	{
		var game_obj = transform.Find(button_container_name).gameObject;

		var button = game_obj.GetComponent<Button>();
		button.onClick.AddListener(handle_click);
		Colours.SetSelectedColour(button, Colours.button_selected);

		return game_obj;
	}

	private void log_warning(string message)
	{
		// TODO: also show in dialog to user
		Debug.LogWarning(message);
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		ensure_grid_enabled();
		clear_filled_cells();
		mode = ResearchMode.none;
		reset_edit_cells_button();
		text_for_clipboard_area = "";
		region_cell_indexes = new HashSet<int>();
	}

	#region handle clicks
	private void handle_click_calculate_cells_in_region_button()
	{
		clear_selected_button(calculate_cells_in_region_button);
		ensure_grid_enabled();
		clear_filled_cells();

		if (roi.offshore) log_warning("Offshore is arbitarily manually defined at the moment.  Can only calculate on shore region.");

		List<Province> provinces = new List<Province>();
		if (roi.province_name_exists()) provinces.Add(map.GetProvince(roi.country_name, roi.province_name));
		else provinces.AddRange(map.GetCountry(roi.country_name).provinces);

		// This block takes a long time as is very inefficient
		region_cell_indexes = new HashSet<int>();
		provinces.ForEach(province => province.regions.ForEach(region =>
		{
			var cell_indexes = map.GetCells(region);
			region_cell_indexes.UnionWith(cell_indexes);
		}));

		fill_colour_roi_cells();
	}

	private void handle_click_edit_cells()
	{
		if (mode == ResearchMode.editing_cells)
		{
			mode = ResearchMode.none;
			reset_edit_cells_button();
		}
		else
		{
			mode = ResearchMode.editing_cells;
			Colours.SetColour(edit_cells.GetComponent<Button>(), Colours.button_selected);
		}
	}
	private void reset_edit_cells_button()
	{
		clear_selected_button(edit_cells);
		Colours.SetColour(edit_cells.GetComponent<Button>(), Colours.button_unselected);
	}

	private void handle_click_get_lat_lon_of_cells()
	{
		if (text_for_clipboard_area == "")
		{
			var sb = new StringBuilder("lat,lon,vertex1_lat,vertex1_lon,vertex2_lat,vertex2_lon,vertex3_lat,vertex3_lon,vertex4_lat,vertex4_lon,vertex5_lat,vertex5_lon,vertex6_lat,vertex6_lon");

			if (region_cell_indexes.Count > 0) Debug.Log("Serialising " + region_cell_indexes.Count + " cell latlons.");
			else log_warning("No cells to serialise");

			var all_cells_indexes_list = new List<int>(region_cell_indexes);
			all_cells_indexes_list.ForEach(cell_index =>
			{
				sb.Append("\n");
				var cell = map.cells[cell_index];
				
				latlon_to_string(sb, cell.latlonCenter);
				cell.latlon.ToList().ForEach(latlon => {
					sb.Append(",");
					latlon_to_string(sb, latlon);
				});
				
				if (cell.latlon.Length == 5) sb.Append(",,");
			});

			text_for_clipboard_area = sb.ToString();
		}
		else text_for_clipboard_area = "";
	}

	private void latlon_to_string(StringBuilder sb, Vector2 latlon)
	{
		sb.Append(latlon.x.ToString(latlon_precision));
		sb.Append(",");
		sb.Append(latlon.y.ToString(latlon_precision));
	}
	private const string latlon_precision = "0.0000";

	private void handle_click_load_cells_in_region_button()
	{
		clear_selected_button(load_cells_in_region_button);
		ensure_grid_enabled();
		clear_filled_cells();

		var region_data = DataManager.instance.region_data(roi);
		region_cell_indexes = new HashSet<int>(region_data.cells.Select(c => c.get_wpm_cell_index()));

		var colour = roi.offshore ? Colours.cell_offshore_fill_colour : Colours.cell_fill_colour;
		fill_colour_cells(region_cell_indexes, colour);

		var other_roi = roi.clone(offshore: !roi.offshore);
		var other_region_data = DataManager.instance.region_data(other_roi);
		var other_cell_indexes = other_region_data.cells.Select(c => c.get_wpm_cell_index());

		var common_cell_indexes = region_cell_indexes.Intersect(other_cell_indexes).ToList();
		fill_colour_cells(common_cell_indexes, Colours.cell_conflict_fill_colour);
	}

	private void handle_click_show_boundary_of_region_button()
	{
		clear_selected_button(show_boundary_of_region_button);
		ensure_grid_enabled();

		var max_lat = -91f;
		var min_lat = 91f;
		var max_lon = -181f;
		var min_lon = 181f;

		region_cell_indexes.ToList().ForEach(idx => {
			var latlon = map.cells[idx].latlonCenter;

			max_lat = Mathf.Max(max_lat, latlon[0]);
			min_lat = Mathf.Min(min_lat, latlon[0]);
			max_lon = Mathf.Max(max_lon, latlon[1]);
			min_lon = Mathf.Min(min_lon, latlon[1]);
		});

		var latlon_extremes = new List<Vector2>() {
			new Vector2(min_lat, min_lon),
			new Vector2(min_lat, max_lon),
			new Vector2(max_lat, min_lon),
			new Vector2(max_lat, max_lon),
		};

		var cell_extremes = latlon_extremes.Select(latlon => map.GetCellIndex(latlon)).ToList();

		fill_colour_cells(cell_extremes, Colours.cell_region_boundary_fill_colour);
	}

	private void handle_click_show_wind_capacity()
	{
		clear_selected_button(show_wind_capacity);
		ensure_grid_enabled();

		GameManagerWorldSim.cell_visuals.colour_nominal_region_with_wind_capacity_factors(roi);
	}

	private void handle_click_cell(int cell_index)
	{
		if (mode != ResearchMode.editing_cells) return;

		var over_ui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);
		if (over_ui) return;

		if (region_cell_indexes.Contains(cell_index))
		{
			region_cell_indexes.Remove(cell_index);
			map.SetCellColor(cell_index, Colours.default_country_fill_colour);
		}
		else
		{
			region_cell_indexes.Add(cell_index);
			fill_colour_roi_cells();
		}
	}
	#endregion

	private void clear_selected_button(GameObject button_container)
	{
		var button = button_container.GetComponent<Button>();
		// clear the selection from the button
		button.enabled = false;
		button.enabled = true;
	}

	private void ensure_grid_enabled()
	{
		GameManagerWorldSim.cell_visuals.show_cells_outline();
	}

	#region fill cells
	private void clear_filled_cells()
	{
		map.ClearCells();
	}

	private void fill_colour_roi_cells()
	{
		fill_colour_cells(region_cell_indexes, Colours.cell_fill_colour);
	}

	private void fill_colour_cells(HashSet<int> cell_indexes, Color colour)
	{
		var cell_indexes_list = new List<int>(cell_indexes);
		fill_colour_cells(cell_indexes_list, colour);
	}
	private void fill_colour_cells(List<int> cell_indexes_list, Color colour)
	{
		map.SetCellColor(cell_indexes_list, colour);
	}
	#endregion
}

using UnityEngine;
using WPM;

public static class WorldMapGlobeWrapper
{
	public static WorldMapGlobe map { get { return WorldMapGlobe.instance; } }

	public static Vector3 latlon_to_world_pos(LonLat latlon)
	{
		return Conversion.GetSpherePointFromLatLon(latlon.to_vec2());
	}

	//public static WSCell wpm_cell_index_to_ws_cell(int wpm_cell_index, WorldSimRegionData region_data)
	//{
	//	var wpm_cell = map.cells[wpm_cell_index];
	//	var ws_cell = region_data.cells.Find(c => c.latlon == wpm_cell.latlonCenter);
	//	if (ws_cell == null) throw new Exception("Unable to find ws_cell for " + wpm_cell.latlonCenter);
	//	return ws_cell;
	//}

	public static int add_WPM_cell_index(WSCell cell)
	{
		var wpm_cell_index = cell.get_wpm_cell_index(false);
		if (wpm_cell_index == -1)
		{
			wpm_cell_index = map.GetCellIndex(cell.latlon);
			cell.set_wpm_cell_index(wpm_cell_index);
		}

		return wpm_cell_index;
	}

	// TODO move this to somewhere else
	internal static bool mouse_over_ui()
	{
		var over_ui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);
		return over_ui;
	}

	public static WSCell latlon_to_ws_cell(Vector2 latlon, WorldSimRegionData region_data)
	{
		var wpm_cell_index = map.GetCellIndex(latlon);
		var wpm_cell = map.cells[wpm_cell_index];
		var low_res_lat = float.Parse(wpm_cell.latlonCenter.x.ToString("0.0000"));
		var low_res_lon = float.Parse(wpm_cell.latlonCenter.y.ToString("0.0000"));
		var low_res_latlon = new Vector2(low_res_lat, low_res_lon);
		return region_data.cells.Find(ws_cell => ws_cell.latlon == low_res_latlon);
	}
}

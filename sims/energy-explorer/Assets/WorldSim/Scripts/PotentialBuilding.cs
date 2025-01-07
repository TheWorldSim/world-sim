using UnityEngine;
using HighlightPlus;
using System;
using System.Collections.Generic;

public class PotentialBuilding : MonoBehaviour
{
	private PowerPlantData data;
	private GameManagerWorldSim game_state;
	private NewBuildingManager building_manager;

	internal void Init(PowerPlantData data, RegionOfInterest roi)
	{
		if (this.data != null) throw new Exception("Already initialised potential building");

		this.data = data;
		GameManagerWorldSim.cell_visuals.colour_nominal_region_with_capacity_factors(roi, data.building_type);
	}

	private void Start()
	{
		game_state = GameManagerWorldSim.instance;
		building_manager = NewBuildingManager.instance;

		building_manager.on_changed_building_active += handle_building_active_change;
		GetComponent<HighlightEffect>().highlighted = true;
	}

	private Vector3 mouse_down_position;
	private void Update()
	{
		transform.position = game_state.get_position_of_cursor;
		transform.rotation = game_state.get_normal_at_cursor;

		if (Input.GetMouseButtonUp(1))
		{
			NewBuildingManager.instance.set_build_inactive();
		}

		if (Input.GetMouseButtonDown(0))
		{
			mouse_down_position = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0) && !WorldMapGlobeWrapper.mouse_over_ui())
		{
			var distance = (Input.mousePosition - mouse_down_position).magnitude;

			// Don't build if moved screen far enough
			// Should have minor bug in that it will build if you manage to move
			// screen all around and then precisely back to where you started
			if (distance > 5) return;

			building_manager.on_changed_building_active -= handle_building_active_change;

			data.set_lon_lat(game_state.get_lat_lon_of_cursor);
			//Debug.Log("building built at: " + data.lon_lat.lat + " lon: " + data.lon_lat.lon);
			NewBuildingManager.instance.built_building(data);

			Destroy(this); // remove PotentialBuilding component from building
		}
	}

	private void handle_building_active_change(BuildingType building_type, List<CombustiblePowerSource> combustible_power_sources)
	{
		if (data.building_type != building_type || !data.same_combustible_power_sources(combustible_power_sources))
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		building_manager.on_changed_building_active -= handle_building_active_change;
	}
}

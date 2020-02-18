using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsButton : MonoBehaviour
{
	public BuildingType building_type;
	public CombustiblePowerSource combustible_power_source = CombustiblePowerSource.none;
	private List<CombustiblePowerSource> combustible_power_sources {
		get {
			var ours = new List<CombustiblePowerSource>();
			if (combustible_power_source != CombustiblePowerSource.none) ours.Add(combustible_power_source);
			return ours;
		}
	}

	private bool active = false;
	private Color unselected_color = new Color(0.96f, 0.96f, 0.96f);
	private Color selected_color = new Color(0.46f, 0.77f, 0.96f);
	private Button button;

	private NewBuildingManager buildings_manager;

	void Start ()
    {
		buildings_manager = NewBuildingManager.instance;

		if (building_type == default)
		{
			throw new Exception("Must set building type");
		}

		if (building_type == BuildingType.thermal_combustion)
		{
			if (combustible_power_source == CombustiblePowerSource.none) throw new Exception("Must set combustible_power_source");
		}
		else
		{
			if (combustible_power_source != CombustiblePowerSource.none) throw new Exception("Must not set combustible_power_source");
		}

		button = gameObject.GetComponent<Button>();
		button.onClick.AddListener(handle_button_click);
		buildings_manager.on_changed_building_active += handle_changed_building_active;
	}

	private void handle_button_click ()
	{
		if (active)
		{
			buildings_manager.set_build_inactive();
		}
		else
		{
			buildings_manager.set_build_active(building_type, combustible_power_sources);
		}
	}

	private void handle_changed_building_active(BuildingType building_type, List<CombustiblePowerSource> combustible_power_sources)
	{
		active = this.building_type == building_type && same_sources(combustible_power_sources);

		var colors = button.colors;
		colors.normalColor = active ? selected_color : unselected_color;
		// also have to set the selected color of the button.  TODO: see about unselecting all buttons
		colors.selectedColor = colors.normalColor;
		button.colors = colors;
	}

	private bool same_sources(List<CombustiblePowerSource> other)
	{
		return CompareCombustiblePowerSource.same_combustible_power_sources(combustible_power_sources, other);
	}
}

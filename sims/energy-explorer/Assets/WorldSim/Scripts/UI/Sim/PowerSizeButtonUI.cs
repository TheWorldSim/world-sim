using System;
using UnityEngine;
using UnityEngine.UI;

public enum PowerSizeGW
{
	none = 0,
	two = 2,
	ten = 10,
	twenty = 20
}

public class PowerSizeButtonUI : MonoBehaviour
{
	public PowerSizeGW power_size_GW = PowerSizeGW.none;

	private bool active = false;
	private Color unselected_color = new Color(0.96f, 0.96f, 0.96f);
	private Color selected_color = new Color(0.46f, 0.77f, 0.96f);
	private Button button;
	private NewBuildingManager buildings_manager;

	private void Awake()
    {
		if (power_size_GW == PowerSizeGW.none) throw new Exception("power_size_GW is not set");

		buildings_manager = NewBuildingManager.instance;

		button = gameObject.GetComponent<Button>();
		button.onClick.AddListener(handle_button_click);
		buildings_manager.on_changed_power_size_GW += handle_changed_power_size_GW;
	}

	private void Start()
	{
		update_ui(buildings_manager.power_size_GW);
	}

	private void handle_button_click()
	{
		buildings_manager.change_power_size_GW(power_size_GW);
	}

	private void handle_changed_power_size_GW(PowerSizeGW power_size_GW)
	{
		update_ui(power_size_GW);
	}

	private void update_ui(PowerSizeGW power_size_GW)
	{
		active = this.power_size_GW == power_size_GW;

		var colors = button.colors;
		colors.normalColor = active ? selected_color : unselected_color;
		// also have to set the selected color of the button.  TODO: see about unselecting all buttons
		colors.selectedColor = colors.normalColor;
		button.colors = colors;
	}
}

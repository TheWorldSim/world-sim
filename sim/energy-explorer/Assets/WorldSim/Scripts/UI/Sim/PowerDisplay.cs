using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplay : MonoBehaviour
{
	private Button toggle_button;
	private bool display_graph = false;
	private Transform bars;
	private Transform graph;

	private RectTransform generation_base_bar;
	private RectTransform generation_storage_bar;
	private RectTransform demand_base_bar;
	private RectTransform demand_storage_bar;
	private GameObject power_image_charging;
	private GameObject power_image_discharging;
	private GameObject power_image_ok;
	private GameObject power_image_low;
	private Transform power_storage_bar;

	private TextMeshProUGUI demand_text;
	private RectTransform demand_container;

	private TextMeshProUGUI supply_text;
	private RectTransform supply_container;

	private PowerManager power;
	private LevelsManager levels;
	private LevelConfig level;

	void Awake()
    {
		toggle_button = transform.Find("ToggleButton").GetComponent<Button>();
		toggle_button.onClick.AddListener(handle_toggle_click);

		bars = transform.Find("Bars");
		graph = transform.Find("Graph");

		var power_bar = bars.Find("PowerBar");
		generation_base_bar = power_bar.Find("GenerationBaseBar").GetComponent<RectTransform>();
		generation_storage_bar = power_bar.Find("GenerationStorageBar").GetComponent<RectTransform>();
		demand_base_bar = power_bar.Find("DemandBaseBar").GetComponent<RectTransform>();
		demand_storage_bar = power_bar.Find("DemandStorageBar").GetComponent<RectTransform>();

		var power_images = bars.Find("PowerImages");
		power_image_charging = power_images.Find("PowerChargingImage").gameObject;
		power_image_discharging = power_images.Find("PowerDischargingImage").gameObject;
		power_image_ok = power_images.Find("PowerOkImage").gameObject;
		power_image_low = power_images.Find("PowerLowImage").gameObject;

		var power_storage_bar_container = bars.Find("PowerStorageBar").Find("Bar");
		power_storage_bar = power_storage_bar_container.Find("Value");

		var power_numbers = bars.Find("PowerNumbers");
		var demand = power_numbers.Find("Demand");
		demand_text = demand.GetComponent<TextMeshProUGUI>();
		demand_container = demand.GetComponent<RectTransform>();

		var supply = power_numbers.Find("Supply");
		supply_text = supply.GetComponent<TextMeshProUGUI>();
		supply_container = supply.GetComponent<RectTransform>();
	}

	private void Start()
	{
		power = GameManagerWorldSim.power;
		power.on_changed_grid_power += handle_grid_power_change;

		levels = GameManagerWorldSim.levels;
		levels.on_changed_level_active += handle_changed_level_active;
		level = levels.get_level_config();

		update_visibility();
	}

	private void handle_grid_power_change() { update_visual_display(); }

	private void update_visual_display()
	{
		if (display_graph) update_graph();
		else update_bars();
	}

	private void update_graph()
	{
		// TODO
	}

	private void update_bars()
	{
		power_image_charging.SetActive(power.is_charging());
		power_image_discharging.SetActive(power.is_discharging());
		power_image_low.SetActive(power.is_low());

		var gw_to_pixels_scale = 3;
		var generation_base_x_pos = power.generation_base_GW * gw_to_pixels_scale;
		var generation_storage_x_pos = power.generation_from_storage_GW * gw_to_pixels_scale;
		var demand_base_x_pos = power.demand_base_GW * gw_to_pixels_scale;
		var demand_storage_x_pos = power.demand_stored_GW * gw_to_pixels_scale;

		var height = 25f;

		generation_base_bar.sizeDelta = new Vector2(generation_base_x_pos, height);
		generation_storage_bar.sizeDelta = new Vector2(generation_storage_x_pos, height);
		demand_base_bar.sizeDelta = new Vector2(demand_base_x_pos, height);
		demand_storage_bar.sizeDelta = new Vector2(demand_storage_x_pos, height);

		generation_storage_bar.anchoredPosition = new Vector3(generation_base_x_pos, 0, 0);
		demand_storage_bar.anchoredPosition = new Vector3(demand_base_x_pos, 0, 0);

		// text
		demand_text.text = "-" + power.demand_base_GW.ToString("0.0");
		supply_text.text = "+" + power.generation_GW.ToString("0.0");

		//// text position
		//var power_number_widths = 80f + 4f; // + 4 for visual gap
		//var x = Mathf.Max(demand_x_pos - power_number_widths, 0);
		//var y = demand_container.anchoredPosition.y;
		//demand_container.anchoredPosition = new Vector3(x, y, 0);

		//x = Mathf.Max(supply_x_pos - power_number_widths, 0);
		//y = supply_container.anchoredPosition.y;
		//supply_container.anchoredPosition = new Vector3(x, y, 0);

		var power_storage_proportion = power.max_storage_GJ > 0 ? power.stored_GJ / power.max_storage_GJ : 0;
		power_storage_bar.localScale = new Vector3(1, power_storage_proportion, 1);
	}

	private void handle_toggle_click()
	{
		display_graph = !display_graph;
		update_visibility();
	}

	private void update_visibility()
	{
		bars.gameObject.SetActive(!display_graph);
		graph.gameObject.SetActive(display_graph);
	}

	private void handle_changed_level_active(bool level_active, LevelConfig level_config)
	{
		level = level_config;
	}
}

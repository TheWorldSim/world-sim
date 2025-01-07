using UnityEngine;
using WPM;

public class ChooseRegion : DialogMenu
{
	private WorldMapGlobe map;
	private GameManagerWorldSim game_state;
	private MainCamera main_camera;

	protected override void Awake()
	{
		base.Awake();
		confirm_enabled = true;
	}

	protected override void Start()
	{
		base.Start();
		game_state = GameManagerWorldSim.instance;
		map = WorldMapGlobe.instance;
		main_camera = MainCamera.instance;
	}

	private int last_option_chosen = -1;
	protected override void handle_click_option(int option)
	{
		base.handle_click_option(option);

		var already_selected = last_option_chosen == option;
		if (already_selected) return;
		last_option_chosen = option;

		var chosen_texas = option % 2 == 0;
		var chosen_uk = option % 2 == 1;

		if (chosen_texas) main_camera.fly_to_province(RegionOfInterest.texas);
		else if (chosen_uk) main_camera.fly_to_country(RegionOfInterest.united_kingdom);
		else Debug.LogError("Unsupported ChooseRegion chosen_option: " + option);

		main_camera.target_zoom_level = true;
	}

	protected override void handle_click_confirm()
	{
		base.handle_click_confirm();
		var country_name = chosen_option % 2 == 0 ? RegionOfInterest.united_states_of_america : RegionOfInterest.united_kingdom;
		var country_index = map.GetCountryIndex(country_name);
		var province_name = chosen_option % 2 == 0 ? RegionOfInterest.texas : "";
		var time_zone = chosen_option % 2 == 0 ? -6 : 0;
		var currency = chosen_option % 2 == 0 ? Currency.USD : Currency.GBP;
		var offshore = chosen_option >= 2; // i.e. 0 || 1 == land, 2 || 3 == offshore

		var region = new RegionOfInterest(
			country_name: country_name,
			country_index: country_index,
			province_name: province_name,
			time_zone: time_zone,
			currency: currency,
			offshore: offshore
		);
		game_state.chosen_region(region, this);
	}
}

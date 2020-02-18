using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Menu
{
	none = 1,
	main = 2,
	role = 3,
	region = 4,
	region_advanced = 5,
}

public partial class UIManager : MonoBehaviour
{
	public static UIManager instance { get; private set; }

	public void selected_main_menu_item(Menu menu_item)
	{
		hide_menu();
		menu_item = menu_item == Menu.region ? region_menu(game_state.selected_user_role) : menu_item;
		show_menu(menu_item);
	}

	private const float default_menu_animation_speed = 2f;

	// Dependencies on other game objects
	private GameObject menu_role_selection;
	private GameObject menu_region_selection;
	private GameObject menu_advanced_region_selection;
	private GameObject loading_screen_container;
	private GameObject menu_main_menu;
	private GameObject research_ui_container;
	private GameObject player_ui_container;

	// Internal state
	private Menu selected_menu = Menu.none;
	private UserRole user_role;
	private RegionOfInterest region;

	private GameManagerWorldSim game_state;

	private void Start()
	{
		instance = this;

		game_state = GameManagerWorldSim.instance;
		// handle changes in state (from user choosing from menus)
		game_state.on_changed_selected_user_role += handle_changed_selected_user_role;
		game_state.on_changed_selected_region += handle_changed_selected_region;

		var main_menu_button = transform.Find("MenuIcon").Find("MainMenuButton").GetComponent<Button>();
		main_menu_button.onClick.AddListener(handle_click_main_menu);

		menu_role_selection = transform.Find("ChooseRole").gameObject;
		menu_region_selection = transform.Find("ChooseRegion").gameObject;
		menu_advanced_region_selection = transform.Find("ChooseRegionAdvanced").gameObject;
		loading_screen_container = transform.Find("LoadingScreen").gameObject;
		menu_main_menu = transform.Find("MainMenu").gameObject;

		research_ui_container = transform.Find("ResearchUI").gameObject;
		player_ui_container = transform.Find("PlayerUI").gameObject;

		if (game_state.on_start_show_menu)
		{
			var speed = GameManagerWorldSim.instance.animation_speed;
			show_menu(Menu.region, animation_speed: speed, delay_from_start: 4f / speed);
		}

		Start_levels_ui_manager();
	}

	#region handle events
	private void handle_changed_selected_user_role(UserRole previous_user_role, UserRole user_role, object source)
	{
		this.user_role = user_role;
		var from_role_menu = Equals(source, menu_role_selection.GetComponent<DialogMenu>());

		if (from_role_menu)
		{
			hide_menu();
			update_full_ui();
		}
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		this.region = region;
		var from_region_menu = Equals(source, menu_region_selection.GetComponent<DialogMenu>());
		var from_region_advanced_menu = Equals(source, menu_advanced_region_selection.GetComponent<DialogMenu>());

		if (from_region_menu || from_region_advanced_menu)
		{
			hide_menu();
			update_full_ui();
		}
	}
	#endregion

	#region handle interactions
	public void handle_click_main_menu()
	{
		if (selected_menu != Menu.none) return;
		show_menu(Menu.main);
	}
	#endregion

	private Menu region_menu(UserRole user_role)
	{
		return user_role == UserRole.player ? Menu.region : Menu.region_advanced;
	}

	private void hide_menu()
	{
		if (selected_menu == Menu.none) return;
		var menu_obj = menu_from_id(selected_menu);
		menu_obj.SetActive(false);
		selected_menu = Menu.none;
	}

	private void show_menu(Menu menu_id, float animation_speed = default_menu_animation_speed, float delay_from_start = 0f)
	{
		if (selected_menu != Menu.none || menu_id == Menu.none) return;
		selected_menu = menu_id;

		float scale = game_state.skip_slow_transitions ? 1 : 0;

		var menu_obj = menu_from_id(menu_id);
		menu_obj.SetActive(true);
		var menu = menu_obj.GetComponent<DialogMenu>();
		menu.delay_from_start = delay_from_start;
		menu.scale = scale;
		menu.animation_speed = animation_speed;
	}

	private GameObject menu_from_id(Menu menu_id)
	{
		if (menu_id == Menu.main) return menu_main_menu;
		else if (menu_id == Menu.role) return menu_role_selection;
		else if (menu_id == Menu.region) return menu_region_selection;
		else if (menu_id == Menu.region_advanced) return menu_advanced_region_selection;
		else
		{
			Debug.LogError("Unsupported menu_id " + menu_id);
			return menu_main_menu;
		}
	}

	private void update_full_ui()
	{
		display_player_ui(user_role == UserRole.player);
		display_research_ui(user_role == UserRole.researcher);
	}

	private void display_player_ui(bool visible)
	{
		player_ui_container.SetActive(false);
		hide_levels_ui();

		if (!visible) return;
		else if (!DataManager.instance.region_datas_ready(region))
		{
			display_loading_region_data();
		}
		else if (GameManagerWorldSim.levels.level_active)
		{
			player_ui_container.SetActive(true);
		}
		else
		{
			show_levels_ui();
		}
	}

	private void display_research_ui(bool visible)
	{
		if (!visible)
		{
			research_ui_container.SetActive(false);
		}
		else if (!DataManager.instance.region_datas_ready(region))
		{
			display_loading_region_data();
		}
		else
		{
			research_ui_container.SetActive(visible);
		}
	}

	private void display_loading_region_data()
	{
		loading_screen_container.SetActive(true);
		DataManager.instance.on_changed_region_data_fetch_progress += handle_changed_region_data_fetch_progress;
	}

	private void handle_changed_region_data_fetch_progress(MultipleFetchersProgress progress)
	{
		if (DataManager.instance.region_datas_ready(region))
		{
			DataManager.instance.on_changed_region_data_fetch_progress -= handle_changed_region_data_fetch_progress;
			loading_screen_container.SetActive(false);
			update_full_ui();
		}
	}
}


public class MenuMainMenu : DialogMenu
{
	protected override void Awake()
	{
		base.Awake();
		confirm_enabled = false;
	}

	protected override void handle_click_option(int option)
	{
		base.handle_click_option(option);
		Menu menu_item = option == 0 ? Menu.role : Menu.region;
		UIManager.instance.selected_main_menu_item(menu_item);
	}
}

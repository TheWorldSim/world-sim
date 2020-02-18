using UnityEngine;

public enum UserRole
{
	none = 0,
	researcher = 1,
	player = 2
}

public class ChooseRole : DialogMenu
{
	protected override void Awake()
	{
		base.Awake();
		confirm_enabled = false;
	}

	protected override void handle_click_option(int option)
	{
		base.handle_click_option(option);

		UserRole role = UserRole.player;
		if (option == 0) role = UserRole.researcher;
		else if (option == 1) role = UserRole.player;
		else Debug.LogError("Unsupported ChooseRole chosen_option: " + option);

		GameManagerWorldSim.instance.chosen_role(role, this);
	}
}

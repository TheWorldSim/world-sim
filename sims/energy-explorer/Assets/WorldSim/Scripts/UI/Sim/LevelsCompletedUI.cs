
public class LevelsCompletedUI : DialogMenu
{
	protected override void Start()
	{
		base.Start();
		button_confirm.interactable = false;
	}

	protected override void handle_click_confirm()
	{
		base.handle_click_confirm();
	}
}

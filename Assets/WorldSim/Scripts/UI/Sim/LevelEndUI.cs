using TMPro;

public class LevelEndUI : DialogMenu
{
	protected override void Start()
	{
		base.Start();
		button_confirm.interactable = true;
	}

	protected override void OnEnable()
	{
		var level_config = GameManagerWorldSim.levels.get_level_config();

		var title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
		title.text = "Level " + level_config.level_number + " Complete";

		var explanation = transform.Find("Explanation").GetComponent<TextMeshProUGUI>();
		explanation.text = "Score: ";
	}

	protected override void handle_click_confirm()
	{
		base.handle_click_confirm();
		GameManagerWorldSim.levels.next_level();
	}
}

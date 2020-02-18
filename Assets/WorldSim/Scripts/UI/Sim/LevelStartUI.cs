using System;
using TMPro;

public class LevelStartUI : DialogMenu
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
		title.text = replace_with_region_name(level_config.title);

		var explanation = transform.Find("Explanation").GetComponent<TextMeshProUGUI>();
		explanation.text = replace_with_region_name(level_config.explanation);
	}

	private string replace_with_region_name(string text)
	{
		var text_parts = text.Split(new string[] { "<region>" }, StringSplitOptions.None);
		var region = GameManagerWorldSim.instance.selected_region_of_interest.ToShortDisplayString();
		return string.Join(region, text_parts);
	}

	protected override void handle_click_confirm()
	{
		base.handle_click_confirm();
		GameManagerWorldSim.levels.start_level();
	}
}

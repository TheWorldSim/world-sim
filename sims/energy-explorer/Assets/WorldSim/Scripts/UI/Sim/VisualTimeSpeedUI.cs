using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VisualTimeSpeedUI : MonoBehaviour
{
	private Button button;
	private TextMeshProUGUI text;

	void Start()
    {
		button = GetComponent<Button>();
		button.onClick.AddListener(handle_button_click);

		GameManagerWorldSim.input_keyboard.on_requested_game_pause_toggle += handle_requested_game_pause_toggle;

		text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
		var date_time = GameManagerWorldSim.date_time;
		set_text(date_time.speed_int, date_time.paused);
		date_time.on_changed_speed += handle_changed_speed;
	}

	private void handle_button_click()
	{
		GameManagerWorldSim.date_time.toggle_paused();
	}

	private void handle_requested_game_pause_toggle()
	{
		GameManagerWorldSim.date_time.toggle_paused();
	}

	private void handle_changed_speed(int new_speed_int, bool paused)
	{
		set_text(new_speed_int, paused);
	}

	private void set_text(int speed_int, bool paused)
	{
		speed_int = paused ? 0 : speed_int;
		text.text = (speed_int == 0) ? "P" : speed_int.ToString();
	}
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ChangeSpeed
{
	no_change = 0,
	increase = 1,
	decrease = -1
}

[RequireComponent(typeof(Button))]
public class VisualTimeSpeedButtonUI : MonoBehaviour
{
	public ChangeSpeed change_speed = ChangeSpeed.no_change;
	private Button button;

	void Awake()
    {
		button = GetComponent<Button>();
		button.onClick.AddListener(handle_button_click);

		var colors = button.colors;
		colors.selectedColor = colors.normalColor; //TODO: see about unselecting all buttons
		button.colors = colors;
	}

	private void Start()
	{
		GameManagerWorldSim.input_keyboard.on_requested_change_game_speed += handle_requested_change_game_speed;
	}

	private void handle_requested_change_game_speed(ChangeSpeed direction)
	{
		if (direction == change_speed)
		{
			ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
		}
	}

	private void handle_button_click()
	{
		GameManagerWorldSim.date_time.change_speed((int)change_speed);
	}
}

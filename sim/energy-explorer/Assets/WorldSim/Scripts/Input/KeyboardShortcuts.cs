using System.Collections.Generic;
using UnityEngine;

public class KeyboardShortcuts : MonoBehaviour
{
	public delegate void RequestedChangeGameSpeed(ChangeSpeed direction);
	public event RequestedChangeGameSpeed on_requested_change_game_speed;
	public delegate void RequestedGamePauseToggle();
	public event RequestedGamePauseToggle on_requested_game_pause_toggle;

	private List<KeyCode> keyboard_shortcut_increase_time = new List<KeyCode>() { KeyCode.Plus, KeyCode.Equals };
	private List<KeyCode> keyboard_shortcut_decrease_time = new List<KeyCode>() { KeyCode.Minus };
	private List<KeyCode> keyboard_shortcut_toggle_pause = new List<KeyCode>() { KeyCode.Space };

	private void Update()
	{
		if (is_key_down(keyboard_shortcut_increase_time))
		{
			on_requested_change_game_speed?.Invoke(ChangeSpeed.increase);
		}

		if (is_key_down(keyboard_shortcut_decrease_time))
		{
			on_requested_change_game_speed?.Invoke(ChangeSpeed.decrease);
		}

		if (is_key_down(keyboard_shortcut_toggle_pause))
		{
			on_requested_game_pause_toggle?.Invoke();
		}

		//if (Input.anyKeyDown)
		//{
		//	OnGUI();
		//}
	}

	//private void OnGUI()
	//{
	//	var e = Event.current;
	//	Debug.Log("Current detected event: " + e);
	//	if (e != null && e.isKey)
	//	{
	//		string key = e.keyCode.ToString();
	//		Debug.Log(key);
	//	}
	//}

	private bool is_key_down(List<KeyCode> codes)
	{
		for (int i = 0; i < codes.Count; i++)
		{
			if (Input.GetKeyDown(codes[i])) return true;
		}
		return false;
	}
}

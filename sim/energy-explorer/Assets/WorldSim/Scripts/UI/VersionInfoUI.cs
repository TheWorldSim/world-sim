using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class VersionInfoUI : MonoBehaviour
{
	private TextMeshProUGUI version;
    void Start()
    {
		version = transform.Find("Version").GetComponent<TextMeshProUGUI>();
		var chosen_data_set = DataManager.instance.chosen_data_set;
		chosen_data_set = chosen_data_set == "" ? "?" : chosen_data_set;
		set_text(chosen_data_set);
		DataManager.instance.on_changed_chosen_data_set += handle_changed_chosen_data_set;
	}

	private void set_text(string data_set)
	{
		version.text = "WorldSim 0.0.1; Data set " + data_set;
	}

	private void handle_changed_chosen_data_set(string chosen_data_set)
	{
		set_text(chosen_data_set);
	}
}

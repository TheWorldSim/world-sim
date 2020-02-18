using System;
using TMPro;
using UnityEngine;

public class LevelInfoPanelUIManager : MonoBehaviour
{
	//private TextMeshProUGUI time_remaining;
	private TextMeshProUGUI uptime;
	private TextMeshProUGUI last_outage_date;
	private TextMeshProUGUI last_outage_length;
	private LevelConfig level;

	void Start()
    {
		//time_remaining = transform.Find("TimeRemaining").GetComponent<TextMeshProUGUI>();
		uptime = transform.Find("Uptime").GetComponent<TextMeshProUGUI>();
		last_outage_date = transform.Find("LastOutageDate").GetComponent<TextMeshProUGUI>();
		last_outage_length = transform.Find("LastOutageLength").GetComponent<TextMeshProUGUI>();

		var levels = GameManagerWorldSim.levels;
		levels.on_changed_success_state += handle_changed_success_state;
		levels.on_changed_level_active += handle_changed_level_active;
		handle_changed_level_active(levels.level_active, levels.get_level_config());
	}

	private void handle_changed_success_state(float uptime_time_s, float level_time_to_elapse_s, DateTime? last_outage_start_date_time, TimeSpan? last_outage_time_span)
	{
		var remaining_s = level_time_to_elapse_s - uptime_time_s;
		var percent_remaining = (remaining_s / level_time_to_elapse_s) * 100;
		var rounded_percent_remaining = Mathf.Ceil((float)percent_remaining);

		//var remaining = level_time_to_elapse - successfully_elapsed_time;
		//time_remaining.text = rounded_percent_remaining.ToString("0") + "% (" + time_span_to_text(remaining) + ")";

		var rounded_percent_complete = 100 - rounded_percent_remaining;
		var uptime_time_span = TimeSpan.FromSeconds(uptime_time_s);
		uptime.text = rounded_percent_complete.ToString("0") + "% (" + time_span_to_text(uptime_time_span) + ")";

		if (last_outage_start_date_time.HasValue)
		{
			last_outage_date.text = last_outage_start_date_time.Value.ToString("yyyy MMM d ddd  HH:mm");
			last_outage_length.text = last_outage_time_span.HasValue
				? time_span_to_text(last_outage_time_span.Value)
				: "";
		}
	}

	private void handle_changed_level_active(bool level_active, LevelConfig level_config)
	{
		if (level_active)
		{
			last_outage_date.text = "";
			last_outage_length.text = "";

			level = level_config;
		}
	}

	private string time_span_to_text(TimeSpan time_span)
	{
		if (time_span.Days > 0)
		{
			return time_span.Days + " Days " + time_span.Hours + " Hours";
		}
		else if (time_span.Hours > 0)
		{
			return time_span.Hours + " Hours " + rounded_minutes_str(time_span.Minutes) + " Minutes";
		}
		else
		{
			return rounded_minutes_str(time_span.Minutes) + " Minutes";
		}
	}

	private int rounded_minutes(int minutes)
	{
		return (int)Mathf.Floor(minutes / 10f) * 10;
	}

	private string rounded_minutes_str(int minutes)
	{
		var mins = rounded_minutes(minutes);
		return (mins == 0) ? "00" : mins.ToString();
	}
}

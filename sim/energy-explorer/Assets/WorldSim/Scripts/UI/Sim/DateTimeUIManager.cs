using System;
using TMPro;
using UnityEngine;

public enum DatetimeSeason
{
	Spring = 1,
	Summer = 2,
	Autumn = 3,
	Winter = 4,
}

public class DateTimeUIManager : MonoBehaviour
{
	private TextMeshProUGUI season_year_text;
	private TextMeshProUGUI date_time_text;
	private float time_zone;
	private int game_speed_int;

	void Start()
    {
		season_year_text = transform.Find("SeasonYear").GetComponent<TextMeshProUGUI>();
		date_time_text = transform.Find("DateTime").GetComponent<TextMeshProUGUI>();
		var date_time = GameManagerWorldSim.date_time;
		date_time.on_changed_game_date_time += handle_changed_game_date_time;
		date_time.on_changed_speed += handle_changed_speed;
		game_speed_int = date_time.speed_int;

		var roi = GameManagerWorldSim.instance.selected_region_of_interest;
		time_zone = roi == null ? 0 : roi.time_zone; 
		GameManagerWorldSim.instance.on_changed_selected_region += handle_changed_selected_region;
	}

	private void handle_changed_speed(int new_speed_int, bool paused)
	{
		game_speed_int = new_speed_int;
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		time_zone = region.time_zone;
	}

	private void handle_changed_game_date_time(DateTime new_datetime)
	{
		var time_zone_corrected_datetime = new_datetime.AddHours(time_zone);

		// TODO Correct for lattitude of country (North / South hemisphere)
		var season = (DatetimeSeason)Mathf.Ceil(time_zone_corrected_datetime.Month / 3f);
		season_year_text.text = time_zone_corrected_datetime.ToString("yyyy ") + season;

		var dt = time_zone_corrected_datetime.ToString("MMM d ddd  HH:mm:ss");
		var str = game_speed_int < 2 ? "0"
			: (game_speed_int < 4 ? ":00"
			: (game_speed_int < 6 ? "0:00": "00:00"));
		date_time_text.text = dt.Substring(0, dt.Length - str.Length) + str;
	}
}

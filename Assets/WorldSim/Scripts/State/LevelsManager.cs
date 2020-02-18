using System;
using System.Collections.Generic;
using UnityEngine;


public class LevelsManager
{
	public bool level_active { get; private set; }

	public delegate void ChangedLevelActive(bool level_active, LevelConfig level_config);
	public event ChangedLevelActive on_changed_level_active;
	public delegate void ChangedSuccessState(float uptime_s, float level_time_to_elapse_s, DateTime? last_outage_start_date_time, TimeSpan? last_outage_time_span);
	public event ChangedSuccessState on_changed_success_state;

	private int level_number = 1;

	public HistoryNumerical history_uptime { get; private set; }
	private DateTime? last_low_power_start_date_time = null;
	private DateTime? last_low_power_end_date_time = null;
	private float level_time_to_elapse_s;
	private bool full_outage = false;
	private bool uninterrupted_power = false;

	public bool level_success { get; private set; }
	private PowerManager power;

	public LevelsManager(PowerManager power, GameDateTimeManager date_time)
	{
		level_active = false;
		level_success = false;
		date_time.on_changed_sim_date_time += handle_changed_sim_date_time;
		this.power = power;
	}

	public LevelConfig get_level_config()
	{
		return LevelConfig.get_level_config(level_number);
	}

	public void start_level()
	{
		level_active = true;
		level_success = false;

		var level_config = get_level_config();

		history_uptime = new HistoryNumerical();
		last_low_power_start_date_time = null;
		last_low_power_end_date_time = null;
		level_time_to_elapse_s = (float)(level_config.date_time_end - level_config.date_time_start).TotalSeconds;
		full_outage = false;
		uninterrupted_power = false;

		on_changed_level_active?.Invoke(level_active, level_config);
	}

	private void finish_level(bool success)
	{
		level_active = false;
		level_success = success;
		var level_config = get_level_config();
		on_changed_level_active?.Invoke(level_active, level_config);
	}

	internal bool have_level()
	{
		return LevelConfig.have_level_config(level_number);
	}

	internal void next_level()
	{
		level_number++;
		level_active = false;
		level_success = false;
		var level_config = have_level() ? get_level_config() : null;
		on_changed_level_active?.Invoke(level_active, level_config);
	}

	private void handle_changed_sim_date_time(ChangedDateTime change)
	{
		if (!level_active) return;

		var low_power = power.is_low();

		var uptime_s = low_power ? 0f : change.elapsed_time_s;
		var history_data_point = new HistoryDataPoint<float>(change, uptime_s);
		history_uptime.update(change, history_data_point);
		var total_uptime_s = history_uptime.total_value;

		update_last_outage_date_times(change, low_power);

		TimeSpan last_outage_time_span = calculate_last_outage_time_span();

		on_changed_success_state?.Invoke(total_uptime_s, level_time_to_elapse_s, last_low_power_start_date_time, last_outage_time_span);

		if (uninterrupted_power) finish_level(true);
	}

	private void update_last_outage_date_times(ChangedDateTime change, bool low_power)
	{
		if (low_power)
		{
			if (last_low_power_start_date_time == null || last_low_power_end_date_time != null)
			{
				last_low_power_start_date_time = change.new_date_time;
				last_low_power_end_date_time = null;
			}
			else if (change.includes(last_low_power_start_date_time.Value))
			{
				full_outage = true;
			}
			uninterrupted_power = false;
		}
		else
		{
			if (last_low_power_end_date_time == null)
			{
				last_low_power_end_date_time = change.new_date_time;
				full_outage = false;
			}
			else if (change.includes(last_low_power_end_date_time.Value))
			{
				uninterrupted_power = true;
			}
		}
	}

	private TimeSpan calculate_last_outage_time_span()
	{
		var level = get_level_config();

		TimeSpan last_outage_time_span;

		if (last_low_power_start_date_time.HasValue)
		{
			var end = full_outage
				? last_low_power_start_date_time.Value
				: (last_low_power_end_date_time.HasValue
					? last_low_power_end_date_time.Value
					: GameManagerWorldSim.date_time.game_date_time);

			if (end > last_low_power_start_date_time.Value)
			{
				last_outage_time_span = end - last_low_power_start_date_time.Value;
			}
			else
			{
				last_outage_time_span = (level.date_time_end - last_low_power_start_date_time.Value) +
						(end - level.date_time_start);
			}
		}

		return last_outage_time_span;
	}
}


public class LevelConfig
{
	public int level_number { get; private set; }
	public string title { get; private set; }
	public string explanation { get; private set; }
	public DateTime date_time_start { get; private set; }
	public DateTime date_time_end { get; private set; }
	public int approx_date_time_step_s { get; private set; }
	public DataTimeWindow data_time_window { get; private set; }

	public LevelConfig(int level_number, string title, string explanation, DateTime date_time_start, DateTime date_time_end, DataTimeWindow data_time_window)
	{
		this.level_number = level_number;
		this.title = title;
		this.explanation = explanation;
		this.date_time_start = date_time_start;
		this.date_time_end = date_time_end;
		this.approx_date_time_step_s = 600; // 10 minutes
		this.data_time_window = data_time_window;
	}

	private static Dictionary<int, LevelConfig> level_configs;

	public static LevelConfig get_level_config(int level)
	{
		make_levels();

		if (level_configs.ContainsKey(level))
		{
			return level_configs[level];
		}
		else
		{
			Debug.LogError("Unsupported game level: " + level);
			return null;
		}
	}

	private static void make_levels()
	{
		if (level_configs != null) return;

		level_configs = new Dictionary<int, LevelConfig>() { };

		add_level(
			"Easy Day",
			"Generate energy for <region> over a Sunday.  For now the wind and sun is averaged over the year.",
			// It will keep repeating the same day until you generate electricity continuously for the whole day.
			new DateTime(2018, 6, 17, 0, 0, 0, DateTimeKind.Utc),
			new DateTime(2018, 6, 18, 0, 0, 0, DateTimeKind.Utc),
			DataTimeWindow.year_average
		);

		add_level(
			"Intermittent Day",
			"Same day in <region> but wind and sun now vary by the hour instead of average over the year.", //  You'll need storage if using renewables.",
			new DateTime(2018, 6, 17, 0, 0, 0, DateTimeKind.Utc),
			new DateTime(2018, 6, 18, 0, 0, 0, DateTimeKind.Utc),
			DataTimeWindow.hourly
		);

		add_level(
			"Summer Week",
			"A full week in <region>.  Energy usage during the week will be higher than the weekend.",
			new DateTime(2018, 6, 16, 0, 0, 0, DateTimeKind.Utc),
			new DateTime(2018, 6, 23, 0, 0, 0, DateTimeKind.Utc),
			DataTimeWindow.hourly
		);

		add_level(
			"Winter Week",
			"A full week in <region>.  Energy usage during this winter week will be different than the summer week.",
			new DateTime(2018, 12, 15, 0, 0, 0, DateTimeKind.Utc),
			new DateTime(2018, 12, 22, 0, 0, 0, DateTimeKind.Utc),
			DataTimeWindow.hourly
		);

		add_level(
			"2018",
			"A full year in <region>.  This may start to really test your long term storage solutions.",
			new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			// temporary hack to deal with not having hour of data in 2019
			new DateTime(2018, 12, 31, 23, 0, 0, DateTimeKind.Utc),
			DataTimeWindow.hourly
		);
	}

	private static int next_level = 1;

	private static void add_level(string title, string explanation, DateTime date_time_start, DateTime date_time_end, DataTimeWindow data_time_window)
	{
		var level = next_level++;
		var full_title = "Level " + level + ": " + title;
		var config = new LevelConfig(level, full_title, explanation, date_time_start, date_time_end, data_time_window);
		level_configs.Add(level, config);
	}

	internal static bool have_level_config(int level_number)
	{
		return level_number <= level_configs.Count;
	}
}

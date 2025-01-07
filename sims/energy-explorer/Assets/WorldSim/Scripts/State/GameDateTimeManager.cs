using System;
using UnityEngine;

public class ChangedDateTime
{
	public DateTime previous_date_time { get; private set; }
	public DateTime new_date_time { get; private set; }
	public TimeSpan elapsed_time { get; private set; }
	public float elapsed_time_s { get; private set; }

	public ChangedDateTime(DateTime previous_date_time, DateTime new_date_time)
	{
		this.previous_date_time = previous_date_time;
		this.new_date_time = new_date_time;
		elapsed_time = new_date_time - previous_date_time;
		elapsed_time_s = (float)elapsed_time.TotalSeconds;
	}

	public bool includes(DateTime dt)
	{
		return previous_date_time < dt && dt <= new_date_time;
	}

	public override string ToString()
	{
		return previous_date_time.ToString() + " - " + new_date_time.ToString();
	}
}

public class GameDateTimeManager : MonoBehaviour
{
	public delegate void ChangedSimDateTime(ChangedDateTime change);
	public event ChangedSimDateTime on_changed_sim_date_time;
	public delegate void ChangedGameDateTime(DateTime new_datetime);
	public event ChangedGameDateTime on_changed_game_date_time;
	public delegate void ChangedSpeed(int new_speed_int, bool paused);
	public event ChangedSpeed on_changed_speed;

	public DateTime game_date_time { get; private set; }

	private const int DEFAULT_SPEED_INT = 5;
	private int _speed_int = DEFAULT_SPEED_INT;
	public int speed_int {
		get { return _speed_int; }
		set { _speed_int = Mathf.Clamp(value, 0, 9); }
	}
	private float speed { get { return 3 * Mathf.Pow(10, ((speed_int - 1f) / 2f) + 1f); } }
	public bool paused { get; private set; }

	private DateTime sim_date_time;
	private DateTime sim_target_date_time;
	private static float SIM_MAXIMUM_TIME_STEP_S = 3600;
	private const float SIM_MINIMUM_TIME_STEP_S = 300;

	private bool started_time_period = false;
	private LevelsManager levels;
	private LevelConfig current_level;

	private void Awake()
	{
		//game_date_time = DateTime.UtcNow; // power_demand_data.start_datetime();
		paused = false;

		// change to true to run tests
		if (false) PowerPlantOperating.TestPowerPlantOperating.test();
	}

	private void Start()
	{
		levels = GameManagerWorldSim.levels;
		levels.on_changed_level_active += handle_changed_level_active;
	}

	private void Update()
	{
		var change_game_date_time = !paused && speed_int != 0 && levels.level_active && current_level != null;
		if (change_game_date_time)
		{
			update_game_date_time();
			update_sim_date_time();
			on_changed_game_date_time?.Invoke(game_date_time);
		}
	}

	private void handle_changed_level_active(bool level_active, LevelConfig level_config)
	{
		current_level = level_config;
		if (level_active)
		{
			game_date_time = current_level.date_time_end;
			change_speed(DEFAULT_SPEED_INT - speed_int);
		}
	}

	private void update_game_date_time()
	{
		started_time_period = game_date_time >= current_level.date_time_end;

		if (started_time_period)
		{
			game_date_time = current_level.date_time_start;
		}
		else
		{
			game_date_time = game_date_time.AddSeconds(Time.deltaTime * speed);
			if (game_date_time > current_level.date_time_end)
			{
				game_date_time = current_level.date_time_end;
			}
		}
	}

	private void update_sim_date_time()
	{
		if (started_time_period)
		{
			// TODO could add code to make sure we do a whole time period
			//if (sim_target_date_time != null)
			//{

			//}

			sim_date_time = game_date_time;
			sim_target_date_time = game_date_time.AddSeconds(SIM_MAXIMUM_TIME_STEP_S);
		}
		else if (sim_target_date_time > game_date_time)
		{
			if ((game_date_time - sim_date_time).TotalSeconds >= SIM_MINIMUM_TIME_STEP_S)
			{
				var change = new ChangedDateTime(previous_date_time: sim_date_time, new_date_time: game_date_time);
				on_changed_sim_date_time?.Invoke(change);
				sim_date_time = game_date_time;
			}
		}
		else while (sim_target_date_time <= game_date_time)
		{
			var change = new ChangedDateTime(previous_date_time: sim_date_time, new_date_time: sim_target_date_time);
			on_changed_sim_date_time?.Invoke(change);
			sim_date_time = sim_target_date_time;
			sim_target_date_time = sim_target_date_time.AddSeconds(SIM_MAXIMUM_TIME_STEP_S);
		}
	}

	internal void change_speed(int change_speed_int)
	{
		var initial_speed = speed_int;
		speed_int += change_speed_int;
		var changed = initial_speed != speed_int;
		if (changed) on_changed_speed?.Invoke(speed_int, paused);
	}

	internal void toggle_paused()
	{
		paused = !paused;
		on_changed_speed?.Invoke(speed_int, paused);
	}
}

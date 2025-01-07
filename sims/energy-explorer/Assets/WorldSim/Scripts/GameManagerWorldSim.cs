using System;
using UnityEngine;
using WPM;

public partial class GameManagerWorldSim : MonoBehaviour
{
	public static GameManagerWorldSim instance { get; private set; }
	public static KeyboardShortcuts input_keyboard { get; private set; }
	public static GameDateTimeManager date_time { get; private set; }
	public static LevelsManager levels { get; private set; }
	public static CellsVisualsManager cell_visuals { get; private set; }
	public static PowerManager power { get; private set; }

	public bool skip_slow_transitions = false;
	public float animation_speed = 2;
	public bool on_start_show_menu = true;

	private WorldMapGlobe map;

	private void Awake()
	{
		instance = this;

		input_keyboard = gameObject.AddComponent<KeyboardShortcuts>();
		date_time = gameObject.AddComponent<GameDateTimeManager>();
		cell_visuals = gameObject.AddComponent<CellsVisualsManager>();
		power = gameObject.AddComponent<PowerManager>();
		levels = new LevelsManager(power, date_time);
	}

	void Start()
    {
		map = WorldMapGlobe.instance;
	}

	private bool hack_selected_player_role_automatically = false;
	private void Update()
	{
		if (!hack_selected_player_role_automatically)
		{
			hack_selected_player_role_automatically = true;
			chosen_role(UserRole.player, this);
		}
	}

	internal Vector3 get_position_of_cursor { get { return map.cursorLocation; } }
	internal Vector2 get_lat_lon_of_cursor { get { return Conversion.GetLatLonFromSpherePoint(map.cursorLocation); } }
	private Quaternion rotate_cursor_normal = Quaternion.Euler(90, 0, 0);

	internal Quaternion get_normal_at_cursor { get { return Quaternion.LookRotation(get_position_of_cursor) * rotate_cursor_normal; } }

	#region Main UI Options for region and user role
	public delegate void ChangedSelectedUserRole(UserRole previous_user_role, UserRole user_role, object source);
	public event ChangedSelectedUserRole on_changed_selected_user_role;
	public delegate void ChangedSelectedRegion(RegionOfInterest previous_region, RegionOfInterest region, object source);
	public event ChangedSelectedRegion on_changed_selected_region;

	public UserRole selected_user_role { get; private set; }
	public RegionOfInterest selected_region_of_interest { get; private set; }
	public void chosen_role(UserRole new_role, object source)
	{
		on_changed_selected_user_role?.Invoke(selected_user_role, new_role, source);
		selected_user_role = new_role;
	}

	public void chosen_region(RegionOfInterest new_region_of_interest, object source)
	{
		on_changed_selected_region?.Invoke(selected_region_of_interest, new_region_of_interest, source);
		selected_region_of_interest = new_region_of_interest;
	}
	#endregion
}

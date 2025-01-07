using UnityEngine;

public partial class UIManager : MonoBehaviour
{
	// Dependencies on other game objects
	private GameObject level_start_container;
	private GameObject level_end_container;
	private GameObject levels_completed_container;

	private void Start_levels_ui_manager()
	{
		GameManagerWorldSim.levels.on_changed_level_active += handle_changed_level_active;

		level_start_container = transform.Find("LevelStart").gameObject;
		level_end_container = transform.Find("LevelEnd").gameObject;
		levels_completed_container = transform.Find("LevelsCompleted").gameObject;
	}

	private void handle_changed_level_active(bool level_active, LevelConfig level_config)
	{
		update_full_ui();
	}

	private void hide_levels_ui()
	{
		level_start_container.SetActive(false);
		level_end_container.SetActive(false);
		levels_completed_container.SetActive(false);
	}

	private void show_levels_ui()
	{
		if (!GameManagerWorldSim.levels.have_level())
		{
			levels_completed_container.SetActive(true);
		}
		else if (GameManagerWorldSim.levels.level_success)
		{
			level_end_container.SetActive(true);
		}
		else
		{
			level_start_container.SetActive(true);
		}
	}
}

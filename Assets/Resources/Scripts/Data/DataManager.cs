using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class DataManager : MonoBehaviour
{
	public static DataManager instance { get; private set; }

	public delegate void ChangedRootData(WORLD_SIM_ROOT_DATA data_root);
	public event ChangedRootData on_changed_root_data; // TODO UI needs to subscribe to this to prevent racy conditions
	public delegate void ChangedChosenDataSet(string chosen_data_set);
	public event ChangedChosenDataSet on_changed_chosen_data_set;

	public bool data_root_ready { get { return root_data != null; } }

	public WORLD_SIM_ROOT_DATA root_data { get; private set; }

	public VALUE_REF value_ref_for_chosen_data_set(List<VALUE_REF> value_refs)
	{
		if (chosen_data_set == default) throw new Exception("Data Manager not ready.  Must have fetched data_root first.");

		var chosen = value_refs
			.Where(v => v.data_sets.Contains(chosen_data_set))
			.ToList();

		if (chosen.Count == 0)
		{
			chosen = get_fall_back_value_refs(value_refs);
		}

		if (chosen.Count > 1)
		{
			throw new NotImplementedException("Multiple value_refs matching chosen_data_set: " + chosen_data_set);
		}

		return chosen[0];
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
    {
		FetchRootData.on_fetched_root_data += handle_fetched_root_data;
		var root_data_fetcher = new FetchRootData();
		StartCoroutine(root_data_fetcher.enumerator);
		Start_data_manager_region_data();
	}

	private void handle_fetched_root_data(WORLD_SIM_ROOT_DATA root_data)
	{
		this.root_data = root_data;
		on_changed_root_data?.Invoke(root_data);
		choose_core_release_data_set(root_data);
		update_data_sets_info(root_data);
	}

	public string chosen_data_set { get; private set; }
	private string chosen_data_set_version;
	private List<string> fallback_data_sets;
	private int unsafe_fallback_data_set_index;

	private void choose_core_release_data_set(WORLD_SIM_ROOT_DATA data)
	{
		var core_data_set_config = data.data_set_configs.Find(config => config.name == "core");
		chosen_data_set = "core@" + core_data_set_config.release_version;
		on_changed_chosen_data_set?.Invoke(chosen_data_set);
		//chosen_data_set = "core@" + core_data_set_config.draft_version;
	}

	private void update_data_sets_info(WORLD_SIM_ROOT_DATA data)
	{
		var split_chosen_data_set = chosen_data_set.Split('@');
		var chosen_data_set_name = split_chosen_data_set[0];
		var chosen_data_set_name_with_at = chosen_data_set_name + "@";
		chosen_data_set_version = split_chosen_data_set[1];

		var data_set = data.data_set_configs.Find(v => v.name == chosen_data_set_name);
		var chosen_data_set_index = data_set.versions.FindIndex(version => version == chosen_data_set_version);

		if (chosen_data_set_index < 0)
		{
			if (chosen_data_set_version != data_set.draft_version)
			{
				Debug.LogError("ERROR: chosen data set is " + chosen_data_set + " but this version is not found in " + string.Join(", ", data_set.versions) + " nor is it draft version: " + data_set.draft_version);
			}
			chosen_data_set_index = 0;
		}

		fallback_data_sets = data_set.versions.Skip(chosen_data_set_index).ToList();
		unsafe_fallback_data_set_index = fallback_data_sets.Count;
		var unsafe_fallback_data_sets = data_set.versions.Take(chosen_data_set_index).ToList();

		fallback_data_sets = fallback_data_sets.Concat(unsafe_fallback_data_sets)
			.Append(data_set.draft_version)
			.Select(v => chosen_data_set_name_with_at + v)
			.ToList();
	}

	private List<VALUE_REF> get_fall_back_value_refs(List<VALUE_REF> value_refs)
	{
		var available_value_refs_by_data_set = get_available_value_refs_by_data_set(value_refs);
		var chosen = new List<VALUE_REF>();

		for (int i = 0; i < fallback_data_sets.Count; i++)
		{
			var fallback_data_set = fallback_data_sets[i];
			if (available_value_refs_by_data_set.ContainsKey(fallback_data_set))
			{
				chosen = available_value_refs_by_data_set[fallback_data_set];

				if (i >= unsafe_fallback_data_set_index)
				{
					Debug.LogWarning("Warning: using unsafe data_set " + fallback_data_set + " at index: " + i + " which is >= unsafe_fallback_data_set_index of " + unsafe_fallback_data_set_index + " for fallback data_sets: " + string.Join(", ", fallback_data_sets));
				}
				else
				{
					//Debug.Log("Info: using data_set " + fallback_data_set + " at index: " + i + " which is < unsafe_fallback_data_set_index of " + unsafe_fallback_data_set_index + " for fallback data_sets: " + string.Join(", ", fallback_data_sets));
				}

				break;
			}
		}

		if (chosen.Count == 0)
		{
			var message = "No value_refs matching chosen_data_set: " + chosen_data_set + " nor any fall back dataset: " + string.Join(", ", fallback_data_sets) + ".  value_ref's data sets are: " + string.Join(", ", available_value_refs_by_data_set.Keys);
			Debug.LogWarning("Warning: " + message);
			throw new NotImplementedException(message);
		}

		return chosen;
	}

	private Dictionary<string, List<VALUE_REF>> get_available_value_refs_by_data_set(List<VALUE_REF> value_refs)
	{
		var available_value_refs_by_data_set = new Dictionary<string, List<VALUE_REF>>();

		value_refs.ForEach(value_ref =>
		{
			value_ref.data_sets.ForEach(data_set =>
			{
				if (!available_value_refs_by_data_set.ContainsKey(data_set))
				{
					available_value_refs_by_data_set.Add(data_set, new List<VALUE_REF>() { value_ref });
				}
				else
				{
					available_value_refs_by_data_set[data_set].Add(value_ref);
				}
			});
		});

		return available_value_refs_by_data_set;
	}
}

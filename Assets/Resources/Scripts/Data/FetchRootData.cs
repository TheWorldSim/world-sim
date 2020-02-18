using System;
using System.Text.RegularExpressions;
using UnityEngine;


public class FetchRootData : DataFetcher
{
	public delegate void FetchedRootData(WORLD_SIM_ROOT_DATA data_root);
	public static event FetchedRootData on_fetched_root_data;

	public FetchRootData() : base("data-compact.json") { }

	protected override void changed_status(AsyncStatus status)
	{
		base.changed_status(status);
		//Debug.Log("Status of fetching data root: " + status);
	}

	protected override void fetched_data(string response)
	{
		base.fetched_data(response);
		var root_data = parse_root_data_response(response);
		on_fetched_root_data?.Invoke(root_data);
	}

	public WORLD_SIM_ROOT_DATA parse_root_data_response(string response)
	{
		var schema_version = get_schema_version(response);
		var supported_version = "0.7";
		if (schema_version != supported_version)
		{
			throw new NotImplementedException("Only supports schema version " + supported_version);
		}

		var json_string = response;
		var root_data = JsonUtility.FromJson<WORLD_SIM_ROOT_DATA>(json_string);

		return root_data;
	}

	private string get_schema_version(string response)
	{
		// regex matching against:
		//      {"schema_version": "0.1"
		var rx = new Regex(@"{""schema_version"":""(?<version>[^""]+)", RegexOptions.Compiled | RegexOptions.Multiline);
		var match = rx.Match(response);
		var version = match.Groups["version"].Value;
		// Debug.Log("Got version: " + version);
		return version;
	}
}

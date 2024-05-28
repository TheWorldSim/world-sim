using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum AsyncStatus
{
	not_fetching = 1,
	fetching = 2,
	error = 3,
	success = 4
}


public class DataFetcher
{
	public IEnumerator enumerator { get; private set; }
	public AsyncStatus status { get { return DataFetcherSingleton.status(resource); } }
	public bool complete { get { return status == AsyncStatus.success; } }

	public string resource { get; private set; }

	private static int next_fetcher_id = 0;
	public int fetcher_id { get; private set; }

	public DataFetcher(string resource)
	{
		fetcher_id = next_fetcher_id++;

		DataFetcherSingleton.on_changed_state += handle_changed_status;
		this.resource = resource;

		if (DataFetcherSingleton.already_fetching(resource))
		{
			Debug.LogWarning("Not fetching " + resource + " again");
			return;
		}

		enumerator = DataFetcherSingleton.fetch(resource, fetcher_id);
	}

	private void handle_changed_status(string resource, int fetcher_id, string response, AsyncStatus status)
	{
		if (this.resource == resource && this.fetcher_id == fetcher_id)
		{
			changed_status(status);
			if (status == AsyncStatus.success) fetched_data(response);

			if (status == AsyncStatus.success || status == AsyncStatus.error)
			{
				DataFetcherSingleton.on_changed_state -= handle_changed_status;
				// todo check object destroyed
			}
		}
	}

	protected virtual void changed_status(AsyncStatus status)
	{
		// subclasses can override this
	}

	protected virtual void fetched_data(string response)
	{
		// subclasses can override this
	}


	/**********************************************
	 * 
	 *            DataFetcherSingleton
	 * 
	 **********************************************/
	private static class DataFetcherSingleton
	{
		public delegate void ChangedState(string resource, int fetcher_id, string response, AsyncStatus status);
		public static event ChangedState on_changed_state;

		public static bool already_fetching(string resource)
		{
			return statuses.ContainsKey(resource);
		}

		public static IEnumerator fetch(string resource, int fetcher_id)
		{
			statuses[resource] = AsyncStatus.not_fetching;

			on_changed_state(resource, fetcher_id, null, statuses[resource]);

			string response = load_last_data(resource);

			bool fresh_download = false;
			if (response == null)
			{
				yield return get_request(resource, fetcher_id);

				if (statuses[resource] == AsyncStatus.success)
				{
					response = responses[resource];
					fresh_download = true;
				}
			}
			else
			{
				// Debug.Log("Used cached version of " + resource);
				statuses[resource] = AsyncStatus.success;
			}

			if (response == "")
			{
				throw new MissingReferenceException("Data unavailable for " + resource);
			}
			else
			{
				if (fresh_download && statuses[resource] == AsyncStatus.success) save_data(resource, response);
				// Debug.Log("Calling on_changed_data for resource " + resource);
				on_changed_state(resource, fetcher_id, response, status: statuses[resource]);
			}
		}

		private static string base_url = "https://raw.githubusercontent.com/TheWorldSim/world-sim-data/master/data/";
		private static Dictionary<string, AsyncStatus> statuses = new Dictionary<string, AsyncStatus>();
		private static Dictionary<string, string> responses = new Dictionary<string, string>();

		private static IEnumerator get_request(string resource, int fetcher_id)
		{
			var uri = base_url + resource;
			using (UnityWebRequest web_request = UnityWebRequest.Get(uri))
			{
				statuses[resource] = AsyncStatus.fetching;
				on_changed_state(resource, fetcher_id, response: null, status: statuses[resource]);

				// Request and wait for the desired page.
				yield return web_request.SendWebRequest();

				string response;
				if (web_request.isNetworkError)
				{
					statuses[resource] = AsyncStatus.error;
					Debug.Log("Error: " + web_request.error + " whilst fetching: " + uri);
					response = web_request.error;
				}
				else
				{
					statuses[resource] = AsyncStatus.success;
					// Debug.Log("Received: " + web_request.downloadHandler.text.Length + " characters whilst fetching: " + uri);
					response = web_request.downloadHandler.text;
				}

				responses.Add(resource, response);
			}
		}

		public static AsyncStatus status(string resource)
		{
			var status = AsyncStatus.not_fetching;
			if (statuses.TryGetValue(resource, out status))
			{
				return status;
			}
			return status;
		}

		private static string file_location(string resource)
		{
			return ".\\data-cache\\" + resource.Replace("/", "_l_");
		}

		private static string load_last_data(string resource)
		{
			var file_path = file_location(resource);
			if (System.IO.File.Exists(file_path))
			{
				var data_str = System.IO.File.ReadAllText(file_path);
				// Debug.Log("Got data from file.  Length: " + data_str.Length);
				return data_str;
			}

			Debug.Log("File not found at: " + file_path);
			Debug.Log("Data cache contains files: " + string.Join("\n", System.IO.Directory.GetFiles(".\\data-cache")));

			return null;
		}

		private static void save_data(string resource, string response)
		{
			var file_path = file_location(resource);
			System.IO.File.WriteAllText(file_path, response);
		}
	}
}


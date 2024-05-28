using System;

public abstract class DataFetcherProgress : DataFetcher
{
	private string name;
	private Action handle_event_from_single_download_progress;

	public DataFetcherProgress(string resource, string name) : base(resource)
	{
		this.name = name;
	}

	public string text
	{
		get
		{
			var txt = name + " ";

			if (complete) txt += "100%";
			else if (status == AsyncStatus.fetching) txt += "downloading...";
			else if (status == AsyncStatus.not_fetching) txt += "waiting.";
			else if (status == AsyncStatus.error) txt += "error.";

			return txt;
		}
	}
}

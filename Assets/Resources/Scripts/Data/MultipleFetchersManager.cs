using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class MultipleFetchersManager : MonoBehaviour
{
	protected void start_requesting_data(MultipleFetchersProgress multiple_fetchers_progress)
	{
		StartCoroutine(request_data_async(multiple_fetchers_progress));
	}

	private IEnumerator request_data_async(MultipleFetchersProgress multiple_fetchers_progress)
	{
		while (true)
		{
			yield return new WaitForSeconds(0);
			var data_fetcher = multiple_fetchers_progress.get_next_outstanding_fetcher();
			if (data_fetcher == null) break;

			// Debug.Log("fetching resource: " + data_fetcher.resource);
			StartCoroutine(data_fetcher.enumerator);
		}
	}
}


public enum AsyncStatusMultiple
{
	not_fetching = 1,
	fetching = 2,
	all_finished_one_or_more_errors = 3,
	all_finished_all_succeeded = 4
}


public class MultipleFetchersProgress
{
	public List<DataFetcherProgress> all_data_fetchers { get; private set; }
	private List<DataFetcherProgress> outstanding_data_fetchers;

	public MultipleFetchersProgress()
	{
		all_data_fetchers = new List<DataFetcherProgress>();
		outstanding_data_fetchers = new List<DataFetcherProgress>();
	}

	public void add_data_fetcher(DataFetcherProgress data_fetcher)
	{
		all_data_fetchers.Add(data_fetcher);
		outstanding_data_fetchers.Add(data_fetcher);
	}

	internal DataFetcherProgress get_next_outstanding_fetcher()
	{
		if (outstanding_data_fetchers.Count == 0) return null;
		var data_fetcher = outstanding_data_fetchers[0];
		outstanding_data_fetchers.Remove(data_fetcher); // improve efficiency if needed
		return data_fetcher;
	}

	public List<string> text { get { return all_data_fetchers.Select(p => p.text).ToList(); } }
	public bool complete { get { return all_data_fetchers.Where(p => !p.complete).Count() == 0; } }

	public AsyncStatusMultiple status
	{
		get
		{
			AsyncStatusMultiple st;

			if (not_fetching() > 0) st = AsyncStatusMultiple.not_fetching;
			else if (fetching() > 0) st = AsyncStatusMultiple.fetching;
			else if (errored() > 0) st = AsyncStatusMultiple.all_finished_one_or_more_errors;
			else st = AsyncStatusMultiple.all_finished_all_succeeded;

			return st;
		}
	}

	private int all () => all_data_fetchers.Count;
	private int fetching () => all_data_fetchers.Where(dp => dp.status == AsyncStatus.fetching).Count();
	private int errored () => all_data_fetchers.Where(dp => dp.status == AsyncStatus.error).Count();
	private int success () => all_data_fetchers.Where(dp => dp.status == AsyncStatus.success).Count();
	private int not_fetching () => all() - (fetching() + errored() + success());

	public override string ToString()
	{
		return "Progress " + (complete ? "" : "in") + "complete " + success() + "/" + all() + " not_fetching " + not_fetching() + " fetching " + fetching() + " errored " + errored();
	}
}

using TMPro;
using UnityEngine;

public class LoadingScreenUI : MonoBehaviour
{
	private Transform progress_text;
	private Transform line_of_text;
	private Transform lines;

	private void OnEnable()
	{
		DataManager.instance.on_changed_region_data_fetch_progress += handle_changed_region_data_fetch_progress;
		progress_text = transform.Find("ProgressText");
		line_of_text = progress_text.Find("LineOfText");
		line_of_text.gameObject.SetActive(true);
		lines = progress_text.Find("Lines");
	}

	void Start()
    {
	}

	private void OnDisable()
	{
		DataManager.instance.on_changed_region_data_fetch_progress -= handle_changed_region_data_fetch_progress;
		//clear_all_lines();
	}

	private void clear_all_lines()
	{
		var i = 0;
		var all_children = new GameObject[lines.childCount];
		foreach (Transform child in lines)
		{
			all_children[i] = child.gameObject;
			i += 1;
		}

		foreach (GameObject child in all_children)
		{
			Destroy(child.gameObject);
		}
	}

	private void handle_changed_region_data_fetch_progress(MultipleFetchersProgress progress)
	{
		line_of_text.gameObject.SetActive(false);

		for (int i = 0; i < progress.all_data_fetchers.Count; i++)
		{
			var df = progress.all_data_fetchers[i];
			create_or_update(df, i);
		}
	}

	private void create_or_update(DataFetcherProgress df, int index)
	{
		var line = line_present(df);
		if (line == null)
		{
			line = Instantiate(line_of_text, lines, false);
			var rt = line.GetComponent<RectTransform>();
			var ap = rt.anchoredPosition;
			ap.y = -index * 16;
			rt.anchoredPosition = ap;
			line.name = "fetcher-" + df.fetcher_id;
			line.gameObject.SetActive(true);
		}

		line.GetComponent<TextMeshProUGUI>().text = df.text;

		var color = new Color(1, 1, 0);
		if (df.status == AsyncStatus.fetching) color = new Color(1, 1, 1);
		if (df.status == AsyncStatus.success) color = new Color(0, 0.9f, 0);
		if (df.status == AsyncStatus.error) color = new Color(0.8f, 0, 0);
		line.GetComponent<TextMeshProUGUI>().color = color;
	}

	private Transform line_present(DataFetcherProgress df)
	{
		return lines.Find("fetcher-" + df.fetcher_id);
	}
}

using TMPro;
using UnityEngine;
using WPM;

public class CitySymbol : MonoBehaviour
{
	public bool is_capital;
	public int population = 100000;
	private TextMeshPro label;
	internal bool is_region_capital;

	void Start()
	{
		label = transform.Find("Label").GetComponent<TextMeshPro>();
		label.text = gameObject.name;

		WorldMapGlobe.instance.OnZoomChange += handle_zoom_change;

		// move height from lower at <= 100,000 pop to higher at >= 1 million
		var height = Mathf.Lerp(-3, -2, (population - 100000) / 900000f);
		var buildings = transform.Find("Buildings").transform;
		buildings.localPosition = new Vector3(buildings.localPosition.x, height, buildings.localPosition.z);

		// scale building size (cubic) > 1 million pop
		float scale_height = population > 1000000 ? Mathf.Pow(population / 1000000f, 0.333f) : 1;
		buildings.localScale *= scale_height;
	}


	private static float MIN_Z = -0.05f;
	private static float MID_Z = 0.06f;
	private static float MAX_Z = 0.5f;
	private static float MIN_Z_FADE = 0f;
	private static float MAX_Z_FADE = 0.1f;
	private void handle_zoom_change(float zoom_level)
	{
		var z1_clamp = Mathf.Clamp(zoom_level, MID_Z, MAX_Z);
		var z1 = (z1_clamp - MID_Z) / (MAX_Z - MID_Z);
		float scale = Mathf.Lerp(0, 8, z1);

		var z2_clamp = Mathf.Clamp(zoom_level, MIN_Z, MID_Z);
		var z2 = (z2_clamp - MIN_Z) / (MID_Z - MIN_Z);
		scale += Mathf.Lerp(0.5f, 2, z2);

		if (!is_capital)
		{
			var z3_clamp = Mathf.Clamp(zoom_level, MIN_Z_FADE, MAX_Z_FADE);
			var z3 = (z3_clamp - MIN_Z_FADE) / (MAX_Z_FADE - MIN_Z_FADE);
			scale *= Mathf.Lerp(1, 0, z3);
		}

		label.transform.localScale = new Vector3(1, 1, 1) * scale;
	}

	private void OnDestroy()
	{
		if (WorldMapGlobe.instance != null)
		{
			WorldMapGlobe.instance.OnZoomChange -= handle_zoom_change;
		}
	}
}

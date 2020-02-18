using System;
using UnityEngine;
using WPM;

public class MainCamera : MonoBehaviour
{
	public static MainCamera instance;
	public bool target_zoom_level = false;

	private WorldMapGlobe map;
	private float zoom_level_on_fly = 0.14f;
	private bool animating_intro_zoom = false;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		map = WorldMapGlobe.instance;

		if (!GameManagerWorldSim.instance.skip_slow_transitions)
		{
			Camera.main.fieldOfView = 180;
			animating_intro_zoom = true;
		}

		// make sure clipping plane is correct.  Somehow it's getting reset
		// to 0.1f which in combination with the tilt results in it breaking
		map.mainCamera.nearClipPlane = 0.01f;

		map.OnZoomChange += handle_zoom_change;
	}

	private void handle_zoom_change(float zoom_level)
	{
		var zoom_for_lerp = zoom_level * 2f;
		map.atmosphereAlpha = Mathf.Lerp(0.05f, 0.256f, zoom_for_lerp);
	}

	// Update is called once per frame
	void Update()
    {
		animate_intro_zoom();
		animate_zoom_level_on_flying_to_region();
	}

	private void animate_intro_zoom()
	{
		// Animates the camera field of view (just a "cool" effect at the beginning)
		if (animating_intro_zoom)
		{
			var speed = GameManagerWorldSim.instance.animation_speed;

			var target_field_of_view = 40f;
			var change_field_of_view = Camera.main.fieldOfView > target_field_of_view;
			if (change_field_of_view)
			{
				var subtract = ((181.0f - Camera.main.fieldOfView) / (200.0f - Camera.main.fieldOfView)) * speed;
				Camera.main.fieldOfView -= subtract;
			}
			else Camera.main.fieldOfView = target_field_of_view;

			var target_zoom = 1f;
			var current_zoom = map.GetZoomLevel();
			var diff = current_zoom - target_zoom;
			var change_zoom = diff > 0;
			if (change_zoom)
			{
				var subtract = ((diff + 1) / 45) * speed;
				map.SetZoomLevel(current_zoom - subtract);
			}
			else map.SetZoomLevel(target_zoom);

			animating_intro_zoom = change_field_of_view || change_zoom;
		}
	}

	private void animate_zoom_level_on_flying_to_region()
	{
		if (target_zoom_level)
		{
			float current_zoom_level = map.GetZoomLevel();
			float diff = current_zoom_level - zoom_level_on_fly;

			if (Mathf.Abs(diff) < 1e-3f)
			{
				map.SetZoomLevel(zoom_level_on_fly);
				target_zoom_level = false;
			}
			else
			{
				map.SetZoomLevel(Mathf.Clamp01(current_zoom_level - diff * 0.1f));
			}

			// Cancel our zoom if user zooming
			if (Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				target_zoom_level = false;
			}
		}
	}

	// TODO fix, these have some leaky abstraction.
	//		Implement own flyTo functions
	// When you manipulate the map object somehow or other
	// it stops them from working
	internal void fly_to_country(string country)
	{
		map.FlyToCountry(country);
	}

	internal void fly_to_province(string province)
	{
		map.FlyToProvince(province);
	}
}

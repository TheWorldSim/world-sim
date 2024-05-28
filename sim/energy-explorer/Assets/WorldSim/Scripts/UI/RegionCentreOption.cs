using UnityEngine;
using WPM;
using System.Linq;
using UnityEngine.UI;

public class RegionCentreOption : MonoBehaviour
{
	private Vector2 target_lat_lon;
	private Rect? target_area_rect_2D;
	private WorldMapGlobe map;
	private GameObject button;
	private Image button_image;
	private const float MAX_DISTANCE = 5;

	void Start()
    {
		GameManagerWorldSim.instance.on_changed_selected_region += handle_changed_selected_region;
		map = WorldMapGlobe.instance;

		button = transform.Find("Button").gameObject;
		button_image = button.GetComponent<Image>();
		button.GetComponent<Button>().onClick.AddListener(handle_click);
	}

	void Update()
	{
		if (target_area_rect_2D == null || map.isFlyingToActive)
		{
			button.SetActive(false);
			return;
		}

		var location = Conversion.GetLatLonFromSpherePoint(map.GetCurrentMapLocation());
		var normalised_location = Rect.PointToNormalized(target_area_rect_2D.Value, location);
		var closest_rect_point = Rect.NormalizedToPoint(target_area_rect_2D.Value, normalised_location);
		var distance = (closest_rect_point - location).magnitude;

		// TODO incorporate zoom & earth curvature etc
		if (distance < MAX_DISTANCE) button.SetActive(false);
		else
		{
			button.SetActive(true);
			var alpha = Mathf.Lerp(0, 0.8f, (distance - MAX_DISTANCE) / 3f);
			var c = button_image.color;
			c.a = alpha;
			button_image.color = c;
		}
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		var country = map.countries[region.country_index];
		var provinces = country.provinces.Where((p) => p.name == region.province_name).ToList();
		var province = provinces.Count == 0 ? null : provinces[0];

		target_lat_lon = province == null ? country.latlonCenter : province.latlonCenter;
		target_area_rect_2D = province == null ? country.regionsRect2D : province.regionsRect2D;
	}

	private void handle_click()
	{
		map.FlyToLocation(target_lat_lon);
	}
}

using System.Linq;
using UnityEngine;
using WPM;

public class RegionHighlightManager : MonoBehaviour
{
	private GameManagerWorldSim manager;
	private WorldMapGlobe map;

	private void Start()
	{
		manager = GameManagerWorldSim.instance;
		manager.on_changed_selected_region += handle_changed_selected_region;
		map = WorldMapGlobe.instance;
	}

	private void Update()
	{
		animate_region_selection();
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		if (previous_region != null && previous_region.province_name_exists())
		{
			map.GetCountry(previous_region.country_name).provinces.ToList().ForEach(province =>
			{
				map.ToggleProvinceSurface(previous_region.country_name, province.name, false, Colours.default_province_fill_colour);
			});
		}

		if (region != default)
		{
			animation_progress__area_fill = 0;
			animating_area_fill = true;
		}
	}

	private bool animating_area_fill;
	private float animation_progress__area_fill = 0;
	private void animate_region_selection()
	{
		if (animating_area_fill)
		{
			var selected_country_name = manager.selected_region_of_interest.country_name;
			var selected_province_name = manager.selected_region_of_interest.province_name;

			float green_until = 0.5f;
			float fade_others_grey_after = 0.75f;

			if (animation_progress__area_fill > fade_others_grey_after)
			{
				var denom = (1 - fade_others_grey_after);
				var t = (animation_progress__area_fill - fade_others_grey_after) / denom;
				var grey_colour = new Color(0f, 0f, 0f, Mathf.Lerp(0, 0.8f, t));
				// Should end up at Colours.non_roi_fill_colour

				fill_non_selected_regions(grey_colour);
			}

			var alpha = (animation_progress__area_fill <= green_until)
				? animation_progress__area_fill / green_until
				: (1 - animation_progress__area_fill) / (1 - green_until);
			var colour = new Color(0, 1, 0, alpha);
			if (selected_province_name == "") map.ToggleCountrySurface(selected_country_name, true, colour);
			else map.ToggleProvinceSurface(selected_country_name, selected_province_name, true, colour);

			if (animation_progress__area_fill >= 1)
			{
				animating_area_fill = false;
			}
			else
			{
				animation_progress__area_fill = Mathf.Clamp01(animation_progress__area_fill + 0.012f);
			}
		}
	}

	private void fill_non_selected_regions(Color colour)
	{
		var selected_country_name = manager.selected_region_of_interest.country_name;
		var selected_province_name = manager.selected_region_of_interest.province_name;

		map.countries.ToList().ForEach(country =>
		{
			if (country.name == selected_country_name)
			{
				if (selected_province_name == "") return;
				else
				{
					country.provinces.ToList().ForEach(province =>
					{
						if (province.name != selected_province_name) map.ToggleProvinceSurface(country.name, province.name, true, colour);
					});
				}
			}
			else
			{
				map.ToggleCountrySurface(country.name, true, colour);
			}
		});
	}
}

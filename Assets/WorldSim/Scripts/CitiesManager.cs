using System.Linq;
using UnityEngine;
using WPM;

public class CitiesManager : MonoBehaviour
{
	public GameObject city_prefab;
	private bool show_cities = false;
	private RegionOfInterest selected_region;
	private WorldMapGlobe map;

	void Start()
    {
		map = WorldMapGlobe.instance;
		GameManagerWorldSim.instance.on_changed_selected_user_role += handle_changed_selected_user_role;
		GameManagerWorldSim.instance.on_changed_selected_region += handle_changed_selected_region;
	}

	private void handle_changed_selected_user_role(UserRole previous_user_role, UserRole user_role, object source)
	{
		show_cities = user_role == UserRole.player;
		update_cities_visual();
	}

	private void handle_changed_selected_region(RegionOfInterest previous_region, RegionOfInterest region, object source)
	{
		selected_region = region;
		update_cities_visual();
	}

	private void update_cities_visual()
	{
		delete_all_cities();
		if (show_cities && selected_region != null)
		{
			var cities_to_show = map.cities.Where(
				city => city.countryIndex == selected_region.country_index &&
				(selected_region.province_name == "" || city.province == selected_region.province_name)).ToList();

			cities_to_show.OrderBy((c) => c.name)
				.ToList()
				.ForEach(city =>
			{
				var position = Conversion.GetSpherePointFromLatLon(city.latlon);
				var rotation = Quaternion.LookRotation(position) * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, 180, 0);
				var city_object = Instantiate(city_prefab, position, rotation, transform);
				city_object.name = city.name;
				var city_symbol = city_object.GetComponent<CitySymbol>();
				city_symbol.population = city.population;
				city_symbol.is_capital = city.cityClass == CITY_CLASS.COUNTRY_CAPITAL;
				city_symbol.is_region_capital = city.cityClass == CITY_CLASS.REGION_CAPITAL;
				city_object.transform.localScale *= 0.0005f;
			});
		}
	}

	private void delete_all_cities()
	{
		var i = 0;
		var all_children = new GameObject[transform.childCount];
		foreach (Transform child in transform)
		{
			all_children[i] = child.gameObject;
			i += 1;
		}

		foreach (GameObject child in all_children)
		{
			Destroy(child.gameObject);
		}
	}
}

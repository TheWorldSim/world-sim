using System;
using System.Linq;
using HighlightPlus;
using UnityEngine;

[RequireComponent(typeof(HighlightTrigger))]
public class PowerPlant : MonoBehaviour
{
	private PowerPlantData data;
	//private bool added_self = false;

	internal void Init(PowerPlantData data)
	{
		if (this.data != null) throw new Exception("Can not Init PowerPlant more than once");

		this.data = data;

		var building_specific_scale = (data.building_type == BuildingType.wind_farm ? 5f : 1f);
		transform.localScale *= (building_specific_scale * 0.0001f);
		transform.position = WorldMapGlobeWrapper.latlon_to_world_pos(data.lon_lat);

		var outline = data.outline.Select(l => WorldMapGlobeWrapper.latlon_to_world_pos(l)).ToArray();
		if (outline.Length >= 2)
		{
			var boundary_cuboid = transform.Find("BoundaryCuboid");
			if (boundary_cuboid != default)
			{
				var diff = outline[1] - outline[0];
				var scale = (diff.magnitude * 6800f) / building_specific_scale; // TODO approximation
				//var half = diff * 0.5f;

				//var scale = Maths.Divide(new Vector3(scale1d) boundary_cuboid.lossyScale;
				boundary_cuboid.localScale = new Vector3(scale, 0.1f, scale);
			}
		}
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log(" " + data.lon_lat.lat + " " + data.lon_lat.lon);
		}
	}
}

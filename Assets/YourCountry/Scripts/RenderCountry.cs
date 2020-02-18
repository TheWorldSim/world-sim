using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TerrainViewType
{
	terrain,
	wind,
	solar,
}
/*
public class RenderCountry : MonoBehaviour
{
	public TerrainViewType view_type = TerrainViewType.terrain;
	public bool interpolate_terrain = true;

	private GameObject mesh_obj;
	private Mesh mesh;
	private Material country_heat_map_material;
	private Material country_terrain_image_material;
	private int counter = 0;
	//private float[] data;
	private Region region = new USA_Texas_9300m();
	private Color[] country_texture_colours = new Color[]
		{
			new Color(0, 0, 0, 0.3f),  // no data
			new Color(1.00f, 1.00f, 0.00f, 1f), // <5.6 m s-1
			new Color(1.00f, 0.80f, 0.00f, 1f),  // 5.6 - 6.5
			new Color(1.00f, 0.65f, 0.00f, 1f), // 6.5 - 7
			new Color(1.00f, 0.30f, 0.00f, 1f), // 7 - 7.5
			new Color(0.85f, 0.20f, 0.50f, 1f), // 7.5 - 8
			new Color(0.75f, 0.35f, 1.00f, 1f), // 8 - 9
			new Color(0.60f, 0.85f, 1.00f, 1f), // 9+
		};

	private void Awake ()
	{
		refresh();
	}

	private void Start()
	{
		GameManagerYourCountry.BuildingActiveChanged += handle_building_active_change;
	}

	public void refresh ()
	{
		make_object();
		// TODO figure out why we have to call this twice
		set_material_texture();
		set_material_texture();
		construct_mesh();
		colour_terrain();
	}

	private void make_object ()
	{
		if (mesh_obj != null)
		{
			Object.DestroyImmediate(mesh_obj);
		};
		mesh_obj = new GameObject("country_mesh" + (counter++));
		mesh_obj.layer = 8;
		mesh_obj.transform.SetParent(transform, false);
		mesh_obj.AddComponent<MeshRenderer>();
		var mesh_filter = mesh_obj.AddComponent<MeshFilter>();
		mesh = new Mesh();
		mesh_filter.sharedMesh = mesh;
		mesh_obj.AddComponent<MeshCollider>();
	}

	private void set_material_texture ()
	{
		country_heat_map_material = Resources.Load<Material>("Material/CountryMaterial");
		Texture2D country_heat_map_texture = new Texture2D(8, 1, TextureFormat.RGBA32, false);
		country_heat_map_texture.SetPixels(country_texture_colours);
		country_heat_map_material.SetTexture("_country_texture", country_heat_map_texture);

		country_terrain_image_material = Resources.Load<Material>("Material/CountryTerrainImageMaterial");
	}

	private void construct_mesh ()
	{
		Coord[] coords = region.get_area_coords();
		var area_stats = region.get_area_stats();
		int max = coords.Length;
		var interpolate_mod = 4; // interpolate_terrain ? 1 : 4;
		Vector3[] vertices = new Vector3[max * interpolate_mod]; // * 4 for extra vertices
		int[] triangles = new int[max * 2 * 3];

		//Debug.Log("data.len " + data.Length + " sqroot " + Mathf.Sqrt(data.Length) + " sq_max " + sq_max + " vertices.len " + vertices.Length);

		int tri_index = 0;
		for (int i = 0; i < max; ++i)
		{
			var coord = coords[i];
			int i4 = i * interpolate_mod; // * 4 to account for extra vertices
			vertices[i4] = new Vector3(coord.x, 0, -coord.y + 1);
			vertices[i4 + 1] = new Vector3(coord.x + 1, 0, -coord.y + 1);
			vertices[i4 + 2] = new Vector3(coord.x, 0, -coord.y);
			vertices[i4 + 3] = new Vector3(coord.x + 1, 0, -coord.y);

			//Debug.Log(" " + i4 + " " + (i4 + 1) + " " + (i4 + 2));
			triangles[tri_index] = i4;
			triangles[tri_index + 1] = i4 + 1;
			triangles[tri_index + 2] = i4 + 3;

			triangles[tri_index + 3] = i4;
			triangles[tri_index + 4] = i4 + 3;
			triangles[tri_index + 5] = i4 + 2;

			tri_index += 6;
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}

	private void colour_terrain ()
	{
		mesh_obj.GetComponent<MeshRenderer>().sharedMaterial =
			view_type == TerrainViewType.terrain ? country_terrain_image_material : country_heat_map_material;

		var area_stats = region.get_area_stats();
		Coord[] coords = region.get_area_coords();
		int max = coords.Length;
		var interpolate_mod = 4; // interpolate ? 1 : 4;  // * 4 to account for extra vertices
		Vector2[] uv = new Vector2[max * interpolate_mod];
		Dictionary<float, Dictionary<float, Vector2>> neighbouring_uv = new Dictionary<float, Dictionary<float, Vector2>>();

		for (int i = 0; i < max; ++i)
		{
			var coord = coords[i];

			Vector2 vec00;
			Vector2 vec01;
			Vector2 vec10;
			Vector2 vec11;
			if (view_type == TerrainViewType.terrain)
			{
				vec11 = get_terrain_image_colour(coord, area_stats);
			}
			else
			{
				vec11 = get_terrain_data_colour(coord);
			}

			if (interpolate_terrain)
			{
				if (!neighbouring_uv.ContainsKey(coord.x))
				{
					neighbouring_uv[coord.x] = new Dictionary<float, Vector2>();
				}
				neighbouring_uv[coord.x][coord.y] = vec11;
				vec00 = get_neighbouring_terrain_colour(coord.minus_x().minus_y(), neighbouring_uv, vec11);
				vec10 = get_neighbouring_terrain_colour(coord.minus_x(), neighbouring_uv, vec11);
				vec01 = get_neighbouring_terrain_colour(coord.minus_y(), neighbouring_uv, vec11);
			}
			else
			{
				vec00 = vec01 = vec10 = vec11;
			}

			int i4 = i * interpolate_mod; // * 4 to account for extra vertices
			uv[i4] = vec00;
			uv[i4 + 1] = vec01;
			uv[i4 + 2] = vec10;
			uv[i4 + 3] = vec11;
		}

		mesh.uv = uv;
	}

	private Vector2 get_terrain_image_colour (Coord coord, AreaStats area_stats)
	{
		float u = coord.x / area_stats.max_x;
		float v = (area_stats.max_y - coord.y) / area_stats.max_y;
		return new Vector2(u, v);
	}

	private Vector2 get_terrain_data_colour (Coord coord)
	{
		float u = view_type == TerrainViewType.wind ? region.potential_wind_W_per_m2_normalised(coord) : region.potential_solar_W_per_m2_normalised(coord);
		return new Vector2(u, 0);
	}

	private Vector2 get_neighbouring_terrain_colour (Coord coord, Dictionary<float, Dictionary<float, Vector2>> neighbouring_uv, Vector2 default_uv)
	{
		return (!neighbouring_uv.ContainsKey(coord.x) || !neighbouring_uv[coord.x].ContainsKey(coord.y)) ? default_uv : neighbouring_uv[coord.x][coord.y];
	}

	private void handle_building_active_change(object obj, BuildingType building_type)
	{
		var new_view_type = TerrainViewType.terrain;
		if (building_type == BuildingType.wind_farm)
		{
			new_view_type = TerrainViewType.wind;
		}
		else if (building_type == BuildingType.solar_panels)
		{
			new_view_type = TerrainViewType.solar;
		}
		
		if (new_view_type != view_type)
		{
			view_type = new_view_type;
			colour_terrain();
		}
	}
}
*/
using UnityEngine;

public interface ElevationDataContainer
{
	(int, int) shape();
	float get_normalized_data(int index);
	float elevation(Vector3 point_on_unit_sphere, float terrain_multiplier);
}

public class ElevationDataMiniWorld : ElevationDataContainer
{
	public (int, int) shape()
	{
		return (5, 20);
	}

	public float get_normalized_data(int index)
	{
		return data[index];
	}

	public float elevation(Vector3 point_on_unit_sphere, float terrain_multiplier)
	{
		return ElevationData.elevation(point_on_unit_sphere, terrain_multiplier, this);
	}

	private static float[] data = new float[]
	{
		1, -1, -1, -1, -1,
		1, -1, -1, -1, -1,
		1, -1, -1, -1, -1,
		1, -1, -1, -1, -1,
		1, -1, -1, -1, -1,

		1, -1, -1,  1, -1,
		1, -1,  1,  1, -1,
		1,  1, -1,  1, -1,
		1, -1, 10,  1, -1,
		1, -1, -1, -1, -1,

		1, -1, -1, -1, -1,
		1, -1, -1, -1, -1,
		1,  1,  1,  1, -1,
		1,  1,  1,  1, -1,
		1, -1,  1,  1, -1,

		1, -1,  1,  1, -1,
		1,  1,  1,  1,  10,
		1,  1,  1,  1, -1,
		1,  1,  1,  1, -1,
		1,  1, -1, -1, -1,
	};
}

public class ElevationData270kmWorld : ElevationDataContainer
{
	public (int, int) shape()
	{
		// lat by lon
		// 270km is 144 (lat) by 288 (lon) total 41472
		return (144, 288);
	}

	public float get_normalized_data(int index)
	{
		float v = ElevationData270km.data[index];
		// TODO scale the data before it gets to being in C#
		return v / 12000f;
	}

	public float elevation(Vector3 point_on_unit_sphere, float terrain_multiplier)
	{
		return ElevationData.elevation(point_on_unit_sphere, terrain_multiplier, this);
	}
}

public class ElevationData90kmWorld : ElevationDataContainer
{
	public (int, int) shape()
	{
		// lat by lon
		return (432, 864);
	}

	public float get_normalized_data(int index)
	{
		float v = ElevationData90km.data[index];
		// TODO scale the data before it gets to being in C#
		return v / 12000f;
	}

	public float elevation(Vector3 point_on_unit_sphere, float terrain_multiplier)
	{
		return ElevationData.elevation(point_on_unit_sphere, terrain_multiplier, this);
	}
}

static class ElevationData
{
	static public float elevation(Vector3 point_on_unit_sphere, float terrain_multiplier, ElevationDataContainer edc)
	{
		//var polar = Maths.Cartesian_to_polar_ratio(point_on_unit_sphere);
		//var lat1 = polar.lat;
		//var lon1 = polar.lon;
		var lat1 = 0;
		var lon1 = 0;

		var (lat, lon) = edc.shape();

		int x = (int)Mathf.Floor((0.5f - lat1) * (lat - 1));
		int y = (int)Mathf.Floor((1 - lon1) * 0.5f * (lon - 1)) * lat;
		int x1 = Mathf.Min(x + 1, lat - 1);
		int y1 = (y + lat) % (lat * (lon - 1));
		
		// TODO add linear interpolation
		float v1 = edc.get_normalized_data(x + y);
		float v2 = edc.get_normalized_data(x1 + y);
		float v3 = edc.get_normalized_data(x + y1);
		float v4 = edc.get_normalized_data(x1 + y1);

		float elevation = (v1 + v2 + v3 + v4) / 4;

		// Debug.Log("x " + x + " y " + y + " x1 " + x1 + " y1 " + y1 + "      v1 " + v1 + " v2 " + v2 + " v4 " + v3 + " v5 " + v4 + " elevation " + elevation);

		return 1 + (elevation * terrain_multiplier);
	}
}

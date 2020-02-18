using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LonLat
{
	public float lon { get; private set; }
	public float lat { get; private set; }

	public LonLat(float lon, float lat)
	{
		this.lon = lon;
		this.lat = lat;
	}

	public LonLat(Vector2 lon_lat)
	{
		lon = lon_lat.x;
		lat = lon_lat.y;
	}

	//public LonLat(Vector3 game_coords)
	//{
	//	// Texas specific for now
	//	// opposite of to_game_vec3 function
	//	lon = scale_texas_x_to_lon.scale(game_coords.x);
	//	lat = scale_texas_z_to_lat.scale(game_coords.z);
	//}

	public Vector2 to_vec2()
	{
		return new Vector2(lat, lon);
	}

	public LonLat minus(LonLat other)
	{
		return new LonLat(
			lon: lon - other.lon,
			lat: lat - other.lat
		);
	}

	public LonLat plus(LonLat other)
	{
		return new LonLat(
			lon: lon + other.lon,
			lat: lat + other.lat
		);
	}

	public LonLat multiple(float scalar)
	{
		return new LonLat(
			lon: lon * scalar,
			lat: lat * scalar
		);
	}

	internal static LonLat from_vec2(Vector2 latlon)
	{
		return new LonLat(lat: latlon[0], lon: latlon[1]);
	}

	//	// Texas specific
	//	// longtitude min: -106.644743  (31.895486, -106.644743)
	//	// longtitude max: -93.531711   (31.179743,  -93.531711)
	//	// latitude min:    25.837238   (25.837238,  -97.394442)
	//	// latitude max:    36.500292   (36.500292, -103.041836)
	//	// x min: -60
	//	// x max: 73
	//	// y min: -34
	//	// y max: 91
	//	private static Scaler scale_texas_lon_to_x = new Scaler(-106.644743f, -93.531711f, -60f, 73f);
	//	private static Scaler scale_texas_lat_to_z = new Scaler(25.837238f, 36.500292f, -34f, 91f);
	//	private static Scaler scale_texas_x_to_lon = new Scaler(-60f, 73f, -106.644743f, -93.531711f);
	//	private static Scaler scale_texas_z_to_lat = new Scaler(-34f, 91f, 25.837238f, 36.500292f);

	//	public Vector3 to_game_vec3()
	//	{
	//		// Texas specific for now
	//		float x = scale_texas_lon_to_x.scale(lon);
	//		float z = scale_texas_lat_to_z.scale(lat);
	//		return new Vector3(x, 0, z);
	//	}
}


public class Maths
{
    public static Vector3 Translate(Vector3 value, Vector3 add)
    {
        return new Vector3(
            value.x + add.x,
            value.y + add.y,
            value.z + add.z
        );
    }

	internal static Vector3 Divide(Vector3 scale1, Vector3 scale2)
	{
		return new Vector3(
			x: scale2.x != 0 ? scale1.x / scale2.x : 0,
			y: scale2.y != 0 ? scale1.y / scale2.y : 0,
			z: scale2.z != 0 ? scale1.z / scale2.z : 0
		);
	}

	public static Vector3[] Translate(Vector3[] arr, Vector3 add)
    {
        List<Vector3> translated = new List<Vector3>();
        for (int i = 0; i < arr.Length; ++i)
        {
            translated.Add(Translate(arr[i], add));
        }
        return translated.ToArray();
    }

    public static Vector3 Multiply(Vector3 value, float mult)
    {
        return new Vector3(
            value.x * mult,
            value.y * mult,
            value.z * mult
        );
    }
    public static Vector3[] Multiply(Vector3[] arr, float mult)
    {
        List<Vector3> transformed = new List<Vector3>();
        for (int i = 0; i < arr.Length; ++i)
        {
            transformed.Add(Multiply(arr[i], mult));
        }
        return transformed.ToArray();
    }

	//public static LonLat Cartesian_to_polar_radians(Vector3 point)
	//{
	//	var x = point.x;
	//	var y = point.y;
	//	var z = point.z;

	//	//calc longitude
	//	var lon = Mathf.Atan2(x, z);
 
	//	//this is easier to write and read than sqrt(pow(x,2), pow(y,2))!
	//	var xzLen = new Vector2(x, z).magnitude;
	//	var lat = Mathf.Atan2(-y, xzLen);
 
	//	return new LonLat(lon, lat);
	//}

	//public static LonLat Cartesian_to_polar_ratio(Vector3 point)
	//{
	//	return new LonLat(Cartesian_to_polar_radians(point).to_vec2() / Mathf.PI);
	//}

	internal static Vector3 Abs(Vector3 vec)
	{
		return new Vector3(Mathf.Abs(vec[0]), Mathf.Abs(vec[1]), Mathf.Abs(vec[2]));
	}
}

public class Scaler
{
	private readonly float domain_min;
	private readonly float domain_span;
	private readonly float range_min;
	private readonly float range_span;

	public Scaler(float domain_min, float domain_max, float range_min, float range_max)
	{
		this.domain_min = domain_min;
		this.domain_span = domain_max - domain_min;
		this.range_min = range_min;
		this.range_span = range_max - range_min;
	}

	public float scale(float value)
	{
		return range_min + (range_span * ((value - domain_min) / domain_span));
	}
}

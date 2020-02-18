
using UnityEngine;

public class Coord
{
	// TODO make these into lat lon
	// when lat lon keep width and height in AreaStats?
	public float x { get; private set; }
	public float y { get; private set; }
	public int index { get; private set; }

	public Coord(float x, float y, int index)
	{
		this.x = x;
		this.y = y;
		this.index = index;
	}

	public Coord minus_x()
	{
		return new Coord(x - 1, y, -1);
	}

	public Coord minus_y()
	{
		return new Coord(x, y - 1, -1);
	}
}

public class AreaStats
{
	// when lat lon keep width and height?
	public float unit_width { get; private set; }
	public float unit_height { get; private set; }
	public float max_x { get; private set; }
	public float max_y { get; private set; }

	public AreaStats(float unit_width, float unit_height, float max_x, float max_y)
	{
		this.unit_width = unit_width;
		this.unit_height = unit_height;
		this.max_x = max_x;
		this.max_y = max_y;
	}
}


public abstract class Region
{
	public abstract Coord[] get_area_coords();
	public abstract AreaStats get_area_stats();
	public abstract float potential_solar_W_per_m2(Coord coord);
	public abstract float potential_solar_W_per_m2_normalised(Coord coord);
	public abstract float potential_wind_W_per_m2(Coord coord);
	public abstract float potential_wind_W_per_m2_normalised(Coord coord);
}

public class USA_Texas_9300m : Region
{
	public override Coord[] get_area_coords()
	{
		return RegionUSATexas.area_coords_9300m;
	}

	private static AreaStats area_stats;
	public override AreaStats get_area_stats()
	{
		if (area_stats == default(AreaStats))
		{
			float max_x = 0, max_y = 0;
			foreach (var coord in get_area_coords())
			{
				max_x = Mathf.Max(max_x, coord.x);
				max_y = Mathf.Max(max_y, coord.y);
			}

			area_stats = new AreaStats(9.3f, 9.3f, max_x, max_y);
		}
		return area_stats;
	}

	public override float potential_solar_W_per_m2(Coord coord)
	{
		return SolarData_Texas_9300m.potential_solar_W_per_m2(coord);
	}

	public override float potential_solar_W_per_m2_normalised(Coord coord)
	{
		return SolarData_Texas_9300m.potential_solar_W_per_m2_normalised(coord);
	}

	public override float potential_wind_W_per_m2(Coord coord)
	{
		return WindData_Texas_50m_Height_9300m.potential_wind_W_per_m2(coord);
	}

	public override float potential_wind_W_per_m2_normalised(Coord coord)
	{
		return WindData_Texas_50m_Height_9300m.potential_wind_W_per_m2_normalised(coord);
	}
}

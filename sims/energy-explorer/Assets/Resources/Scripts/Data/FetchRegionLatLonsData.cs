using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FetchRegionLatlonsData : DataFetcherProgress
{
	public static FetchRegionLatlonsData Init(RegionOfInterest roi)
	{
		var data_root = DataManager.instance.root_data;
		var region_latlons = data_root.data.regions.get_latlons_value_ref(roi);

		return new FetchRegionLatlonsData(region_latlons.value_file, roi);
	}

	public delegate void OnLatLonData(RegionOfInterest roi, List<WSCell> cells);
	public event OnLatLonData on_latlon_data;

	RegionOfInterest roi;

	private FetchRegionLatlonsData(string resource, RegionOfInterest roi) : base(resource, name: roi.offshore_str + " Region Geometry")
	{
		this.roi = roi;
	}

	protected override void fetched_data(string response)
	{
		base.fetched_data(response);
		var cells = parse_latlon_response(response);
		on_latlon_data?.Invoke(roi, cells);
	}

	public List<WSCell> parse_latlon_response(string response)
	{
		//response = "lat,lon,vertex1_lat,vertex1_lon,vertex2_lat,vertex2_lon,vertex3_lat,vertex3_lon,vertex4_lat,vertex4_lon,vertex5_lat,vertex5_lon,vertex6_lat,vertex6_lon\n" + "36.6697,-102.1565,36.5932,-101.9285,36.4717,-102.1373,36.5483,-102.3650,36.7463,-102.3847,36.8680,-102.1760,36.7915,-101.9475\n" + "36.6239,-102.5927,36.5483,-102.3650,36.4261,-102.5727,36.5018,-102.8001,36.6996,-102.8204,36.8220,-102.6128,36.7463,-102.3847\n" + "36.5766,-103.0273,36.5018,-102.8001,36.3791,-103.0067,36.4539,-103.2337,36.6515,-103.2547,36.7744,-103.0481,,";

		var lines = response.Split('\n')
			.Select(l => l.Trim())
			.ToList();

		var expected_header = "lat,lon,vertex1_lat,vertex1_lon,vertex2_lat,vertex2_lon,vertex3_lat,vertex3_lon,vertex4_lat,vertex4_lon,vertex5_lat,vertex5_lon,vertex6_lat,vertex6_lon";
		var actual_header = lines[0];
		if (actual_header != expected_header)
		{
			Debug.LogWarning("Expecting the header to be: " + expected_header + " but was " + actual_header);
		}

		var cells = new List<WSCell>();
		var vertices_to_cells = new Dictionary<Vector2, List<WSCell>>();

		lines.Skip(1)
			.ToList()
			.ForEach(line =>
			{
				var values = line.Split(',')
					.Where(v => v != "")
					.Select(v => float.Parse(v))
					.ToList();

				// check values count
				if (values.Count != 12 && values.Count != 14)
				{
					Debug.Log("Unexpected number of values: " + values.Count + " in line: " + line);
				}

				var latlon = new Vector2(values[0], values[1]);
				var vertices = new List<Vector2>();
				var cell = new WSCell(latlon: latlon, vertices: vertices);
				cells.Add(cell);

				// calculate vertices and map to cells
				for (int i = 2; i < values.Count; i += 2)
				{
					var vertex = new Vector2(values[i], values[i + 1]);
					vertices.Add(vertex);

					if (!vertices_to_cells.ContainsKey(vertex))
					{
						vertices_to_cells[vertex] = new List<WSCell>();
					}
					vertices_to_cells[vertex].Add(cell);
				}
			});

		// calculate neighbours
		cells.ForEach(cell => cell.add_neighbours(vertices_to_cells));

		return cells;
	}
}

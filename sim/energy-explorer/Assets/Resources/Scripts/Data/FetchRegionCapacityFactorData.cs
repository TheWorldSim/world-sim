using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FetchRegionCapacityFactorData : DataFetcherProgress
{
	public delegate void OnCapacityFactorData(RegionOfInterest roi, DataTimeWindow time_window, CapacityFactorData capacity_factors);
	public event OnCapacityFactorData on_capacity_factor_data;

	// internal state
	private RegionOfInterest roi;
	private DataTimeWindow time_window;

	protected FetchRegionCapacityFactorData(string resource, string name, RegionOfInterest roi, DataTimeWindow time_window) : base(resource, name)
	{
		this.roi = roi;
		this.time_window = time_window;
	}

	protected override void fetched_data(string response)
	{
		base.fetched_data(response);
		var capacity_factors = parse_capacity_factor_response(response);
		on_capacity_factor_data?.Invoke(roi, time_window, capacity_factors);
	}

	private static CapacityFactorData parse_capacity_factor_response(string response)
	{
		//response = (
		//	"\"datetime\",\"29.4348,-93.6550\",\"29.4224,-94.0836\",\"29.1082,-93.8604\",\"29.0952,-94.2878\"\n" +
		//	"1514764800,425,412,446,437\n" +
		//	"1517443200,531,506,533,518\n" +
		//	"1519862400,452,447,461,461"
		//);

		var lines = response.Split('\n')
			.Select(l => l.Trim())
			.Where(l => l[0] != '#') // remove meta data header lines
			.ToList();

		var header = lines[0];
		var cell_latlon_to_index = parse_latlons_from_header(header);

		var lines_no_header = lines.Skip(1).ToList();

		var datetime_to_index = new Dictionary<DateTime, int>();
		var date_time_latlon_capacity = new List<List<float>>(lines_no_header.Count);

		for (int datetime_index = 0; datetime_index < lines_no_header.Count; ++datetime_index)
		{
			var line = lines_no_header[datetime_index];

			var values = line.Split(',')
				.Select(v => int.Parse(v))
				.ToList();

			var seconds_since_1970 = values[0];
			var date_time = DateTimeHelper.convert_from_unix_time_stamp(seconds_since_1970);
			datetime_to_index[date_time] = datetime_index;

			var only_capacity_factors = values.Select(v => v / 1000f).Skip(1);

			// This smells.  Add(null) ensures the value is present for the following
			// [] index operation
			date_time_latlon_capacity.Add(null);
			date_time_latlon_capacity[datetime_index] = only_capacity_factors.ToList();
		}

		var data = new CapacityFactorData(cell_latlon_to_index, datetime_to_index, date_time_latlon_capacity);
		return data;
	}

	private static Dictionary<Vector2, int> parse_latlons_from_header(string header)
	{
		var latlons = header.Substring(1, header.Length - 2) // don't include the starting and trailing "
			.Split(new string[] { "\",\"" }, StringSplitOptions.None)
			.Skip(1) // skip "datetime"
			.ToList();

		var cell_latlon_to_index = new Dictionary<Vector2, int>();
		for (int i = 0; i < latlons.Count; ++i)
		{
			var parts = latlons[i].Split(',');
			var lat = float.Parse(parts[0]);
			var lon = float.Parse(parts[1]);
			var latlon = new Vector2(lat, lon);
			cell_latlon_to_index[latlon] = i;
		}

		return cell_latlon_to_index;
	}
}

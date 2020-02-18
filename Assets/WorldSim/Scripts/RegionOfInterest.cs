

using System;

public class RegionOfInterest
{
	public string country_name { get; private set; }
	public int country_index { get; private set; }
	public string province_name { get; private set; }
	public float time_zone { get; private set; }
	public Currency currency { get; private set; }
	public bool offshore { get; private set; }
	public string offshore_str { get; private set; }
    public string string_id { get; private set; }

	private int hash_id;

	public RegionOfInterest(string country_name, int country_index, string province_name, float time_zone, Currency currency, bool offshore = false)
	{
		this.country_name = country_name;
		this.country_index = country_index;
		this.province_name = province_name;
		this.time_zone = time_zone + 1; // leave them on summer time
		this.currency = currency;
		this.offshore = offshore;
		this.offshore_str = offshore ? "Offshore" : "Onshore";
		string_id = country_name + "--" + province_name + " " + offshore_str;
		hash_id = string_id.GetHashCode();
	}

	internal bool equals(RegionOfInterest other)
	{
		if (other == null) return false;
		return country_name == other.country_name && province_name == other.province_name && offshore == other.offshore;
	}

	internal bool province_name_exists()
	{
		return province_name != default && province_name != "";
	}

	// provinces
	public static string texas = "Texas";
	// countries
	public static string united_kingdom = "United Kingdom";
	public static string united_states_of_america = "United States of America";
	//public string offshore_str;

	public RegionOfInterest clone(bool offshore)
	{
		return new RegionOfInterest(
			country_name: country_name,
			country_index: country_index,
			province_name: province_name,
			time_zone: time_zone,
			currency: currency,
			offshore: offshore
		);
	}

	public override string ToString()
	{
		return "RegionOfInterest " + string_id;
	}

	public string ToDisplayString(bool include_offshore = false)
	{
		var str = province_name_exists() ? province_name + ", " + country_name : country_name;
		return include_offshore ? (str + " (" + offshore_str + ")") : str;
	}

	public string ToShortDisplayString(bool include_offshore = false)
	{
		var str = province_name_exists() ? province_name : country_name;
		return include_offshore ? (str + " (" + offshore_str + ")") : str;
	}

	public override int GetHashCode()
	{
		return hash_id;
	}
}

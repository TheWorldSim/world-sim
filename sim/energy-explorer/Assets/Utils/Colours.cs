using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Colours
{
	private static float strong = 0.75f;
	//private static float medium = 0.5f;
	// private static float weak = 0.25f;
	private static float vweak = 0.1f;
	//private static float vvweak = 0.05f;

	private static Color black (float strength = 0.5f) => new Color(0, 0, 0, strength);
	private static Color blue2 (float strength = 0.5f) => new Color(0.17647f, 0.54902f, 1f);
	private static Color grey (float strength = 0.5f) => new Color(0.7f, 0.7f, 0.7f, 0.05f);
	private static Color lime_green (float strength = 0.5f) => new Color(0.7f, 1f, 0.6f, strength);
	private static Color orange_notice (float strength = 0.5f) => new Color(0.7f, 0.7f, 0f, strong);
	private static Color red_error (float strength = 0.5f) => new Color(1f, 0.3f, 0f, 0.75f);
	private static Color transparent = new Color(0, 0, 0, 0);
	private static Color white (float strength = 0.5f) => new Color(1f, 1f, 1f);

	public static readonly Color button_selected = blue2();
	public static readonly Color button_unselected = white();

	public static readonly Color default_country_fill_colour = transparent;
	public static readonly Color default_province_fill_colour = transparent;
	public static readonly Color hex_grid_colour = grey(vweak);
	public static readonly Color cell_fill_colour = lime_green(strong);
	public static readonly Color non_roi_fill_colour = black(strong);
	public static readonly Color cell_offshore_fill_colour = blue2(strong);
	public static readonly Color cell_conflict_fill_colour = red_error(strong);
	public static readonly Color cell_region_boundary_fill_colour = orange_notice(strong);

	internal static void SetColour(Selectable obj, Color colour)
	{
		var colours = obj.colors;
		colours.normalColor = colour;
		obj.colors = colours;
	}

	internal static void SetSelectedColour(Selectable obj, Color colour)
	{
		var colours = obj.colors;
		colours.selectedColor = colour;
		obj.colors = colours;
	}

	private static Color bakers_blue8_0 = new Color(0.031f, 0.271f, 0.58f);
	private static Color bakers_blue8_1 = new Color(0.129f, 0.443f, 0.71f);
	private static Color bakers_blue8_2 = new Color(0.259f, 0.573f, 0.776f);
	private static Color bakers_blue8_3 = new Color(0.42f, 0.682f, 0.839f);
	private static Color bakers_blue8_4 = new Color(0.62f, 0.792f, 0.882f);
	private static Color bakers_blue8_5 = new Color(0.776f, 0.859f, 0.937f);
	private static Color bakers_blue8_6 = new Color(0.871f, 0.922f, 0.969f);
	private static Color bakers_blue8_7 = new Color(0.969f, 0.984f, 1f);
	private static List<Color> bakers_blue8_all = new List<Color>() {
		bakers_blue8_0, bakers_blue8_1, bakers_blue8_2, bakers_blue8_3,
		bakers_blue8_4, bakers_blue8_5, bakers_blue8_6, bakers_blue8_7,
	};

	internal static Color bakers_blue8(float capacity_factor, float min, float max)
	{
		var scaled_capacity_factor = (capacity_factor - min) / (max - min);

		var number_of_buckets = 6f;
		var capacity_factor_bucketed = (float)Math.Round(scaled_capacity_factor * number_of_buckets) / number_of_buckets;
		var capacity_factor_bucketed_h2 = Mathf.Clamp((capacity_factor_bucketed * 1.3f) - 0.3f, 0, 1);

		var r = 0.15f + capacity_factor_bucketed * 0.68f;
		var g = 0.25f + capacity_factor_bucketed * 0.68f;
		var b = 1f;
		return new Color(r, g, b, capacity_factor_bucketed_h2);
	}

	internal static Color solar(float capacity_factor, float min, float max)
	{
		var scaled_capacity_factor = (capacity_factor - min) / (max - min);

		var number_of_buckets = 6f;
		var capacity_factor_bucketed = (float)Math.Round(scaled_capacity_factor * number_of_buckets) / number_of_buckets;
		var capacity_factor_bucketed_h2 = Mathf.Clamp((capacity_factor_bucketed * 1.3f) - 0.3f, 0, 1);

		var r = 0.75f + capacity_factor_bucketed * 0.25f;
		var g = 0.75f + capacity_factor_bucketed * 0.25f;
		var b = 0.15f + capacity_factor_bucketed * 0.68f;
		return new Color(r, g, b, capacity_factor_bucketed_h2);
	}
}

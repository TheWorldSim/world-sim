using System;
using System.Collections.Generic;
using UnityEngine;

public class WSCell
{
	public int index { get; private set; }
	public Vector2 latlon { get; private set; }
	public List<Vector2> vertices { get; private set; }
	public HashSet<WSCell> neighbours { get; private set; }

	private int wpm_cell_index = -1;

	public WSCell(Vector2 latlon, List<Vector2> vertices)
	{
		index = get_cell_index(latlon);
		this.latlon = latlon;
		this.vertices = vertices;
		neighbours = new HashSet<WSCell>();
	}

	public int get_wpm_cell_index(bool error_if_not_set = true)
	{
		if (wpm_cell_index == -1 && error_if_not_set) throw new Exception("wpm_cell_index not set");
		return wpm_cell_index;
	}

	public void set_wpm_cell_index(int value)
	{
		wpm_cell_index = value;
	}

	private static Dictionary<Vector2, int> cell_to_index = new Dictionary<Vector2, int>();
	private static int next_index = 0;

	private static int get_cell_index(Vector2 latlon)
	{
		if (cell_to_index.TryGetValue(latlon, out int index))
		{
			return index;
		}

		cell_to_index[latlon] = ++next_index;

		return next_index;
	}

	internal void add_neighbours(Dictionary<Vector2, List<WSCell>> vertices_to_cells)
	{
		vertices.ForEach(vertex =>
		{
			var cells = vertices_to_cells[vertex];
			cells.ForEach(cell =>
			{
				if (cell != this && !neighbours.Contains(cell)) neighbours.Add(cell);
			});
		});
	}
}

public static class TestWSCell
{
	public static WSCell generate_ws_cell()
	{
		var latlon = new Vector2(0, 0);
		var vertices = new List<Vector2>() {
			new Vector2(0, 0),
			new Vector2(1, 1)
		};

		return new WSCell(latlon, vertices);
	}
}

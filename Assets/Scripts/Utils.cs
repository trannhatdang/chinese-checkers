using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static List<List<int>> ReadCSV(TextAsset data)
	{
		List<List<int>> ret = new List<List<int>>();
		var lines = data.text.Split('\n');

		for (int i = 0; i < lines.Length; ++i)
		{
			var readline = lines[i].Split(',');

			var line = new List<int>();
			for (int j = 0; j < readline.Length; ++j)
			{
				int newNum = 0;
				bool success = Int32.TryParse(readline[j], out newNum);

				if (!success)
				{
					break;
				}

				line.Add(newNum);
			}

			ret.Add(line);
		}

		return ret;
	}
}

using System;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Y2023.Day6;

[ProblemName("Wait For It")]
public partial class Solution : Solver
{
	public object PartOne(string input)
	{
		var lines = input.Split("\n");
		var times = (from match in NumberRegex().Matches(lines[0]) select int.Parse(match.Value)).ToArray();
		var records = (from match in NumberRegex().Matches(lines[1]) select int.Parse(match.Value)).ToArray();
		var minLenght = Math.Min(times.Length, records.Length);

		int sum = 0;

		for (int i = 0; i < minLenght; i++)
		{
			var time = times[i];
			var record = records[i];
			int margin = 0;

			for (int j = 1; j < time; j++)
			{
				if (j * (time - j) > record) { margin++; }
			}
			
			if (sum == 0) { sum = margin; }
			else { sum *= margin; }
		}

		return sum;
	}
	public object PartTwo(string input)
	{
		var lines = input.Split("\n");
		var time = ulong.Parse(PaddedNumberRegex().Match(lines[0]).Value.Replace(" ", ""));
		var record = ulong.Parse(PaddedNumberRegex().Match(lines[1]).Value.Replace(" ", ""));

		int margin = 0;

		for (ulong j = 1; j < time; j++)
		{
			if (j * (time - j) > record) { margin++; }
		}

		return margin;
	}

	[GeneratedRegex(@"\d+")]
	private static partial Regex NumberRegex();
	
	[GeneratedRegex(@"\d.*$")]
	private static partial Regex PaddedNumberRegex();
}

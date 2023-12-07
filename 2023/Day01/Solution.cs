using System;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Y2023.Day1;

[ProblemName("Trebuchet")]
public partial class Solution : Solver
{
	private static readonly Dictionary<string, int> _stringToIntTable = new()
	{
		{"1", 1},
		{"2", 2},
		{"3", 3},
		{"4", 4},
		{"5", 5},
		{"6", 6},
		{"7", 7},
		{"8", 8},
		{"9", 9},
		{"0", 0},
		{"one", 1},
		{"two", 2},
		{"three", 3},
		{"four", 4},
		{"five", 5},
		{"six", 6},
		{"seven", 7},
		{"eight", 8},
		{"nine", 9},
	};

	public int ParseLine(string line)
	{
		List<int> digits = [];

		var matches = NumberRegex().Matches(line).ToList();

		foreach (var matchObj in matches)
		{
			if (_stringToIntTable.TryGetValue(matchObj.Groups[1].Value, out int digit)) { digits.Add(digit); }
		}

		string twoDigitNumberString = String.Join("", digits.First(), digits.Last());
		return Int32.Parse(twoDigitNumberString);
	}
	
	[GeneratedRegex(@"(?=(\d|one|two|three|four|five|six|seven|eight|nine))")]
	private static partial Regex NumberRegex();
	
	public object PartOne(string input)
	{
		int sum = 0;
		foreach (string line in input.Split('\n'))
		{
			List<int> digits = [];

			foreach (char c in line)
			{
				if (int.TryParse(c.ToString(), out int num))
				{
					digits.Add(num);
				}
			}
			if (digits.Count > 0)
			{
				string twoDigitNumberString = String.Join("", digits.First(), digits.Last());
				if (int.TryParse(twoDigitNumberString, out int twoDigitNumber))
				{
					sum += twoDigitNumber;
				}
			}
		}

		return $"{sum}";
	}
	
	public object PartTwo(string input)
	{
		int sum = 0;
		foreach (string line in input.Split('\n'))
		{
			int number = ParseLine(line);
			sum += number;
		}
		return $"{sum}";
	}

}

using System;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Y2023.Day5;

[ProblemName("If You Give A Seed A Fertilizer")]
public partial class Solution : Solver
{
	public object PartOne(string input)
	{
		var parts = input.Split("\n\n");
		var seeds = from m in NumberRegex().Matches(parts[0]) select uint.Parse(m.Value);
		var maps = parts.Skip(1).Select(ParseMap).ToList();

		uint smallestLocation = uint.MaxValue;

		foreach (uint seed in seeds)
		{
			uint location = seed;
			foreach (var map in maps)
			{
				foreach (var (Destination, Source, Offset) in map)
				{
					if (location >= Source && location < Source + Offset)
					{
						location = Destination + (location - Source);
						break;
					}
				}
			}

			smallestLocation = Math.Min(smallestLocation, location);
		}
		return smallestLocation;
	}
	public object PartTwo(string input)
	{
		var parts = input.Split("\n\n");
		
		var maps = parts.Skip(1).Select(ParseMap).ToList();

		var seedRanges = NumberPairRegex()
			.Matches(parts[0])
			.Select(match => {
				return match.Value
					.Split(' ')
					.Select(uint.Parse)
					.ToArray();
			})
			.ToArray();

		for (uint i = 0; i < uint.MaxValue; i++)
		{
			uint seed = GetSeedFromLocation(i, maps);
			if (SeedIsInRange(seed, seedRanges))
			{
				return i;
			}
		}

		throw new Exception("No valid seeds found");
	}

	private uint GetSeedFromLocation(uint location, List<List<(uint Destination, uint Source, uint Offset)>> maps)
	{
		uint seed = location;
		for (int i = maps.Count - 1; i >= 0; i--)
		{
			foreach (var (Destination, Source, Offset) in maps[i])
			{
				if (seed >= Destination && seed < Destination + Offset)
				{
					seed = Source + (seed - Destination);
					break;
				}
			}
		}

		return seed;
	}

	private bool SeedIsInRange(uint seed, uint[][] seedRanges)
	{
		bool inRange = false;
		foreach (var seedRange in seedRanges)
		{
			if (seed >= seedRange[0] && seed < seedRange[0] + seedRange[1])
			{
				inRange = true;
			}
		}

		return inRange;
	}

	private List<(uint Destination, uint Source, uint Offset)> ParseMap(string mapString)
	{
		List<(uint, uint, uint)> map = [];

		foreach (string line in mapString.Split('\n').Skip(1))
        {

            var values = (from m in NumberRegex().Matches(line) select uint.Parse(m.Value)).ToArray();
			map.Add((values[0], values[1], values[2]));
        }

		return map;
    }
	
	[GeneratedRegex(@"\d+")]
	private static partial Regex NumberRegex();
	
	[GeneratedRegex(@"\d+ \d+")]
	private static partial Regex NumberPairRegex();
}

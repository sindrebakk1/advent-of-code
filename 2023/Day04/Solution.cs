using System;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Y2023.Day4;

[ProblemName("Scratchcards")]
public partial class Solution : Solver
{
	public object PartOne(string input) => (
		from line in input.Split("\n")
		let card = ParseCard(line)
		where card.matches > 0
		select Math.Pow(2, card.matches - 1)
	).Sum();
	public object PartTwo(string input)
	{
		var cards = input.Split("\n").Select(ParseCard).ToArray();
		var counts = cards.Select(_ => 1).ToArray();

		for (var i = 0; i < cards.Length; i++)
		{
			var (card, count) = (cards[i], counts[i]);
			for (var j = 0; j < card.matches; j++)
			{
				counts[i + j + 1] += count;
			}
		}
		return counts.Sum();
	}

	Card ParseCard(string line)
	{
		var parts = line.Split(':', '|');
		var l = from m in NumberRegex().Matches(parts[1]) select m.Value;
		var r = from m in NumberRegex().Matches(parts[2]) select m.Value;
		return new Card(l.Intersect(r).Count());
	}

	[GeneratedRegex(@"\d+")]
	private static partial Regex NumberRegex();
}

record Card(int matches);

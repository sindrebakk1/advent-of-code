using System;
using AdventOfCode.Common;
using AdventOfCode.Y2023.Day02;

namespace AdventOfCode.Y2023.Day2;

[ProblemName("Cube Conundrum")]
public class Solution : Solver
{
	public object PartOne(string input)
	{
		Game game = new(12, 13, 14);

		int result = 0;

		foreach (string gameString in input.Split('\n'))
		{
			game.Play(gameString);

			if (game.IsPossible())
			{
				result += game.Id;
			}
		}

		return result;
	}
	public object PartTwo(string input)
	{
		Game game = new(12, 13, 14);

		int result = 0;

		foreach (string gameString in input.Split('\n'))
		{
			game.Play(gameString);

			result += game.GetMinPower();
		}

		return result;
	}
}

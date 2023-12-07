using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day02;

enum Block
{
	Red,
	Green,
	Blue,
}

partial class Game(int redLimit, int greenLimit, int blueLimit)
{
	public int Id;

	private int _red;
	private int _green;
	private int _blue;

	private int _minRed;
	private int _minGreen;
	private int _minBlue;

	private int _power;

	private readonly int _redLimit = redLimit;
	private readonly int _greenLimit = greenLimit;
	private readonly int _blueLimit = blueLimit;

	private bool _possible = true;

	public int Red
	{
		get { return _red; }
		set
		{
			if (value > _redLimit)
			{
				_possible = false;
			}
			_red = value;
		}
	}

	public int Green
	{
		get { return _green; }
		set
		{
			if (value > _greenLimit)
			{
				_possible = false;
			}
			_green = value;
		}
	}

	public int Blue
	{
		get { return _blue; }
		set
		{
			if (value > _blueLimit)
			{
				_possible = false;
			}
			_blue = value;
		}
	}

	public void Add(int amount, Block color)
	{
		switch (color)
		{
			case Block.Red:
				Red += amount;
				break;
			case Block.Green:
				Green += amount;
				break;
			case Block.Blue:
				Blue += amount;
				break;
		}
	}

	public bool IsPossible()
	{
		return _possible;
	}

	public int GetMinPower()
	{
		return _minRed * _minGreen * _minBlue;
	}

	private void ResetAmounts()
	{
		_red = 0;
		_green = 0;
		_blue = 0;
	}

	private void ResetMinimums()
	{
		_minRed = 0;
		_minGreen = 0;
		_minBlue = 0;
	}

	private (int GameId, List<string> Rounds) ParseGameString(string line)
	{
		List<string> rounds = [];
		int gameId = Int32.Parse(GameIdRegex().Match(line).Value);

		var roundMatches = RoundRegex().Matches(line);

		foreach (var matchObj in roundMatches)
		{
			string match = matchObj.ToString() ?? "";
			if (match.Length > 0)
			{
				rounds.Add(match);
			}
		}

		return (gameId, rounds);
	}

	private (int RedAmount, int GreenAmount, int BlueAmount) ParseRoundString(string roundString)
	{
		int red = 0;
		int green = 0;
		int blue = 0;

		var redMatces = RedAmountRegex().Matches(roundString);
		var greenMatches = GreenAmountRegex().Matches(roundString);
		var blueMatches = BlueAmountRegex().Matches(roundString);

		foreach (var matchObj in redMatces)
		{
			string match = matchObj.ToString() ?? "";
			if (Int32.TryParse(match, out int redAmount))
			{
				red += redAmount;
			}
		}

		foreach (var matchObj in greenMatches)
		{
			string match = matchObj!.ToString() ?? "";
			if (Int32.TryParse(match, out int greenAmount))
			{
				green += greenAmount;
			}
		}

		foreach (var matchObj in blueMatches)
		{
			string match = matchObj.ToString() ?? "";
			if (Int32.TryParse(match, out int blueAmount))
			{
				blue += blueAmount;
			}
		}


		return (red, green, blue);
	}

	private void PlayRound(string roundString)
	{
		ResetAmounts();

		var (RedAmount, GreenAmount, BlueAmount) = ParseRoundString(roundString);

		Add(RedAmount, Block.Red);
		Add(GreenAmount, Block.Green);
		Add(BlueAmount, Block.Blue);

		if (RedAmount > _minRed) { _minRed = RedAmount; }
		if (GreenAmount > _minGreen) { _minGreen = GreenAmount; }
		if (BlueAmount > _minBlue) { _minBlue = BlueAmount; }
	}

	public void Play(string gameString)
	{
		_possible = true;
		ResetMinimums();

		var (GameId, Rounds) = ParseGameString(gameString);
		Id = GameId;

		foreach (string roundString in Rounds)
		{
			PlayRound(roundString);
		}
	}

	[GeneratedRegex(@"(?<=^Game )\d+(?=:)")]
	private static partial Regex GameIdRegex();

	[GeneratedRegex(@"(?<=Game \d+: )[^;\n]+(?=[;])|(?<=; )[^;\n]+(?=[\n;]|$)")]
	private static partial Regex RoundRegex();

	[GeneratedRegex(@"\d+(?= red)")]
	private static partial Regex RedAmountRegex();

	[GeneratedRegex(@"\d+(?= green)")]
	private static partial Regex GreenAmountRegex();

	[GeneratedRegex(@"\d+(?= blue)")]
	private static partial Regex BlueAmountRegex();
}
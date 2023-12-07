using System;
using AdventOfCode.Common;

namespace AdventOfCode.Y2023.Day3;

[ProblemName("Gear Ratios")]
public class Solution : Solver
{
	private Dictionary<Tuple<int, int>, Gear> lookup = [];
	public object PartOne(string input)
	{
		List<string> lines = new List<string>(input.Split('\n'));

		int sum = 0;
		
		for (int i = 0; i < lines.Count; i++)
		{
			string curr = lines[i];
			string? next = null;
			string? prev = null;

			if (i > 0) { prev = lines[i - 1]; }
			if (i < lines.Count - 1) { next = lines[i + 1]; }


			for (int l = 0, r; l < curr.Length; l++)
			{
				if (Char.IsNumber(curr[l]))
				{
					for (r = l; r < curr.Length; r++)
					{
						if (r + 1 >= curr.Length || !Char.IsNumber(curr[r + 1])) { break; }
					}

					var partNumber = new PartNumber(curr, i, l, r);
					bool isValid = partNumber.IsValid(next, prev, out var gearLocation);

					if (gearLocation is not null)
					{
						if (lookup.TryGetValue(gearLocation, out var gear))
						{
							gear.AddConnection(partNumber.Value);
							lookup[gearLocation] = gear;
						}
						else
						{
							var newGear = new Gear(gearLocation.Item1, gearLocation.Item2);
							newGear.AddConnection(partNumber.Value);
							lookup.Add(gearLocation, newGear);
						}
					}

					if (isValid)
					{
						sum += partNumber.Value;
					}

					l = r;
				}
			}
		}

		return sum;
	}
	public object _PartTwo(string input)
	{
		int sum = 0;
		foreach (var gear in lookup.Values)
		{
			sum += gear.Ratio;
		}
		return sum;
	}
}

class PartNumber(string line, int lineNumber, int left, int right)
{
	private readonly string _currentLine = line;

	private readonly int _lineNumber = lineNumber;

	private readonly int _left = left;
	private readonly int _right = right;

	public int Value { get; } = Int32.Parse(line[left..(right + 1)]);

	private static bool IsSymbol(char c)
	{
		return !Char.IsNumber(c) && c != 46;
	}

	public bool IsValid(string? nextLine, string? previousLine, out Tuple<int, int>? gearLocation)
	{
		gearLocation = null;
		int start = _left - 1 >= 0 ? _left - 1 : 0;
		int end = _right + 1 < _currentLine.Length ? _right + 1 : _currentLine.Length - 1;

		if (IsSymbol(_currentLine[start]))
		{
			if (_currentLine[start] == 42)
			{
				gearLocation = new(_lineNumber, start);
			}
			return true;
		}

		if (IsSymbol(_currentLine[end]))
		{
			if (_currentLine[end] == 42)
			{
				gearLocation = new(_lineNumber, end);
			}
			return true;
		}

		for (int i = start; i <= end; i++)
		{
			if (nextLine is not null && IsSymbol(nextLine[i]))
			{
				if (nextLine[i] == 42)
				{
					gearLocation = new(_lineNumber + 1, i);
				}
				return true;
			}
			if (previousLine is not null && IsSymbol(previousLine[i]))
			{
				if (previousLine[i] == 42)
				{
					gearLocation = new(_lineNumber - 1, i);
				}
				return true;
			}
		}

		return false;
	}
}

public struct Gear(int row, int col)
{
	private readonly int _row = row;
	private readonly int _col = col;
	private readonly List<int> _connections = [];

	public readonly int Row { get { return _row; } }
	public readonly int Collumn { get { return _col; } }

	public int Ratio = 0;

	public void AddConnection(int connection)
	{
		_connections.Add(connection);
		if (_connections.Count == 2)
		{
			Ratio = _connections[0] * _connections[1];
		}
		else
		{
			Ratio = 0;
		}
	}

	public override string ToString()
	{
		return $"(Row: {Row}, Collumn: {Collumn}, Ratio: {Ratio})";
	}
}

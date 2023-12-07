using System;
using System.Reflection;
using System.Diagnostics;

namespace AdventOfCode.Common;

[AttributeUsage(AttributeTargets.Class)]
class ProblemName(string name) : Attribute
{
	public readonly string Name = name;
}

public interface Solver
{
	object PartOne(string input);
	object? PartTwo(string input) => null;
}

static class SolverExtensions
{

	public static IEnumerable<object> Solve(this Solver solver, string input)
	{
		yield return solver.PartOne(input);

		var res = solver.PartTwo(input);
		if (res != null)
		{
			yield return res;
		}
	}

	public static string GetName(this Solver solver)
	{
		return (
			solver
				.GetType()
				.GetCustomAttribute(typeof(ProblemName)) as ProblemName
		).Name;
	}

	public static string DayName(this Solver solver)
	{
		return $"Day {solver.Day()}";
	}

	public static int Year(this Solver solver)
	{
		return Year(solver.GetType());
	}

	public static int Year(Type t)
	{
		return int.Parse(t.FullName.Split('.')[1].Substring(1));
	}
	public static int Day(this Solver solver)
	{
		return Day(solver.GetType());
	}

	public static int Day(Type t)
	{
		return int.Parse(t.FullName.Split('.')[2].Substring(3));
	}

	public static string WorkingDir(int year)
	{
		return Path.Combine(year.ToString());
	}

	public static string WorkingDir(int year, int day)
	{
		return Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));
	}

	public static string WorkingDir(this Solver solver)
	{
		return WorkingDir(solver.Year(), solver.Day());
	}
}

record SolverResult(string[] answers, string[] errors);

static class Runner
{

	private static string GetNormalizedInput(string file)
	{
		var input = File.ReadAllText(file);

		// on Windows we have "\r\n", not sure if this causes more harm or not
		input = input.Replace("\r", "");

		if (input.EndsWith("\n"))
		{
			input = input.Substring(0, input.Length - 1);
		}
		return input;
	}

	public static SolverResult RunSolver(Solver solver, bool example)
	{

		var workingDir = solver.WorkingDir();
		var indent = "    ";
		Write(ConsoleColor.White, $"{indent}{solver.DayName()}: {solver.GetName()}");
		WriteLine();
		var file = Path.Combine(workingDir, "input.in");
		var exampleFile = Path.Combine(workingDir, "example.in");
		var input = example && File.Exists(exampleFile)
			? GetNormalizedInput(exampleFile)
			: GetNormalizedInput(file);
		
		var iline = 0;
		var answers = new List<string>();
		var errors = new List<string>();
		var stopwatch = Stopwatch.StartNew();
		foreach (var line in solver.Solve(input))
		{
			var ticks = stopwatch.ElapsedTicks;
			var skipped = line.ToString() == "skipped";
			var result = skipped ? "Skipped" : line.ToString();
			answers.Add(result);

			var (statusColor, status) = skipped ?
				(ConsoleColor.DarkGray, "X") : (ConsoleColor.Cyan, "✓");

			Write(statusColor, $"{indent}  {status}");
			Console.Write($" {result} ");
			var diff = ticks * 1000.0 / Stopwatch.Frequency;

			WriteLine(
				diff > 1000 ? ConsoleColor.Red :
				diff > 500 ? ConsoleColor.Yellow :
				ConsoleColor.DarkGreen,
				skipped ? "" : $"({diff.ToString("F3")} ms)"
			);
			iline++;
			stopwatch.Restart();
		}

		return new SolverResult(answers.ToArray(), errors.ToArray());
	}

	public static void RunAll(bool example, params Solver[] solvers)
	{
		var errors = new List<string>();

		foreach (var solver in solvers)
		{
			var result = RunSolver(solver, example);
			WriteLine();
			errors.AddRange(result.errors);
		}

		WriteLine();

		if (errors.Count > 0)
		{
			WriteLine(ConsoleColor.Red, "Errors:\n" + string.Join("\n", errors));
		}
	}

	private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "")
	{
		Write(color, text + "\n");
	}
	private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "")
	{
		var c = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.Write(text);
		Console.ForegroundColor = c;
	}
}
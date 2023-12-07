using System;
using System.Reflection;

using AdventOfCode.Common;

var tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
	.Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
	.OrderBy(t => t.FullName)
	.ToArray();

ConsoleKeyInfo? key = null;
do
{
	Console.Clear();
	Console.WriteLine("Advent Of Code");
	
	if (key?.Key == ConsoleKey.A)
	{
		bool example = key?.Modifiers == ConsoleModifiers.Shift;
		try
		{
			Runner.RunAll(example, GetSolvers(tsolvers));
		}
		catch (AggregateException a)
		{
			throw;
		}
	}
	
	Console.WriteLine("press 'a' to run all solvers, SHIFT+'a' to run all with example input or 'q'/ESCAPE to quit");
	key = Console.ReadKey(true);
} while (key == null || (key?.KeyChar != 'q' && key?.Key != ConsoleKey.Escape));


static Solver[] GetSolvers(params Type[] tsolver)
{
	return tsolver.Select(t => Activator.CreateInstance(t) as Solver).ToArray();
}
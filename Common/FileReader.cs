using System;

namespace AdventOfCode.Common;

public static class FileReader
{
	public static List<string> Read(string filePath) {
		var reader = new StreamReader(filePath);
		List<string> result = [];

		do
		{
			var line = reader.ReadLine();
			if (line != null) { result.Add(line); }
		} while (reader.EndOfStream != true);

		reader.Dispose();

		return result;
	}
}

using FluentSerializer.Core.BenchmarkUtils.Runner;

using System;
using System.Globalization;

namespace FluentSerializer.Core.BenchmarkUtils.TestData;

public readonly record struct TestCase<TData>(Func<TData> GetData, int Count, long SizeInBytes) : ITestCase
{
	public override string ToString() => $"{GetReadableSize()} {Count,5:(##000)}";

	// Returns the human-readable file size for an arbitrary, 64-bit file size 
	// The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
	private string GetReadableSize()
	{
		// Get absolute value
		long absoluteValue = (SizeInBytes < 0 ? -SizeInBytes : SizeInBytes);
		// Determine the suffix and readable value
		string suffix;
		double readable;
		if (absoluteValue >= 0x1000000000000000) // Exabyte
		{
			suffix = "EB";
			readable = (SizeInBytes >> 50);
		}
		else if (absoluteValue >= 0x4000000000000) // Petabyte
		{
			suffix = "PB";
			readable = (SizeInBytes >> 40);
		}
		else if (absoluteValue >= 0x10000000000) // Terabyte
		{
			suffix = "TB";
			readable = (SizeInBytes >> 30);
		}
		else if (absoluteValue >= 0x40000000) // Gigabyte
		{
			suffix = "GB";
			readable = (SizeInBytes >> 20);
		}
		else if (absoluteValue >= 0x100000) // Megabyte
		{
			suffix = "MB";
			readable = (SizeInBytes >> 10);
		}
		else if (absoluteValue >= 0x400) // Kilobyte
		{
			suffix = "KB";
			readable = SizeInBytes;
		}
		else
		{
			return SizeInBytes.ToString("0 B", CultureInfo.InvariantCulture); // Byte
		}
		// Divide by 1024 to get fractional value
		readable /= 1024;
		// Return formatted number with suffix
		return readable.ToString("0.### ", StaticTestRunner.AppCulture) + suffix;
	}
}
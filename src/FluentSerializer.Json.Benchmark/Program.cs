using FluentSerializer.Core.BenchmarkUtils.Configuration;
using FluentSerializer.Core.BenchmarkUtils.Runner;
using FluentSerializer.Json.Benchmark.Data;

using System;
using System.Linq;

namespace FluentSerializer.Json.Benchmark;

public static class Program
{
	[STAThread]
	public static void Main(params string[] arguments)
	{
		StaticTestRunner.RequireElevatedPermissions(in arguments);

		if (!arguments.Contains("--no-generate"))
			JsonBenchmarkData.Default.GenerateTestCaseFiles();

		StaticTestRunner.Run(
			typeof(Program).Assembly, in arguments,
			"json-serializer", new ValueSizeTestOrderer());
	}
}
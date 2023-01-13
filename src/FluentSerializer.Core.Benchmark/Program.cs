using FluentSerializer.Core.BenchmarkUtils.Configuration;
using FluentSerializer.Core.BenchmarkUtils.Runner;

using System;

namespace FluentSerializer.Core.Benchmark;

public static class Program
{
	[STAThread]
	public static void Main(params string[] arguments)
	{
		StaticTestRunner.RequireElevatedPermissions(in arguments);
		StaticTestRunner.Run(typeof(Program).Assembly, in arguments, "core", new ClassAndCategoryOrderer());
	}
}
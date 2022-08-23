using FluentSerializer.Core.BenchmarkUtils.Configuration;
using FluentSerializer.Core.BenchmarkUtils.Runner;
using FluentSerializer.Xml.Benchmark.Data;

using System;
using System.Linq;

#if !NET5_0_OR_GREATER
using System.Security.Permissions;
#endif

namespace FluentSerializer.Xml.Benchmark;

public static class Program
{
	[STAThread]
#if !NET5_0_OR_GREATER
	[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
#endif
	public static void Main(params string[] arguments)
	{
		StaticTestRunner.RequireElevatedPermissions(in arguments);

		if (!arguments.Contains("--no-generate"))
			XmlBenchmarkData.Default.GenerateTestCaseFiles();

		StaticTestRunner.Run(
			typeof(Program).Assembly, in arguments,
			"xml-serializer", new ValueSizeTestOrderer());
	}
}
using FluentSerializer.Core.BenchmarkUtils.Runner;
using FluentSerializer.Json.Benchmark.Data;
using System;
using System.Linq;
using System.Security.Permissions;

namespace FluentSerializer.Json.Benchmark
{
    public static class Program
	{
#if !NET5_0_OR_GREATER
		[STAThread, PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
#endif
		public static void Main(params string[] arguments)
		{
			StaticTestRunner.RequireElevatedPermissions();

			if (!arguments.Contains("--no-generate"))
				JsonDataCollection.Default.GenerateTestCaseFiles();

            StaticTestRunner.Run(typeof(Program).Assembly, "json-serializer");
        }
    }
}

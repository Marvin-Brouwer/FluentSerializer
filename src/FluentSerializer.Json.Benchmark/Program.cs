using FluentSerializer.Core.BenchmarkUtils.Runner;
using FluentSerializer.Json.Benchmark.Data;
using System;
using System.Security.Permissions;

namespace FluentSerializer.Json.Benchmark
{
    public static class Program
	{
		[STAThread, PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
		public static void Main()
		{
			StaticTestRunner.RequireElevatedPermissions();
			JsonDataCollection.Default.GenerateTestCaseFiles();

            StaticTestRunner.Run(typeof(Program).Assembly, "json");
        }
    }
}

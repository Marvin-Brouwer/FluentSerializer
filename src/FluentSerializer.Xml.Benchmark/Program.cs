using FluentSerializer.Core.BenchmarkUtils.Runner;
using FluentSerializer.Xml.Benchmark.Data;
using System;
using System.Security.Permissions;

namespace FluentSerializer.Xml.Benchmark
{
    public static class Program
    {
        [STAThread, PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
		public static void Main()
		{
			StaticTestRunner.RequireElevatedPermissions();
			XmlDataCollection.Default.GenerateTestCaseFiles();

            StaticTestRunner.Run(typeof(Program).Assembly, "xml-serializer");
        }
    }
}

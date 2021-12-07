using FluentSerializer.Core.Profiling.Runner;
using FluentSerializer.Json.Profiling.Data;
using System;
using System.Security.Permissions;

namespace FluentSerializer.Json.Profiling
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

using FluentSerializer.Core.Profiling.Runner;
using FluentSerializer.Json.Profiling.Data;
using System;
using System.Security.Permissions;

namespace FluentSerializer.Json.Profiling
{
    public static class Program
	{
		[STAThread, PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
		public static void Main(params string[] parameters)
		{
			StaticTestRunner.RequireElevatedPermissions();
			JsonDataCollection.Default.GenerateTestCaseFiles();

            StaticTestRunner.Run(parameters, typeof(Program).Assembly);
        }
    }
}

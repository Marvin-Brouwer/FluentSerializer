using FluentSerializer.Core.BenchmarkUtils.Runner;
using System;

#if !NET5_0_OR_GREATER
using System.Security.Permissions;
#endif

namespace FluentSerializer.Core.Benchmark;

public static class Program
{
	[STAThread]
#if !NET5_0_OR_GREATER
	[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
#endif
	public static void Main(params string[] arguments)
	{
		StaticTestRunner.RequireElevatedPermissions();
		StaticTestRunner.Run(typeof(Program).Assembly, arguments, "core");
	}
}
using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Reflection;
using FluentSerializer.Core.Profiling.Configuration;
using BenchmarkDotNet.Environments;
using System.Security.Permissions;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;
using Perfolizer.Horology;

#if (DEBUG)
using BenchmarkDotNet.Toolchains.InProcess.Emit;
#endif

namespace FluentSerializer.Core.Profiling.Runner
{
	public abstract class StaticTestRunner
    {
        private static ManualConfig CreateConfig()
        {
            var config = ManualConfig.Create(DefaultConfig.Instance)
                .AddExporter(PlainExporter.Default)
                .WithOrderer(new GroupedSlowestToFastestOrderer())
                .AddJob(CreateJob(CoreRuntime.Core31))
                .AddJob(CreateJob(CoreRuntime.Core50))
                .AddExporter(PlainExporter.Default, MarkdownExporter.GitHub);
#if (DEBUG)
            config = config.WithOptions(ConfigOptions.DisableOptimizationsValidator);
#endif

            return config;
        }

        private static Job CreateJob(Runtime runtime)
        {
            return Job.Dry
                .WithRuntime(runtime)
#if (DEBUG)
                .WithLaunchCount(1)
                .WithToolchain(new InProcessEmitToolchain(TimeSpan.FromHours(1.0), true))
#else
                .WithLaunchCount(5)
                .WithWarmupCount(3)
#endif
				.WithIterationCount(1)
				.WithMinIterationTime(TimeInterval.FromMilliseconds(10))
				.WithMinIterationCount(1)
				.WithMaxRelativeError(0.01)
                .WithId(typeof(BenchmarkRunner).Assembly.FullName);
        }

		[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
		public static void Run(string[] parameters, Assembly assembly)
		{
			RequireElevatedPermissions();

			var config = CreateConfig();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Starting benchmark runner...");
			Console.ResetColor();
			Console.WriteLine();

			BenchmarkSwitcher.FromAssembly(assembly).RunAllJoined(config);
		}

		public static void RequireElevatedPermissions()
		{
			if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !IsAdministrator()) Elevate();
		}

		private static void Elevate()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine();
			Console.WriteLine("Restarting process with admin permissions.");

			// Restart program and run as admin
			var exeName = Process.GetCurrentProcess().MainModule.FileName;
			var startInfo = new ProcessStartInfo(exeName);
			startInfo.UseShellExecute = true;
			startInfo.Verb = "runas";

			Process.Start(startInfo);
			Environment.Exit(0);
		}

		private static bool IsAdministrator()
		{
			try
			{
				var user = WindowsIdentity.GetCurrent();
				var principal = new WindowsPrincipal(user);
				return principal.IsInRole(WindowsBuiltInRole.Administrator);
			}
			catch
			{
				return false;
			}
		}
	}
}

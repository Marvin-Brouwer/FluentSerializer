using System;
using System.Collections.Generic;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Reflection;
using FluentSerializer.Core.BenchmarkUtils.Configuration;
using System.Security.Permissions;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;
using Perfolizer.Horology;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Columns;
using System.Threading;
using System.Globalization;
using Microsoft.Extensions.PlatformAbstractions;
using BenchmarkDotNet.Reports;

#if (DEBUG)
using BenchmarkDotNet.Toolchains.InProcess.Emit;
#endif

namespace FluentSerializer.Core.BenchmarkUtils.Runner
{
	public abstract class StaticTestRunner
    {
		public static readonly CultureInfo AppCulture = new(CultureInfo.InvariantCulture.Name)
		{
			NumberFormat = NumberFormatInfo.InvariantInfo
		};

        private static ManualConfig CreateConfig()
        {
			Environment.SetEnvironmentVariable("COMPlus_gcAllowVeryLargeObjects", "1");
			Environment.SetEnvironmentVariable("DOTNET_gcAllowVeryLargeObjects", "1");

			Thread.CurrentThread.CurrentCulture = AppCulture;
			Thread.CurrentThread.CurrentUICulture = AppCulture;

			var config = ManualConfig.Create(DefaultConfig.Instance)
                .WithOrderer(new ValueSizeTestOrderer())
                .AddJob(CreateJob())
				.WithCultureInfo(AppCulture)
                .AddExporter(MarkdownExporter.Console)
				.WithSummaryStyle(SummaryStyle.Default
					.WithCultureInfo(AppCulture)
					// We'd actually like it to grow when the size grows but this doesn't seem to be consistent between
					// the XML and JSON benchmarks so we set it to a constant metric.
					.WithSizeUnit(SizeUnit.MB)
					.WithTimeUnit(TimeUnit.Millisecond));

			// We only ever profile methods so no need for an additional column
			var columnProviders = (List<IColumnProvider>)config.GetColumnProviders();
			columnProviders.RemoveAt(0);
			columnProviders.Insert(0, MethodOnlyDescriptorColumnProvider.Default);

#if (DEBUG)
            config = config.WithOptions(ConfigOptions.DisableOptimizationsValidator);
#endif
			return config;
        }

        private static Job CreateJob()
        {
            return Job.Dry
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
				// Make sure the compile projects have access to the correct build tool
                .WithNuGet("Microsoft.Net.Compilers.Toolset")
                .WithId(typeof(BenchmarkRunner).Assembly.FullName)
				.WithEnvironmentVariable("COMPlus_gcAllowVeryLargeObjects", Environment.GetEnvironmentVariable("COMPlus_gcAllowVeryLargeObjects") ?? "0")
				.WithEnvironmentVariable("DOTNET_gcAllowVeryLargeObjects", Environment.GetEnvironmentVariable("DOTNET_gcAllowVeryLargeObjects") ?? "0")
				.WithGcForce(true)
				// This is set to false until the new benchmark dotnet version is released:
				// https://github.com/dotnet/BenchmarkDotNet/issues/1519
				.WithGcAllowVeryLargeObjects(false);
        }

		[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
		public static void Run(Assembly assembly, string dataType)
		{
			RequireElevatedPermissions();

			var config = CreateConfig();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Starting benchmark runner...");
			Console.ResetColor();
			Console.WriteLine();

			BenchmarkSwitcher.FromAssembly(assembly).RunAllJoined(config);

			FixFileNames(dataType, config);
		}

		/// <summary>
		/// Manually fix file names, the markdown exporter doesn't allow for inheritance so we'll fix it ourselves;
		/// </summary>
		private static void FixFileNames(string dataType, ManualConfig config)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine();
			Console.WriteLine("Correcting fileName");
			Console.ResetColor();

			var resultsDir = new DirectoryInfo(Path.Join(config.ArtifactsPath, "results"));
			var markdownSummaryFile = resultsDir
				.GetFiles("*.md")
				.OrderByDescending(directory => directory.CreationTimeUtc)
				.FirstOrDefault();

			if (markdownSummaryFile is null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No summary found with pattern \"*.md\"");
				Console.ResetColor();
				Console.WriteLine();
				return;
			}

			var oldName = markdownSummaryFile.FullName;

			var runtimeName = PlatformServices.Default.Application.RuntimeFramework.Identifier[1..].ToLowerInvariant();
			var runtimeVersion = PlatformServices.Default.Application.RuntimeFramework.Version.ToString().Replace('.', '_');
			var readableFileName = markdownSummaryFile.FullName
				.Replace("BenchmarkRun-joined", $"{dataType}-benchmark-{runtimeName}_{runtimeVersion}")
				.Replace("-report-console", string.Empty);

			Console.WriteLine("Renaming report");
			Console.WriteLine($"  from: \"{oldName}\"");
			Console.WriteLine($"  to: \"{readableFileName}\"");
			Console.WriteLine();
			markdownSummaryFile.MoveTo(readableFileName);
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
			var startInfo = new ProcessStartInfo(exeName)
			{
				UseShellExecute = true,
				Verb = "runas"
			};

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

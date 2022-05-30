using System;
using System.Collections.Generic;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Reflection;
using FluentSerializer.Core.BenchmarkUtils.Configuration;
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

namespace FluentSerializer.Core.BenchmarkUtils.Runner;

public abstract class StaticTestRunner
{
	public static readonly CultureInfo AppCulture = new(CultureInfo.InvariantCulture.Name)
	{
		NumberFormat = NumberFormatInfo.InvariantInfo
	};

	private static ManualConfig CreateConfig(string[] arguments)
	{
		Environment.SetEnvironmentVariable("COMPlus_gcAllowVeryLargeObjects", "1");
		Environment.SetEnvironmentVariable("DOTNET_gcAllowVeryLargeObjects", "1");

		Thread.CurrentThread.CurrentCulture = AppCulture;
		Thread.CurrentThread.CurrentUICulture = AppCulture;

		var config = ManualConfig.Create(DefaultConfig.Instance)
			.WithOrderer(new ValueSizeTestOrderer())
			.AddJob(CreateJob(arguments))
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

	private static Job CreateJob(string[] parameters)
	{
		var quickRun = parameters.Contains("--quick");
		return CreateBasicJob(quickRun)
#if (DEBUG)
				.WithToolchain(new InProcessEmitToolchain(TimeSpan.FromHours(1.0), true))
#endif
				.WithMinIterationTime(TimeInterval.FromMilliseconds(10))
			.WithMinIterationCount(1)
			.WithMaxRelativeError(0.0001)
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

	private static Job CreateBasicJob(bool quickRun)
	{
#if DEBUG
		_ = quickRun;

		return Job.Dry
			.WithLaunchCount(1)
			.WithIterationCount(1);
#else
			if (quickRun) return Job.Dry
                .WithLaunchCount(1)
                .WithIterationCount(1);

			return Job.Dry
				.WithWarmupCount(32)
				.WithLaunchCount(4)
				.WithIterationCount(8);
#endif
	}

#if (!NET6_0_OR_GREATER)
		[System.Security.Permissions.PrincipalPermission(
			System.Security.Permissions.SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
#endif
	public static void Run(Assembly assembly, string[] arguments, string dataType)
	{
		RequireElevatedPermissions();

		var config = CreateConfig(arguments);

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("Starting benchmark runner...");
		Console.ResetColor();
		Console.WriteLine();

		BenchmarkSwitcher.FromAssembly(assembly).RunAllJoined(config);

		FixConsoleArtifactFileName(dataType, config);
		FixGitHubSummaryFileName(dataType, config);
	}

	/// <summary>
	/// Manually fix file names, the markdown exporter doesn't allow for inheritance so we'll fix it ourselves;
	/// </summary>
	private static void FixConsoleArtifactFileName(string dataType, ManualConfig config)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine();
		Console.WriteLine("Correcting console summary fileName");
		Console.ResetColor();

		var consoleFilePattern = "BenchmarkRun-joined-*-report-console.md";
		var markdownSummaryFile = FindFileName(consoleFilePattern, config);
		if (markdownSummaryFile is null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"No summary found with pattern \"{consoleFilePattern}\"");
			Console.ResetColor();
			Console.WriteLine();
			return;
		}

		var runtimeName = PlatformServices.Default.Application.RuntimeFramework.Identifier[1..].ToLowerInvariant();
		var runtimeVersion = PlatformServices.Default.Application.RuntimeFramework.Version.ToString().Replace('.', '_');
		var readableFileName = markdownSummaryFile.FullName
			.Replace("BenchmarkRun-joined", $"{dataType}-benchmark-{runtimeName}_{runtimeVersion}")
			.Replace("-report-console", string.Empty);

		FixFileNames(markdownSummaryFile, readableFileName);
	}

	/// <summary>
	/// Manually fix file names, the markdown exporter doesn't allow for inheritance so we'll fix it ourselves;
	/// </summary>
	private static void FixGitHubSummaryFileName(string dataType, ManualConfig config)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine();
		Console.WriteLine("Correcting GitHub summary filename");
		Console.ResetColor();

		var gitHubFilePattern = "BenchmarkRun-joined-*-report-console.md";
		var markdownSummaryFile = FindFileName(gitHubFilePattern, config);
		if (markdownSummaryFile is null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"No summary found with pattern \"{gitHubFilePattern}\"");
			Console.ResetColor();
			Console.WriteLine();
			return;
		}

		var runtimeName = PlatformServices.Default.Application.RuntimeFramework.Identifier[1..].ToLowerInvariant();
		var runtimeVersion = PlatformServices.Default.Application.RuntimeFramework.Version.ToString().Replace('.', '_');
		var readableFileName = $"{dataType}-benchmark-{runtimeName}_{runtimeVersion}-github.md";
		var parentDirectory = markdownSummaryFile.Directory!.Parent!;

		FixFileNames(markdownSummaryFile, Path.Join(parentDirectory.FullName, readableFileName));
	}

	private static FileInfo? FindFileName(string pattern, ManualConfig config)
	{
		var resultsDir = new DirectoryInfo(Path.Join(config.ArtifactsPath, "results"));
		var markdownSummaryFile = resultsDir
			.GetFiles(pattern)
			.OrderByDescending(directory => directory.CreationTimeUtc)
			.FirstOrDefault();

		return markdownSummaryFile;
	}


	/// <summary>
	/// Manually fix file names, the markdown exporter doesn't allow for inheritance so we'll fix it ourselves;
	/// </summary>
	private static void FixFileNames(FileInfo markdownSummaryFile, string readableFilePath)
	{
		var newFileInfo = new FileInfo(readableFilePath);

		Console.WriteLine("Copying report");
		Console.WriteLine($"  from: \"{markdownSummaryFile.FullName}\"");
		Console.WriteLine($"  to: \"{newFileInfo.FullName}\"");
		Console.WriteLine();

		markdownSummaryFile.CopyTo(newFileInfo.FullName, newFileInfo.Exists);
	}

	public static void RequireElevatedPermissions()
	{
		if (!IsWindowsAdministrator()) ElevateWindowsApp();
	}

	private static void ElevateWindowsApp()
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine();
		Console.WriteLine("Restarting process with admin permissions.");

		// Restart program and run as admin
		var exeName = GetProcessFileName();
		var startInfo = new ProcessStartInfo(exeName)
		{
			UseShellExecute = true,
			Verb = "runas"
		};

		Process.Start(startInfo);
		Environment.Exit(0);
	}

#if (NET6_0_OR_GREATER)
	private static string GetProcessFileName() => Environment.ProcessPath!;
#else
		private static string GetProcessFileName() => Process.GetCurrentProcess().MainModule.FileName;
#endif


	private static bool IsWindowsAdministrator()
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return false;

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

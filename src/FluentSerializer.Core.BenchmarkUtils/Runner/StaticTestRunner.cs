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
using BenchmarkDotNet.Order;
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

	private static ManualConfig CreateConfig(string[] arguments, IOrderer? orderer)
	{
		Environment.SetEnvironmentVariable("COMPlus_gcAllowVeryLargeObjects", "1");
		Environment.SetEnvironmentVariable("DOTNET_gcAllowVeryLargeObjects", "1");

		Thread.CurrentThread.CurrentCulture = AppCulture;
		Thread.CurrentThread.CurrentUICulture = AppCulture;

		var config = ManualConfig.Create(DefaultConfig.Instance)
			.AddJob(CreateJob(arguments))
			.WithCultureInfo(AppCulture)
			.AddExporter(MarkdownExporter.Console)
			.WithSummaryStyle(SummaryStyle.Default
				.WithCultureInfo(AppCulture)
				// We'd actually like it to grow when the size grows but this doesn't seem to be consistent between
				// the XML and JSON benchmarks so we set it to a constant metric.
				.WithSizeUnit(SizeUnit.MB)
				.WithTimeUnit(TimeUnit.Millisecond)
			);

		if (orderer is not null)
			config = config.WithOrderer(orderer);

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
		return CreateBasicJob(parameters)
#if (DEBUG)
			.WithToolchain(new InProcessEmitToolchain(TimeSpan.FromHours(1.0), true))
#endif
			.WithMinIterationTime(TimeInterval.FromMilliseconds(10))
			.WithMinIterationCount(1)
#if (!DEBUG)
			.WithMaxRelativeError(0.001)
			.WithMaxAbsoluteError(TimeInterval.FromNanoseconds(10))
#endif
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

	private static Job CreateBasicJob(string[] parameters)
	{
#if DEBUG
		_ = parameters;

		return Job.Dry;
#else
		var runType = parameters.FirstOrDefault(parameter => parameter.StartsWith("--jobType="));

		Console.ForegroundColor = ConsoleColor.DarkGray;
		if (runType is not null) Console.WriteLine(runType);
		Console.ResetColor();
		Console.WriteLine();

		return runType switch
		{
			null => Job.Default,
			"--jobType=Dry" => Job.Dry,
			"--jobType=Short" => Job.ShortRun,
			"--jobType=Long" => Job.LongRun,
			"--jobType=VeryLong" => Job.VeryLongRun,
			_ => Job.Default
		};
#endif
	}

#if (!NET6_0_OR_GREATER)
		[System.Security.Permissions.PrincipalPermission(
			System.Security.Permissions.SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
#endif
	public static void Run(Assembly assembly, in string[] arguments, string dataType, IOrderer? orderer = null)
	{
		RequireElevatedPermissions(in arguments);

		var jobDate = DateTime.UtcNow;
		var config = CreateConfig(arguments, orderer);

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("Starting benchmark runner...");
		Console.ResetColor();
		Console.WriteLine();

		BenchmarkSwitcher.FromAssembly(assembly).RunAllJoined(config);

		FixConsoleArtifactFileName(dataType, config, jobDate);
		FixGitHubSummaryFileName(dataType, config);


		if (!arguments.Contains("--wait-on-exit")) return;
		Console.WriteLine("Press any key to exit.");
		Console.ReadKey();
	}

	/// <summary>
	/// Manually fix file names, the markdown exporter doesn't allow for inheritance so we'll fix it ourselves;
	/// </summary>
	private static void FixConsoleArtifactFileName(string dataType, ManualConfig config, DateTime jobDate)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine();
		Console.WriteLine("Correcting console summary fileName");
		Console.ResetColor();

		var consoleFilePattern = "*-report-console.md";
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
		var readableFileName = $"{dataType}-benchmark-{runtimeName}_{runtimeVersion}-{jobDate:yyyy_MM_dd-HH_mm_ss}.md";
		var directory = markdownSummaryFile.Directory!;

		FixFileNames(markdownSummaryFile, Path.Join(directory.FullName, readableFileName));
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

		var gitHubFilePattern = "*-report-console.md";
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
#if NET5_0_OR_GREATER
			.MaxBy(directory => directory.CreationTimeUtc);
#else
			.OrderByDescending(directory => directory.CreationTimeUtc)
			.FirstOrDefault();
#endif

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

	public static void RequireElevatedPermissions(in string[] arguments)
	{
		if (!IsWindowsAdministrator()) ElevateWindowsApp(in arguments);
	}

	private static void ElevateWindowsApp(in string[] arguments)
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine();
		Console.WriteLine("Restarting process with admin permissions.");
		Console.ResetColor();

		// Restart program and run as admin
		var exeName = GetProcessFileName();
		var startInfo = new ProcessStartInfo(exeName)
		{
			UseShellExecute = true,
			Verb = "runas"
		};

		foreach (var argument in arguments)
			startInfo.ArgumentList.Add(argument);

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

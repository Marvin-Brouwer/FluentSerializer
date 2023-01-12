using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Extensions;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using FluentSerializer.Core.BenchmarkUtils.Configuration;

using Perfolizer.Horology;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Reflection.Metadata;

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
			.StopOnFirstError(true)
			.WithOptions(ConfigOptions.GenerateMSBuildBinLog)
			.WithSummaryStyle(SummaryStyle.Default
				.WithCultureInfo(AppCulture)
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
		var runType = Array.Find(parameters, parameter => parameter.StartsWith("--jobType=", StringComparison.Ordinal));

		return CreateBasicJob(runType)
#if (DEBUG)
			.WithToolchain(new InProcessEmitToolchain(TimeSpan.FromHours(1.0), true))
#endif
			.WithMinIterationTime(TimeInterval.FromMilliseconds(10))
			.WithMinIterationCount(1)
			.WithMaxRelativeError(0.001)
			.WithMaxAbsoluteError(TimeInterval.FromNanoseconds(10))
			// Make sure the compile projects have access to the correct build tool
			.WithNuGet("Microsoft.Net.Compilers.Toolset")
			.WithId(typeof(BenchmarkRunner).Assembly!.FullName!)
			.WithEnvironmentVariable("COMPlus_gcAllowVeryLargeObjects", Environment.GetEnvironmentVariable("COMPlus_gcAllowVeryLargeObjects") ?? "0")
			.WithEnvironmentVariable("DOTNET_gcAllowVeryLargeObjects", Environment.GetEnvironmentVariable("DOTNET_gcAllowVeryLargeObjects") ?? "0")
			// This is set to false until the new benchmark dotnet version is released:
			// https://github.com/dotnet/BenchmarkDotNet/issues/1519
			.WithGcAllowVeryLargeObjects(false)
			.WithGcForce(true)
			.WithPowerPlan(PowerPlan.HighPerformance);
	}

	private static Job CreateBasicJob(string? runType)
	{
#if DEBUG
		_ = runType;
		return Job.Dry;
#else

		Console.ForegroundColor = ConsoleColor.DarkGray;

		var parsedRunType = runType switch
		{
			null => Job.Default,
			"--jobType=Default" => Job.Default,
			"--jobType=Dry" => Job.Dry,
			"--jobType=Short" => Job.ShortRun,
			"--jobType=Medium" => Job.MediumRun,
			"--jobType=Long" => Job.LongRun,
			"--jobType=VeryLong" => Job.VeryLongRun,
			_ => null
		};

		if (parsedRunType is not null)
		{
			Console.WriteLine(runType);
		}
		else
		{
			Console.WriteLine($"Input '{runType}' is not a valid jobType");
			Console.WriteLine("Using '--jobType=Default' instead");
		}

		Console.ResetColor();
		Console.WriteLine();

		return parsedRunType ?? Job.Default;
#endif
	}

	public static void Run(Assembly assembly, in string[] arguments, string dataType, IOrderer? orderer = null)
	{
		RequireElevatedPermissions(in arguments);
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

		var jobDate = DateTime.UtcNow;
		var config = CreateConfig(arguments, orderer);
		if (arguments.Contains("--quick-exit"))
		{
			var cancellationValidator = CancellationValidator.Default;

			Console.WriteLine("Quick exit mode enabled.");
			config = config.AddValidator(cancellationValidator);
			Console.CancelKeyPress += (_, e) =>
			{
				Console.WriteLine("Cancellation signal recieved.");
				cancellationValidator.RequestCancellation();

				// Quick kill child processes
				Process.GetCurrentProcess().KillTree(TimeSpan.Zero);
				Process.GetCurrentProcess().Kill();
			};
		}

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("Starting benchmark runner...");
		Console.ResetColor();
		Console.WriteLine();

		BenchmarkSwitcher.FromAssembly(assembly).RunAllJoined(config);

		var osDisplayNameParam = Array.Find(arguments, parameter => parameter.StartsWith("-os-displayName=", StringComparison.Ordinal));
		var osDisplayName = osDisplayNameParam?.Split('=')[1];
		FixConsoleArtifactFileName(dataType, config, jobDate, in osDisplayName);
		var gitHubSummaryFileName = FixGitHubSummaryFileName(dataType, config);
		if (gitHubSummaryFileName is not null) WrapGitHubFileSummary(gitHubSummaryFileName);

		// Ring a bell if posible to signal done
		Console.Write("\a");
		if (!arguments.Contains("--wait-on-exit")) return;
		Console.WriteLine("Press any key to exit.");
		Console.ReadKey();
	}

	/// <summary>
	/// Manually fix file names, the markdown exporter doesn't allow for inheritance so we'll fix it ourselves;
	/// </summary>
	private static void FixConsoleArtifactFileName(string dataType, ManualConfig config, DateTime jobDate, in string? osDisplayName)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine();
		Console.WriteLine("Correcting console summary fileName");
		Console.ResetColor();

		const string consoleFilePattern = "*-report-console.md";
		var markdownSummaryFile = FindFileName(consoleFilePattern, config);
		if (markdownSummaryFile is null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"No summary found with pattern \"{consoleFilePattern}\"");
			Console.ResetColor();
			Console.WriteLine();
			return;
		}

		var osName = GetOsName(in osDisplayName);
		var runtimeVersion = GetRuntimeVersion();
		var readableFileName = $"{dataType}-benchmark-{runtimeVersion}-{osName}-{jobDate:yyyy_MM_dd-HH_mm_ss}.md";
		var directory = markdownSummaryFile.Directory!;

#if NETSTANDARD2_0
		FixFileNames(markdownSummaryFile, string.Join(Path.DirectorySeparatorChar.ToString(), directory.FullName, readableFileName));
#else
		FixFileNames(markdownSummaryFile, Path.Join(directory.FullName, readableFileName));
#endif
	}

	private static string GetOsName(in string? osDisplayName)
	{
		if (!string.IsNullOrWhiteSpace(osDisplayName)) return osDisplayName!;

#if NET5_0_OR_GREATER
		var runtimeIdentifier = RuntimeInformation.RuntimeIdentifier;
#else
		var runtimeIdentifier = Environment.OSVersion.Platform.ToString();
		if (string.IsNullOrEmpty(runtimeIdentifier)) return "unknown";
#endif

		return runtimeIdentifier!
			.Split('-')[0]
			.Split('.')[0]
			.ToLowerInvariant();
	}

	private static string GetRuntimeVersion()
	{
		var versionString = typeof(object).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
		var plusIndex = versionString!.IndexOf('+');
		if (plusIndex != -1)
		{
			versionString = versionString[0..plusIndex];
		}
		var dashIndex = versionString!.IndexOf('-');
		if (dashIndex != -1)
		{
			versionString = versionString[0..dashIndex];
		}

		var version = new Version(versionString);

		var frameworkTag = RuntimeInformation.FrameworkDescription[1..]
			.Replace(versionString, string.Empty)
			.Replace(" ", string.Empty)
			.ToLowerInvariant();

		if (frameworkTag == "netcore") frameworkTag = "netcoreapp";

		return $"{frameworkTag}_{version.Major}_{version.Minor}";
	}

	/// <summary>
	/// Manually fix file names, the markdown exporter doesn't allow for inheritance so we'll fix it ourselves;
	/// </summary>
	private static string? FixGitHubSummaryFileName(string dataType, ManualConfig config)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine();
		Console.WriteLine("Correcting GitHub summary filename");
		Console.ResetColor();

		const string gitHubFilePattern = "*-report-console.md";
		var markdownSummaryFile = FindFileName(gitHubFilePattern, config);
		if (markdownSummaryFile is null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"No summary found with pattern \"{gitHubFilePattern}\"");
			Console.ResetColor();
			Console.WriteLine();
			return null;
		}

		var runtimeVersion = GetRuntimeVersion();
		var readableFileName = $"{dataType}-benchmark-{runtimeVersion}-github.md";
		var parentDirectory = markdownSummaryFile.Directory!.Parent!;
#if NETSTANDARD2_0
		var fullPath = string.Join(Path.DirectorySeparatorChar.ToString(), parentDirectory.FullName, readableFileName);
#else
		var fullPath = Path.Join(parentDirectory.FullName, readableFileName);
#endif

		FixFileNames(markdownSummaryFile, fullPath);
		return fullPath;
	}

	/// <summary>
	/// Since these summaries ended up not being really readable, wrap the tables in a txt block.
	/// </summary>
	private static void WrapGitHubFileSummary(string gitHubSummaryFileName)
	{
		using var content = File.Open(gitHubSummaryFileName, FileMode.Open);
		using var streamReader = new StreamReader(content);
		var text = streamReader.ReadToEnd();
		streamReader.Close();
		content.Close();

		text = text.Replace(
			$"{Environment.NewLine}{Environment.NewLine}|",
			$"{Environment.NewLine}{Environment.NewLine}```txt{Environment.NewLine}|");
		text += $"``` {Environment.NewLine}";

		File.WriteAllText(gitHubSummaryFileName, text);
	}

	private static FileInfo? FindFileName(string pattern, ManualConfig config)
	{
#if NETSTANDARD2_0
		var resultsDirPath = string.Join(Path.DirectorySeparatorChar.ToString(), config.ArtifactsPath, "results");
#else
		var resultsDirPath = Path.Join(config.ArtifactsPath, "results");
#endif
		var resultsDir = new DirectoryInfo(resultsDirPath);
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

#if NETSTANDARD2_0
		startInfo.Arguments = string.Join(" ", arguments);
#else
		foreach (var argument in arguments)
			startInfo.ArgumentList.Add(argument);
#endif

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
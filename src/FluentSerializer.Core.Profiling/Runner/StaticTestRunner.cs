using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Linq;
using System.Reflection;
using FluentSerializer.Core.Profiling.Configuration;
using BenchmarkDotNet.Environments;

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
                // Todo figure out how to make this work
                // maybe use this: https://www.cafe-encounter.net/p2312/multi-targetting-net-framework-and-net-core-in-a-single-project
                //.AddJob(CreateJob(ClrRuntime.Net48))
                .AddJob(CreateJob(CoreRuntime.Core31))
                .AddJob(CreateJob(CoreRuntime.Core60))
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
                .WithIterationCount(1)
                .WithToolchain(new InProcessEmitToolchain(TimeSpan.FromHours(1.0), true))
#else
                .WithLaunchCount(3)
                .WithWarmupCount(3)
                .WithIterationCount(3)
#endif
                .WithMaxRelativeError(0.01)
                .WithId(typeof(BenchmarkRunner).Assembly.FullName);
        }

        public static void Run(string[] parameters, Assembly assembly)
        {
            var config = CreateConfig();
            Console.WriteLine("Starting benchmark runner");

            if (!parameters.Any())
                BenchmarkSwitcher.FromAssembly(assembly).RunAllJoined(config);

            BenchmarkSwitcher.FromAssembly(assembly).Run(parameters, config);
        }
    }
}

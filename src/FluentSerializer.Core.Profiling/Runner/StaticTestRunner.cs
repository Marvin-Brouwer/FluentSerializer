using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Profiling.Runner
{
    public abstract class StaticTestRunner
    {
        protected static ManualConfig CreateConfig()
        {
            var config =
#if DEBUG
                new DebugInProcessConfig()
#else
                ManualConfig.CreateMinimumViable()
#endif
                .AddExporter(PlainExporter.Default)
                .AddJob(Job.Dry
                    .WithLaunchCount(2)
                    .WithWarmupCount(2)
                    .WithIterationCount(5)
                    .WithMaxRelativeError(0.01)
                    .WithId(typeof(BenchmarkRunner).Assembly.FullName)
                )
                .AddExporter(PlainExporter.Default, MarkdownExporter.GitHub);
#if (DEBUG)
            config = config.WithOptions(ConfigOptions.DisableOptimizationsValidator);
#endif

            return config;
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

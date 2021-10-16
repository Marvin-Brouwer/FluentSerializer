using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

namespace FluentSerializer.Core.Profiling.Move
{
    public abstract class StaticTestRunner
    {
        protected static ManualConfig CreateConfig()
        {
            var config = ManualConfig
                .CreateMinimumViable()
                .AddExporter(PlainExporter.Default)
                .AddJob(Job.Dry
                    .WithLaunchCount(5)
                    .WithMaxRelativeError(0.01)
                    .WithId(typeof(BenchmarkRunner).Assembly.FullName)
                );
            #if (DEBUG)
            config = config.WithOptions(ConfigOptions.DisableOptimizationsValidator);
            #endif

            return config;
        }

        public static void Run(string[] parameters, System.Collections.Generic.IEnumerable<Type> profileDefinitions)
        {
            var config = CreateConfig();
            var tests = profileDefinitions.ToArray();

            if (!parameters.Any())
                BenchmarkSwitcher.FromTypes(tests).RunAllJoined(config);
            BenchmarkSwitcher.FromTypes(tests).Run(parameters, config);
        }
    }
}

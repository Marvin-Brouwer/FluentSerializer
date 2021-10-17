using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Profiling.Move
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
                    .WithLaunchCount(5)
                    .WithMaxRelativeError(0.01)
                    .WithId(typeof(BenchmarkRunner).Assembly.FullName)
                );
#if (DEBUG)
            config = config.WithOptions(ConfigOptions.DisableOptimizationsValidator);
#endif

            return config;
        }

        public static void Run(string[] parameters, Assembly assembly)
        {
            var config = CreateConfig();

            if (!parameters.Any())
                BenchmarkSwitcher.FromAssembly(assembly).RunAllJoined(config);

            BenchmarkSwitcher.FromAssembly(assembly).Run(parameters, config);
        }
    }
}

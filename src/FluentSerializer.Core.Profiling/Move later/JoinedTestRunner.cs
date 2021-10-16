using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using FluentAssertions;
using System;
using System.Linq;
using Xunit.Abstractions;

namespace FluentSerializer.Core.Profiling.Move
{
    public abstract class JoinedTestRunner
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

        public static void Run<TData>(string[] parameters)
            where TData : TestDataConfiguration, new()
        {
            var config = CreateConfig();
            var tests = new TData().GetTestClasses();

            if (!parameters.Any())
                BenchmarkSwitcher.FromTypes(tests.ToArray()).RunAllJoined(config);
            BenchmarkSwitcher.FromTypes(tests.ToArray()).Run(parameters, config);
        }

        private readonly ITestOutputHelper _output;

        public JoinedTestRunner(ITestOutputHelper output)
        {
            _output = output;
        }

        public virtual void ProfileInTestExplorer(Type type)
        {
            // Arrange
            var config = CreateConfig()
                .AddLogger(new BenchmarToXUnitLogger(_output))
                .WithOptions(ConfigOptions.DisableLogFile);

            // Act
            var summary = BenchmarkRunner.Run(type, config);

            // Assert
            foreach (var error in summary.ValidationErrors) _output.WriteLine(error.Message);
            summary.HasCriticalValidationErrors.Should().BeFalse();
        }
    }
}

using FluentSerializer.Core.Profiling.Data;
using FluentSerializer.Core.Profiling.Move;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace FluentSerializer.Core.Profiling
{
    public sealed class BenchmarkTests : TestDataConfiguration
    {
        public override IEnumerable<Type> GetTestClasses()
        {
            yield return typeof(DataCompilerProfile);
        }
    }

    public static class Program
    {
        [STAThread] public static void Main(params string[] parameters) => JoinedTestRunner.Run<BenchmarkTests>(parameters);
    }
    public sealed class TestRunner : JoinedTestRunner
    {
        public TestRunner(ITestOutputHelper output) : base(output) { }

        [Theory, ClassData(typeof(BenchmarkTests))]
        public override void ProfileInTestExplorer(Type type) => base.ProfileInTestExplorer(type);
    }
}

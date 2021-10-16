using FluentSerializer.Core.Profiling.Data;
using FluentSerializer.Core.Profiling.Move;
using System;
using System.Collections.Generic;

namespace FluentSerializer.Core.Profiling
{
    public static class BenchmarkTests
    {
        [STAThread] public static void Main(params string[] parameters) => StaticTestRunner.Run(parameters, GetTestClasses());

        public static IEnumerable<Type> GetTestClasses()
        {
            yield return typeof(DataCompilerProfile);
        }
    }
}

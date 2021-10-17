using FluentSerializer.Core.Profiling.Move;
using System;

namespace FluentSerializer.Core.Profiling
{
    public static class BenchmarkTests
    {
        [STAThread] public static void Main(params string[] parameters) => 
            StaticTestRunner.Run(parameters, typeof(BenchmarkTests).Assembly);
    }
}

using FluentSerializer.Core.Profiling.Runner;
using System;

namespace FluentSerializer.Xml.Profiling
{
    public static class Program
    {
        [STAThread] public static void Main(params string[] parameters) =>
            StaticTestRunner.Run(parameters, typeof(Program).Assembly);
    }
}

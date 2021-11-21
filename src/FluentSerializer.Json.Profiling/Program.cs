using FluentSerializer.Core.Profiling.Runner;
using FluentSerializer.Json.Profiling.Data;
using System;

namespace FluentSerializer.Json.Profiling
{
    public static class Program
    {
        [STAThread]
        public static void Main(params string[] parameters)
        {
            JsonDataCollection.Default.GenerateStringFiles();

            StaticTestRunner.Run(parameters, typeof(Program).Assembly);
        }
    }
}

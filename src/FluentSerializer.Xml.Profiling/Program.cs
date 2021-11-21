using FluentSerializer.Core.Profiling.Runner;
using FluentSerializer.Xml.Profiling.Data;
using System;

namespace FluentSerializer.Xml.Profiling
{
    public static class Program
    {
        [STAThread]
        public static void Main(params string[] parameters)
        {
            XmlDataCollection.Default.GenerateStringFiles();

            StaticTestRunner.Run(parameters, typeof(Program).Assembly);
        }
    }
}

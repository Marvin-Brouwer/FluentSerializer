using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using System.Collections.Generic;

namespace FluentSerializer.Core.Profiling.Move
{        
    public class Exporter : IExporter
    {
        public string Name => "TestExporter";

        public IEnumerable<string> ExportToFiles(Summary summary, ILogger consoleLogger)
        {
            throw new System.NotImplementedException();
        }

        public void ExportToLog(Summary summary, ILogger logger)
        {
            throw new System.NotImplementedException();
        }
    }
}
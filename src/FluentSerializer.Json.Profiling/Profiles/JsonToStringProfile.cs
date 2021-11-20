using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiling.Data;

namespace FluentSerializer.Json.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class JsonToStringProfile
    {
        private MemoryStream WriteStream { get; set; }
        private StreamWriter StreamWriter { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            WriteStream = new MemoryStream();
            StreamWriter = new StreamWriter(WriteStream);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            StreamWriter.Dispose();
            WriteStream.Dispose();
            StreamWriter = null;
            WriteStream = null;
        }

        public static IEnumerable<DataContainer<IJsonObject>> Inputs => JsonDataSet.JsonValues;

        [ParamsSource(nameof(Inputs))]
        public DataContainer<IJsonObject> Input { get; set; }

        [Benchmark, BenchmarkCategory("WriteTo")]
        public void WriteTo()
        {
            var result = Input.Value.WriteTo(JsonDataSet.StringFastPool, true);
            _ = result;
        }
    }
}

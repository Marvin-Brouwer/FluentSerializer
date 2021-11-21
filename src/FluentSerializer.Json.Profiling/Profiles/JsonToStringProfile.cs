using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Tests.Helpers;
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

            JsonDataCollection.Default.GenerateObjectData();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            StreamWriter.Dispose();
            WriteStream.Dispose();
            StreamWriter = null;
            WriteStream = null;
        }

        [Benchmark, BenchmarkCategory("WriteTo")]
        public void JsonToString()
        {
            var value = JsonDataCollection.Default.ObjectTestData;

            value.WriteTo(TestStringBuilderPool.StringFastPool, true);
            StreamWriter.Flush();
            _ = Encoding.UTF8.GetString(WriteStream.ToArray());
        }
    }
}

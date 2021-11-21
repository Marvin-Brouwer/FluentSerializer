using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Json.Profiling.Data;

namespace FluentSerializer.Json.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class StringToJsonProfile
    {

        [Benchmark, BenchmarkCategory("Parse")]
        public void StringToJson()
        {
            using var textStream = JsonDataCollection.Default.StringTestData;
            using var reader = new StreamReader(textStream);
            
            JsonParser.Parse(reader.ReadToEnd());
        }
    }
}

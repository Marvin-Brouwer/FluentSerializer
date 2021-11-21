using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.Profiling.Data;

namespace FluentSerializer.Json.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class StringToJsonProfile
    {
        public static IEnumerable<DataContainer<string>> Inputs => JsonDataSet.JsonStringValues();

        [ParamsSource(nameof(Inputs))]
        public DataContainer<string> Input { get; set; }


        [Benchmark, BenchmarkCategory("Parse")]
        public void ParseJson()
        {
            JsonParser.Parse(Input.Value);
        }
    }
}

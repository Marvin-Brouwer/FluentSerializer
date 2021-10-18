using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Order;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiling.Data;

namespace FluentSerializer.Json.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    [ThreadingDiagnoser]
    #if (!DEBUG)
    //[EtwProfiler]
    //[ConcurrencyVisualizerProfiler]
    //[NativeMemoryProfiler]
    #endif
    [Orderer(SummaryOrderPolicy.SlowestToFastest)]
    public class JsonToStringProfile
    {
        public static IEnumerable<JsonObject> GetValues() => JsonDataSet.JsonValues;

        [Benchmark(Description = nameof(JsonDataToString)), BenchmarkCategory("ToString", "Json")]
        [ArgumentsSource(nameof(GetValues))]
        public void JsonDataToString(JsonObject data)
        {
            data.ToString(true);
            data.ToString(false);
        }
    }
}

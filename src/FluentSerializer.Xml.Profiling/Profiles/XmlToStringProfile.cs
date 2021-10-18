using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Order;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Profiling.Data;

namespace FluentSerializer.Xml.Profiling.Profiles
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
    public class XmlToStringProfile
    {
        public static IEnumerable<XmlElement> GetValues() => XmlDataSet.XmlValues;


        [Benchmark(Description = nameof(XmlDataToString)), BenchmarkCategory("ToString", "Xml")]
        [ArgumentsSource(nameof(GetValues))]
        public void XmlDataToString(XmlElement data)
        {
            data.ToString(true);
            data.ToString(false);
        }
    }
}

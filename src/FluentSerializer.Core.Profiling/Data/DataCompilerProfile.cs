using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Order;
using FluentSerializer.Core.Data;
using FluentSerializer.Core.Data.Json;
using FluentSerializer.Core.Data.Xml;
using FluentSerializer.Core.Profiling.Data.TestData;
using System.Linq;

namespace FluentSerializer.Core.Profiling.Data
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
    [Orderer(SummaryOrderPolicy.Declared)]
    public class DataCompilerProfile
    {
        public static IEnumerable<JsonObject> GetJsonValues() => TestDataSet.JsonValues;
        public static IEnumerable<XmlElement> GetXmlValues() => TestDataSet.XmlValues;


        [Benchmark(Description = nameof(JsonDataToString)), BenchmarkCategory("ToString", "Json")]
        [ArgumentsSource(nameof(GetJsonValues))]
        public void JsonDataToString(JsonObject data)
        {
            data.ToString(true);
            data.ToString(false);
        }

        [Benchmark(Description = nameof(SerialJsonWriter)), BenchmarkCategory("ISerialWriter", "Json")]
        [ArgumentsSource(nameof(GetJsonValues))]
        public void SerialJsonWriter(JsonObject data)
        {
            new SerialJsonWriter(true, true).Write(data);
            new SerialJsonWriter(false, true).Write(data);
        }

        [Benchmark(Description = nameof(XmlDataToString)), BenchmarkCategory("ToString", "Xml")]
        [ArgumentsSource(nameof(GetXmlValues))]
        public void XmlDataToString(XmlElement data)
        {
            data.ToString(true);
            data.ToString(false);
        }

        [Benchmark(Description = nameof(SerialXmlWriter)), BenchmarkCategory("ISerialWriter", "Xml")]
        [ArgumentsSource(nameof(GetXmlValues))]
        public void SerialXmlWriter(XmlElement data)
        {
            new SerialXmlWriter(true, true).Write(data);
            new SerialXmlWriter(false, true).Write(data);
        }
    }
}

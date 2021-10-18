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
        private const int LargeSet = 5000;
        private const int MiddleSet = 500;
        private const int SmallSet = 100;

        private static readonly TestDataSet TestData = new(LargeSet, MiddleSet, SmallSet);

        public static IEnumerable<JsonObject> GetJsonValues()
        {
            Console.WriteLine("GetJsonValues");
            return TestData.JsonValues;
        }
        public static IEnumerable<XmlElement> GetXmlValues()
        {
            Console.WriteLine("GetXmlValues");
            return TestData.XmlValues;
        }


        [ParamsSource(nameof(GetJsonValues))]
        public JsonObject JsonTestData { get; set; }
        [ParamsSource(nameof(GetXmlValues))]
        public XmlElement XmlTestData { get; set; }

        [Benchmark(Description = nameof(JsonDataToString)), BenchmarkCategory("ToString", "Json")]
        public void JsonDataToString()
        {
            Console.WriteLine("JsonDataToString");
            JsonTestData.ToString(true);
            JsonTestData.ToString(false);
        }

        [Benchmark(Description = nameof(SerialJsonWriter)), BenchmarkCategory("ISerialWriter", "Json")]
        public void SerialJsonWriter()
        {
            Console.WriteLine("SerialJsonWriter");
            new SerialJsonWriter(true, true).Write(JsonTestData);
            new SerialJsonWriter(false, true).Write(JsonTestData);
        }

        [Benchmark(Description = nameof(XmlDataToString)), BenchmarkCategory("ToString", "Xml")]
        public void XmlDataToString()
        {
            Console.WriteLine("XmlDataToString");
            XmlTestData.ToString(true);
            XmlTestData.ToString(false);
        }

        [Benchmark(Description = nameof(SerialXmlWriter)), BenchmarkCategory("ISerialWriter", "Xml")]
        public void SerialXmlWriter()
        {
            Console.WriteLine("SerialXmlWriter");
            new SerialXmlWriter(true, true).Write(XmlTestData);
            new SerialXmlWriter(false, true).Write(XmlTestData);
        }
    }
}

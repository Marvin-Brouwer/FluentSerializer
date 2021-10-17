using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
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
    public class DataCompilerProfile
    {
        private JsonObject _jsonTest;
        private XmlElement _xmlTest;
        private int _iteration;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _iteration = -1;
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _iteration++;
            var testDataSet = BogusConfiguration.Generate(98123600 + _iteration);

            _jsonTest = new JsonObject(
                new JsonProperty("data", new JsonArray(testDataSet.Select(testItem => testItem.ToJsonElement())))
            );

            _xmlTest = new XmlElement("Data", 
                testDataSet.Select(testItem => testItem.ToXmlElement())
            );
        }

        [Benchmark(Description = nameof(JsonDataToSTring)), BenchmarkCategory("ToString", "Json")]
        public void JsonDataToSTring()
        {
            _jsonTest.ToString();
            _jsonTest.ToString(false);
        }

        [Benchmark(Description = nameof(XmlDataToSTring)), BenchmarkCategory("ToString", "Xml")]
        public void XmlDataToSTring()
        {
            _xmlTest.ToString();
            _xmlTest.ToString(false);
        }

        [Benchmark(Description = nameof(SerialJsonWriter)), BenchmarkCategory("ISerialWriter", "Json")]
        public void SerialJsonWriter()
        {
            new SerialJsonWriter(true, true).Write(_jsonTest);
            new SerialJsonWriter(false, true).Write(_jsonTest);
        }

        [Benchmark(Description = nameof(SerialXmlWriter)), BenchmarkCategory("ISerialWriter", "Xml")]
        public void SerialXmlWriter()
        {
            new SerialXmlWriter(true, true).Write(_xmlTest);
            new SerialXmlWriter(false, true).Write(_xmlTest);
        }
    }
}

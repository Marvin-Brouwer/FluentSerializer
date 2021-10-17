using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Data;
using FluentSerializer.Core.Data.Json;
using FluentSerializer.Core.Data.Xml;

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

        [IterationSetup]
        public void Setup()
        {
            // todo use Bogus or Autofixture
            _jsonTest = new JsonObject(
                new JsonProperty("prop", JsonValue.String("Test")),
                new JsonProperty("prop2", new JsonObject(
                    new JsonProperty("array", new JsonArray(
                        new JsonObject(),
                        new JsonArray()
                    )),
                    new JsonProperty("prop3", new JsonValue("1")),
                    new JsonProperty("prop4", new JsonValue("true")),
                    new JsonProperty("prop5", new JsonValue("null"))
                ))
            );

            _xmlTest = new XmlElement("Class",
                new XmlAttribute("someAttribute", "1"),
                new XmlElement("someProperty", new XmlElement("AnotherClass")),
                new XmlText("text here")
            );
        }

        [Benchmark]
        public void ContainerBasedSerializer()
        {
            _jsonTest.ToString();
            _jsonTest.ToString(false);
            _xmlTest.ToString();
            _xmlTest.ToString(false);
        }

        [Benchmark]
        public void ServiceBasedSerializer()
        {
            new SerialJsonWriter(true, true).Write(_jsonTest);
            new SerialJsonWriter(false, true).Write(_jsonTest);
            new SerialXmlWriter(true, true).Write(_xmlTest);
            new SerialXmlWriter(false, true).Write(_xmlTest);
        }
    }
}

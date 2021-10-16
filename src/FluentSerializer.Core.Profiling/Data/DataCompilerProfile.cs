using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Data;

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
        public IJsonNode JsonTest { get; private set; }
        public IXmlNode XmlTest { get; private set; }

        [IterationSetup]
        public void Setup()
        {
            JsonTest = new JsonObject(
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

            XmlTest = new XmlElement("Class",
                new XmlAttribute("someAttribute", "1"),
                new XmlElement("someProperty", new XmlElement("AnotherClass")),
                new XmlText("text here")
            );
        }

        [Benchmark]
        public void ContainerBasedSerializer()
        {
            var jsonResult = JsonTest.ToString();
            var jsonResultNonFormat = JsonTest.ToString(false);
            var xmlResult = XmlTest.ToString();
            var xmlResultNonFormat = XmlTest.ToString(false);
        }
        //public void ServiceBasedSerializer()
        //{

        //}
    }
}

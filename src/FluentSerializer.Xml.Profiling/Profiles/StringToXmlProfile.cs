using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Xml.Profiling.Data;

namespace FluentSerializer.Xml.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class StringToXmlProfile
    {
        [Benchmark, BenchmarkCategory("Parse")]
        public void StringToXml()
        {
            using var textStream = XmlDataCollection.Default.StringTestData;
            using var reader = new StreamReader(textStream);

            XmlParser.Parse(reader.ReadToEnd());
        }
    }
}

using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.Profiling.Data;

namespace FluentSerializer.Xml.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class StringToXmlProfile
    {
        public static IEnumerable<DataContainer<string>> Inputs => XmlDataSet.XmlStringValues;

        [ParamsSource(nameof(Inputs))]
        public DataContainer<string> Input { get; set; }


        [Benchmark, BenchmarkCategory("Parse")]
        public void ParseXml()
        {
            XmlParser.Parse(Input.Value);
        }
    }
}

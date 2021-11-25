using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.Profiling.Profilers;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.Profiling.Data;

namespace FluentSerializer.Xml.Profiling.Profiles
{
    public class XmlWriteProfile : WriteProfile
    {
        public IEnumerable<TestCase<Stream>> Values => XmlDataCollection.Default.StringTestData;

        [ParamsSource(nameof(Values))]
        public TestCase<Stream> Value { get => CaseValue; set => CaseValue = value; }

        [Benchmark, BenchmarkCategory(nameof(Write))]
        public void Write() => XmlParser.Parse(CaseReader.ReadToEnd());
    }
}

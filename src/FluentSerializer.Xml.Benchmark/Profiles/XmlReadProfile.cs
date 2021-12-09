using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.BenchmarkUtils.Profiles;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Xml.Benchmark.Data;

namespace FluentSerializer.Xml.Benchmark.Profiles
{
    public class XmlReadProfile : ReadProfile
    {
        public IEnumerable<TestCase<Stream>> Values => XmlDataCollection.Default.StringTestData;

        [ParamsSource(nameof(Values))]
        public TestCase<Stream> Value { get => CaseValue; set => CaseValue = value; }

        [Benchmark, BenchmarkCategory(nameof(ReadXml))]
        public void ReadXml() => XmlParser.Parse(CaseReader.ReadToEnd());
    }
}

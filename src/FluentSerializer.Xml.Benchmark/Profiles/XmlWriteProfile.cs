using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.BenchmarkUtils.Profilers;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Xml.Benchmark.Data;

namespace FluentSerializer.Xml.Benchmark.Profiles
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

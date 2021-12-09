using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.BenchmarkUtils.Profilers;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Xml.Benchmark.Data;

namespace FluentSerializer.Xml.Benchmark.Profiles
{
    public class XmlReadProfile : ReadProfile
    {
        public IEnumerable<TestCase<IDataNode>> Values() => XmlDataCollection.Default.ObjectTestData;

        [ParamsSource(nameof(Values))]
        public TestCase<IDataNode> Value { get; set; }

        [Benchmark, BenchmarkCategory(nameof(Read))]
        public void Read() => base.Read(Value.GetData());
    }
}

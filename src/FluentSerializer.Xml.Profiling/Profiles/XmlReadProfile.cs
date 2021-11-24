using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Profiling.Profilers;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.Profiling.Data;

namespace FluentSerializer.Xml.Profiling.Profiles
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

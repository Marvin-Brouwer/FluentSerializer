using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.BenchmarkUtils.Profilers;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Json.Benchmark.Data;

namespace FluentSerializer.Json.Benchmark.Profiles
{
    public class JsonReadProfile : ReadProfile
    {
        public IEnumerable<TestCase<IDataNode>> Values() => JsonDataCollection.Default.ObjectTestData;

        [ParamsSource(nameof(Values))]
        public TestCase<IDataNode> Value { get; set; }

        [Benchmark, BenchmarkCategory(nameof(Read))]
        public void Read() => base.Read(Value.GetData());
    }
}

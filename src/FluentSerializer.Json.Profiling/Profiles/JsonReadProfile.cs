using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Profiling.Profilers;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.Profiling.Data;

namespace FluentSerializer.Json.Profiling.Profiles
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

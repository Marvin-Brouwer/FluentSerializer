using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.BenchmarkUtils.Profilers;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Json.Benchmark.Data;

namespace FluentSerializer.Json.Benchmark.Profiles
{
    public class JsonWriteProfile : WriteProfile
    {
        public IEnumerable<TestCase<Stream>> Values => JsonDataCollection.Default.StringTestData;

        [ParamsSource(nameof(Values))]
        public TestCase<Stream> Value { get => CaseValue; set => CaseValue = value; }

        [Benchmark, BenchmarkCategory(nameof(Write))]
        public void Write() => JsonParser.Parse(CaseReader.ReadToEnd());
    }
}

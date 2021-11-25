using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.Profiling.Profilers;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.Profiling.Data;

namespace FluentSerializer.Json.Profiling.Profiles
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

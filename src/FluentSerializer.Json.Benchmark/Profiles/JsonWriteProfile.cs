using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.BenchmarkUtils.Profiles;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Json.Benchmark.Data;

namespace FluentSerializer.Json.Benchmark.Profiles;

public class JsonWriteProfile : WriteProfile
{
	public IEnumerable<TestCase<IDataNode>> Values() => JsonDataCollection.Default.ObjectTestData;

	[ParamsSource(nameof(Values))]
	public TestCase<IDataNode> Value { get; set; }

	[Benchmark, BenchmarkCategory(nameof(WriteJson))]
	public string WriteJson() => Write(Value.GetData());
}
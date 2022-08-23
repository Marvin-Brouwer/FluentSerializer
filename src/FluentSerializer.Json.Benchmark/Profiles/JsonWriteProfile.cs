using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.BenchmarkUtils.Profiles;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Benchmark.Data;

using System.Collections.Generic;

namespace FluentSerializer.Json.Benchmark.Profiles;

public class JsonWriteProfile : WriteProfile
{
	public static IEnumerable<TestCase<IDataNode>> Values() => JsonBenchmarkData.Default.ObjectTestData;

	[ParamsSource(nameof(Values))]
	public TestCase<IDataNode> Value { get; set; }

	[Benchmark, BenchmarkCategory(nameof(WriteJson))]
	public string WriteJson() => Write(Value);
}
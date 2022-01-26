using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.BenchmarkUtils.Profiles;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Json.Benchmark.Data;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Benchmark.Profiles;

public class JsonReadProfile : ReadProfile
{
	public IEnumerable<TestCase<Stream>> Values => JsonDataCollection.Default.StringTestData;

	[ParamsSource(nameof(Values))]
	public TestCase<Stream> Value { get => CaseValue; set => CaseValue = value; }

	[Benchmark, BenchmarkCategory(nameof(ReadJson))]
	public IJsonObject ReadJson() => JsonParser.Parse(CaseReader.ReadToEnd());
}
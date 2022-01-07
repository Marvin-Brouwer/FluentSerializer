using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.BenchmarkUtils.Profiles;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Xml.Benchmark.Data;

namespace FluentSerializer.Xml.Benchmark.Profiles;

public class XmlWriteProfile : WriteProfile
{
	public IEnumerable<TestCase<IDataNode>> Values() => XmlDataCollection.Default.ObjectTestData;

	[ParamsSource(nameof(Values))]
	public TestCase<IDataNode> Value { get; set; }

	[Benchmark, BenchmarkCategory(nameof(Write))]
	public string WriteXml() => Write(Value);
}
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[MemoryDiagnoser]
public class CamelCaseNamingStrategyProfile
{
	[Benchmark(Baseline = true), BenchmarkCategory("ShortNamed")]
	public (string className, string propertyName) ShortNamed()
	{
		var strategy = new CamelCaseNamingStrategy();

		return (
			strategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.ShortNamedClass.NamingContext),
			strategy.GetName(NameTestData.ShortNamedClass.PropertyInfo, NameTestData.ShortNamedClass.ClassType, NameTestData.ShortNamedClass.NamingContext)
		);
	}

	[Benchmark, BenchmarkCategory("ShortNamed")]
	public (string className, string propertyName) ShortNamedNew()
	{
		var strategy = new NewCamelCaseNamingStrategy();

		return (
			strategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.ShortNamedClass.NamingContext),
			strategy.GetName(NameTestData.ShortNamedClass.PropertyInfo, NameTestData.ShortNamedClass.ClassType, NameTestData.ShortNamedClass.NamingContext)
		);
	}
}
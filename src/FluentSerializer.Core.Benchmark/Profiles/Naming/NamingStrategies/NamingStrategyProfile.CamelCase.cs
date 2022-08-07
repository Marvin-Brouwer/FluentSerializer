using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using FluentSerializer.Core.Naming.NamingStrategies;
using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[MemoryDiagnoser]
public class NamingStrategyProfile_CamelCase
{
	private static readonly CamelCaseNamingStrategy _baseLineStrategy = new ();
	private static readonly NewCamelCaseNamingStrategy _optimizedStrategy = new ();

	[Benchmark(Baseline = true), BenchmarkCategory("CamelCase_ShortNamedClass")]
	public string CamelCase_ShortNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("CamelCase_ShortNamedClass")]
	public ReadOnlySpan<char> CamelCase_ShortNamedClassOptimized() =>
		_optimizedStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark(Baseline = true), BenchmarkCategory("CamelCase_ShortNamedProperty")]
	public string CamelCase_ShortNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("CamelCase_ShortNamedProperty")]
	public ReadOnlySpan<char> CamelCase_ShortNamedPropertyOptimized() =>
		_optimizedStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);


	[Benchmark(Baseline = true), BenchmarkCategory("CamelCase_LongNamedClass")]
	public string CamelCase_LongNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("CamelCase_LongNamedClass")]
	public ReadOnlySpan<char> CamelCase_LongNamedClassOptimized() =>
		_optimizedStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark(Baseline = true), BenchmarkCategory("CamelCase_LongNamedProperty")]
	public string CamelCase_LongNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("CamelCase_LongNamedProperty")]
	public ReadOnlySpan<char> CamelCase_LongNamedPropertyOptimized() =>
		_optimizedStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
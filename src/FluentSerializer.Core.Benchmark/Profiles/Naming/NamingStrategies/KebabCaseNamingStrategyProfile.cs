using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using FluentSerializer.Core.Naming.NamingStrategies;
using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[MemoryDiagnoser]
public class KebabCaseNamingStrategyProfile
{
	private static readonly KebabCaseNamingStrategy _baseLineStrategy = new ();
	private static readonly NewKebabCaseNamingStrategy _optimizedStrategy = new ();

	[Benchmark(Baseline = true), BenchmarkCategory("KebabCase_ShortNamedClass")]
	public string KebabCase_ShortNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("KebabCase_ShortNamedClass")]
	public ReadOnlySpan<char> KebabCase_ShortNamedClassOptimized() =>
		_optimizedStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark(Baseline = true), BenchmarkCategory("KebabCase_ShortNamedProperty")]
	public string KebabCase_ShortNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("KebabCase_ShortNamedProperty")]
	public ReadOnlySpan<char> KebabCase_ShortNamedPropertyOptimized() =>
		_optimizedStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);


	[Benchmark(Baseline = true), BenchmarkCategory("KebabCase_LongNamedClass")]
	public string KebabCase_LongNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("KebabCase_LongNamedClass")]
	public ReadOnlySpan<char> KebabCase_LongNamedClassOptimized() =>
		_optimizedStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark(Baseline = true), BenchmarkCategory("KebabCase_LongNamedProperty")]
	public string KebabCase_LongNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("KebabCase_LongNamedProperty")]
	public ReadOnlySpan<char> KebabCase_LongNamedPropertyOptimized() =>
		_optimizedStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
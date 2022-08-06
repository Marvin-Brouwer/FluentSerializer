using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using FluentSerializer.Core.Naming.NamingStrategies;
using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[MemoryDiagnoser]
public class SnakeCaseNamingStrategyProfile
{
	private static readonly SnakeCaseNamingStrategy _baseLineStrategy = new ();
	private static readonly NewSnakeCaseNamingStrategy _optimizedStrategy = new ();

	[Benchmark(Baseline = true), BenchmarkCategory("SnakeCase_ShortNamedClass")]
	public string SnakeCase_ShortNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("SnakeCase_ShortNamedClass")]
	public ReadOnlySpan<char> SnakeCase_ShortNamedClassOptimized() =>
		_optimizedStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark(Baseline = true), BenchmarkCategory("SnakeCase_ShortNamedProperty")]
	public string SnakeCase_ShortNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("SnakeCase_ShortNamedProperty")]
	public ReadOnlySpan<char> SnakeCase_ShortNamedPropertyOptimized() =>
		_optimizedStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);


	[Benchmark(Baseline = true), BenchmarkCategory("SnakeCase_LongNamedClass")]
	public string SnakeCase_LongNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("SnakeCase_LongNamedClass")]
	public ReadOnlySpan<char> SnakeCase_LongNamedClassOptimized() =>
		_optimizedStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark(Baseline = true), BenchmarkCategory("SnakeCase_LongNamedProperty")]
	public string SnakeCase_LongNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("SnakeCase_LongNamedProperty")]
	public ReadOnlySpan<char> SnakeCase_LongNamedPropertyOptimized() =>
		_optimizedStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
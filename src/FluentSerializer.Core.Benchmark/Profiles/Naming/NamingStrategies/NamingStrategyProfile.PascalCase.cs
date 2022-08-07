using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using FluentSerializer.Core.Naming.NamingStrategies;
using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[MemoryDiagnoser]
public class NamingStrategyProfile_PascalCase
{
	private static readonly PascalCaseNamingStrategy _baseLineStrategy = new ();
	private static readonly NewPascalCaseNamingStrategy _optimizedStrategy = new ();

	[Benchmark(Baseline = true), BenchmarkCategory("PascalCase_ShortNamedClass")]
	public string PascalCase_ShortNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("PascalCase_ShortNamedClass")]
	public ReadOnlySpan<char> PascalCase_ShortNamedClassOptimized() =>
		_optimizedStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark(Baseline = true), BenchmarkCategory("PascalCase_ShortNamedProperty")]
	public string PascalCase_ShortNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("PascalCase_ShortNamedProperty")]
	public ReadOnlySpan<char> PascalCase_ShortNamedPropertyOptimized() =>
		_optimizedStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);


	[Benchmark(Baseline = true), BenchmarkCategory("PascalCase_LongNamedClass")]
	public string PascalCase_LongNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("PascalCase_LongNamedClass")]
	public ReadOnlySpan<char> PascalCase_LongNamedClassOptimized() =>
		_optimizedStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark(Baseline = true), BenchmarkCategory("PascalCase_LongNamedProperty")]
	public string PascalCase_LongNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark, BenchmarkCategory("PascalCase_LongNamedProperty")]
	public ReadOnlySpan<char> PascalCase_LongNamedPropertyOptimized() =>
		_optimizedStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
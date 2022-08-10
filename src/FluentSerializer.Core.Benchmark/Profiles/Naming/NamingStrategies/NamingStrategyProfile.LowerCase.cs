using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.Naming.NamingStrategies;

using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[MemoryDiagnoser]
public class NamingStrategyProfile_LowerCase
{
	private static readonly LowerCaseNamingStrategy _baseLineStrategy = new ();

	[Benchmark]
	public ReadOnlySpan<char> LowerCase_ShortNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> LowerCase_ShortNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> LowerCase_LongNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> LowerCase_LongNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
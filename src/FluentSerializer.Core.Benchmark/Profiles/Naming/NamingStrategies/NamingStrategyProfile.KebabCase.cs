using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.Naming.NamingStrategies;

using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[MemoryDiagnoser]
public class NamingStrategyProfile_KebabCase
{
	private static readonly KebabCaseNamingStrategy NamingStrategy = new ();

	[Benchmark]
	public ReadOnlySpan<char> KebabCase_ShortNamedClass() =>
		NamingStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> KebabCase_ShortNamedProperty() =>
		NamingStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> KebabCase_LongNamedClass() =>
		NamingStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> KebabCase_LongNamedProperty() =>
		NamingStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
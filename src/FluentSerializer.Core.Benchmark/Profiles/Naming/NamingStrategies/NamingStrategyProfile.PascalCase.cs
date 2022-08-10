using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.Naming.NamingStrategies;
using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[MemoryDiagnoser]
public class NamingStrategyProfile_PascalCase
{
	private static readonly PascalCaseNamingStrategy _namingStrategy = new ();

	[Benchmark]
	public ReadOnlySpan<char> PascalCase_ShortNamedClass() =>
		_namingStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> PascalCase_ShortNamedProperty() =>
		_namingStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> PascalCase_LongNamedClass() =>
		_namingStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> PascalCase_LongNamedProperty() =>
		_namingStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
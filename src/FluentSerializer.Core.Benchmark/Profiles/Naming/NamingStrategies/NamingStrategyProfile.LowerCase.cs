using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.Naming.NamingStrategies;

using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

[MemoryDiagnoser]
public class NamingStrategyProfile_LowerCase
{

#pragma warning disable CS0649
#pragma warning disable S3459 // Unassigned members should be removed
	private static readonly LowerCaseNamingStrategy NamingStrategy;
#pragma warning restore S3459 // Unassigned members should be removed
#pragma warning restore CS0649

	[Benchmark]
	public ReadOnlySpan<char> LowerCase_ShortNamedClass() =>
		NamingStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> LowerCase_ShortNamedProperty() =>
		NamingStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> LowerCase_LongNamedClass() =>
		NamingStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> LowerCase_LongNamedProperty() =>
		NamingStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
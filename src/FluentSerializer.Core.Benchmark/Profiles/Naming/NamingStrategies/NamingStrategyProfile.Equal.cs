using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.Naming.NamingStrategies;

using System;

namespace FluentSerializer.Core.Benchmark.Profiles.Naming.NamingStrategies;

/// <summary>
/// This benchmark is pretty much redundant, we can't possibly optimize just assigning a varialble.
/// However, for completion sake, this is added.
/// </summary>
[MemoryDiagnoser]
public class NamingStrategyProfile_Equal
{
	private static readonly CustomNamingStrategy _baseLineStrategy = new ("Override");

	[Benchmark]
	public ReadOnlySpan<char> Equal_ShortNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> Equal_ShortNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.ShortNamedClass.PropertyInfo,
			NameTestData.ShortNamedClass.ClassType, NameTestData.NamingContext);


	[Benchmark]
	public ReadOnlySpan<char> Equal_LongNamedClass() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);

	[Benchmark]
	public ReadOnlySpan<char> Equal_LongNamedProperty() =>
		_baseLineStrategy.GetName(NameTestData.LongNamedWrapperClass.LongNamedInnerClass.PropertyInfo,
			NameTestData.LongNamedWrapperClass.LongNamedInnerClass.ClassType, NameTestData.NamingContext);
}
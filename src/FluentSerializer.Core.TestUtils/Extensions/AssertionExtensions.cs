#if NET8_0_OR_GREATER
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;

using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.TestUtils.Assertions;

using System;

namespace FluentSerializer.Core.TestUtils.Extensions;

public static class AssertionExtensions
{
	/// <summary>
	/// Custom implementation of <see cref="FluentAssertions.Primitives.StringAssertions"/>
	/// to make it easier to work with escape characters.
	/// </summary>
	[CustomAssertion]
	public static AndConstraint<StringAssertions> ShouldBeBinaryEquatableTo(this string assertions, string expectation)
	{
		return new StringAssertions(assertions, AssertionChain.GetOrCreate()).BeEquatableTo(expectation);
	}

	/// <summary>
	/// Custom implementation of <see cref="FluentAssertions.Primitives.StringAssertions"/>
	/// to make it easier to work with escape characters.
	/// </summary>
	[CustomAssertion]
	public static AndConstraint<GenericCollectionAssertions<T>> AllBeUnique<T>(this GenericCollectionAssertions<T> assertions)
		where T : struct
	{
		return new ValueTypeCollectionAssertions<T>(assertions.Subject, AssertionChain.GetOrCreate()).AllBeUnique();
	}

	/// <summary>
	/// Assert objects are equal using a specific implementation of <see cref="IEquatable{T}"/>.
	/// This is especially useful for records where you try to override the equality compare,
	/// by default FluentAssertions will not understand that.
	/// </summary>
	[CustomAssertion]
	public static NodeAssertions Should<TDataNode>(this TDataNode assertions)
		where TDataNode : IDataNode
	{
		return new NodeAssertions(assertions, AssertionChain.GetOrCreate());
	}

	/// <inheritdoc cref="FluentAssertions.AssertionExtensions.Should{T}(System.IComparable{T})"/>
	[CustomAssertion]
	public static NodeAssertions Should(this IDataNode assertions)
	{
		return new NodeAssertions(assertions, AssertionChain.GetOrCreate());
	}
}
#endif
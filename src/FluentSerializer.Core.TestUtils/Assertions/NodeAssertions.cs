#if NET8_0_OR_GREATER
using Ardalis.GuardClauses;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;

using System.Text.Json;

namespace FluentSerializer.Core.TestUtils.Assertions;

/// <summary>
/// Assertions that are specifically for <see cref="IDataNode"/>s
/// </summary>
public sealed class NodeAssertions : ReferenceTypeAssertions<IDataNode, NodeAssertions>
{
	public NodeAssertions(IDataNode instance) : base(instance) { }

	protected override string Identifier => Subject?.ToString() ?? string.Empty;

	/// <summary>
	/// Compare the equality of two <see cref="IDataNode"/>s.
	/// </summary>
	/// <param name="format">Format the output for humans</param>
	/// <param name="escape">Escape ASCI characters when outputting the error</param>
	/// <returns></returns>
	public AndConstraint<NodeAssertions> BeEquatableTo(
		IDataNode expectation, bool format = false, bool escape = false, string because = "", params object[] becauseArgs)
	{
		Execute.Assertion
			.BecauseOf(because, becauseArgs)
			.Given(() => Subject.Equals(expectation))
			.ForCondition(result => result)
			.FailWith("Expected result to be {0}, but found {1}.",
				// We serialize the output here, because we had some issues with escape characters 
				_ => escape
					? JsonSerializer.Serialize(expectation.WriteTo(Helpers.TestStringBuilderPool.Default, format, true, 0))
					: expectation.WriteTo(Helpers.TestStringBuilderPool.Default, format, true, 0),
				_ => escape
					? JsonSerializer.Serialize(Subject.WriteTo(Helpers.TestStringBuilderPool.Default, format, true, 0))
					: Subject.WriteTo(Helpers.TestStringBuilderPool.Default, format, true, 0)
			);

		return new AndConstraint<NodeAssertions>(this);
	}

	/// <summary>
	/// This seems like a strange thing to assert.
	/// However, this helps understand why some tests allow for this result.
	/// </summary>
	public AndConstraint<NodeAssertions> HaveInvalidName()
	{
		Execute.Assertion
			.Given(() => !HasValidName(Subject.Name))
			.ForCondition(result => result)
			.FailWith($"Expected node '{Subject.Name}' to have an invalid name");

		return new AndConstraint<NodeAssertions>(this);
	}

	private static bool HasValidName(string name)
	{
		try
		{
			Guard.Against.InvalidName(name);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
#endif
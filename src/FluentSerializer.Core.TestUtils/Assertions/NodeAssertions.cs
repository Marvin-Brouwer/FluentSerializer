using Ardalis.GuardClauses;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.TestUtils.Assertions;

/// <summary>
/// Assertions that are specifically for <see cref="IDataNode"/>s
/// </summary>
public class NodeAssertions : ReferenceTypeAssertions<IDataNode, NodeAssertions>
{
	public NodeAssertions(IDataNode instance) : base(instance) { }

	protected override string Identifier => Subject?.ToString() ?? string.Empty;

	public AndConstraint<NodeAssertions> BeEquatableTo(
		IDataNode expectation, bool format = false, string because = "", params object[] becauseArgs)
	{
		Execute.Assertion
			.BecauseOf(because, becauseArgs)
			.Given(() => Subject.Equals(expectation))
			.ForCondition(result => result)
			.FailWith("Expected result to be {0}, but found {1}.",
				_ => expectation.WriteTo(Helpers.TestStringBuilderPool.Default, format, true, 0),
				_ => Subject.WriteTo(Helpers.TestStringBuilderPool.Default, format, true, 0));

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
			Guard.Against.InvalidName(name, nameof(IDataNode.Name));
			return true;
		}
		catch
		{
			return false;
		}
	}
}
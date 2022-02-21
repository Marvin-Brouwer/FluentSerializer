using FluentAssertions;
using FluentSerializer.Core.Extensions;
using System;
using System.Linq.Expressions;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Extensions;

public sealed class NamingExtensionTests
{
	[Fact,
	 Trait("Category", "UnitTest")]
	public void GetProperty_PropertyExpression_ReturnsPropertyInfo()
	{
		// Arrange
		Expression<Func<TestClass, int>> input = (testClass) => testClass.Id;

		// Act
		var result = input.GetProperty();

		// Assert
		result.Name.Should().Be(nameof(TestClass.Id));
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void GetProperty_FieldExpression_Throws()
	{
		// Arrange
		Expression<Func<TestClass, int>> input = (testClass) => testClass.Field;

		// Act
		var result = () => input.GetProperty();

		// Assert
		result.Should().ThrowExactly<InvalidCastException>();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void GetProperty_MethodExpression_Throws()
	{
		// Arrange
		Expression<Func<TestClass, int>> input = (testClass) => testClass.GetId();

		// Act
		var result = () => input.GetProperty();

		// Assert
		result.Should().ThrowExactly<InvalidCastException>();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void GetProperty_AssigningExpression_Throws()
	{
		// Arrange
		Expression<Func<TestClass, int>> input = (testClass) => testClass.Id + 0;

		// Act
		var result = () => input.GetProperty();

		// Assert
		result.Should().ThrowExactly<InvalidCastException>();
	}

	private sealed class TestClass {
		public int Field = default!;
		public int Id { get; init; } = default!;
		public int GetId() => Id;
	}
}
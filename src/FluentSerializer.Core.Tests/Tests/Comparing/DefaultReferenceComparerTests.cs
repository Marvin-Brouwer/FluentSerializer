using FluentAssertions;

using FluentSerializer.Core.Comparing;

using Xunit;


namespace FluentSerializer.Core.Tests.Tests.Comparing;

public sealed class DefaultReferenceComparerTests
{
	private static readonly DefaultReferenceComparer Sut = DefaultReferenceComparer.Default;

	[Fact]
	public void Equals_BothAreNull_ReturnsTrue()
	{
		// Arrange
		int? a = null;
		string? b = null;

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_OneIsNull_ReturnsFalse()
	{
		// Arrange
		string? a = null;
		var b = "b";
		string? c = null;

		// Act
		var resultLeft = Sut.Equals(a, b);
		var resultRight = Sut.Equals(b, c);

		// Assert
		resultLeft.Should().BeFalse();
		resultRight.Should().BeFalse();
	}

	[Fact]
	public void Equals_ReferenceEqual_ReturnsTrue()
	{
		// Arrange
		var a = "Same";
		var b = a;

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_Overloaded_AreNotEqual_ReturnsFalse()
	{
		// Arrange
		var a = new EqualsOverloaded(false, 0);
		var b = new EqualsOverloaded(false, 1);

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Equals_Overloaded_AreEqual_ReturnsTrue()
	{
		// Arrange
		var a = new EqualsOverloaded(true, 0);
		var b = new EqualsOverloaded(true, 1);

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_ValueTypes_AreEqual_ReturnsTrue()
	{
		// Arrange
		var a = new CompareValue("Same");
		var b = new CompareValue("Same");

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_ClassTypes_AreEqual_ReturnsTrue()
	{
		// Arrange
		var a = new CompareInstance("Same");
		var b = new CompareInstance("Same");

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_Comparable_AreEqual_ReturnsTrue()
	{
		// Arrange
		var a = "Same";
		var b = "Same";

		// Act
		var result = Sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Equals_AreNotEqual_ReturnsFalse()
	{
		// Arrange
		var a = "A";
		var b = "B";
		var c = "C";

		// Act
		var resultName = Sut.Equals(a, b);
		var resultValue = Sut.Equals(b, c);

		// Assert
		resultName.Should().BeFalse();
		resultValue.Should().BeFalse();
	}

	private readonly record struct CompareValue(string Value);
	private sealed record CompareInstance(string Value);
	private sealed class EqualsOverloaded
	{
		private readonly bool _equalsReturns;
		private readonly int _hashCode;

		public EqualsOverloaded(bool equalsReturns, int hashCode)
		{
			_equalsReturns = equalsReturns;
			_hashCode = hashCode;
		}

		public override bool Equals(object? obj) => _equalsReturns;

		public override int GetHashCode() => _hashCode;
	}
}
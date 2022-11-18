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
	public void Equals_AreEqual_ReturnsTrue()
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
}
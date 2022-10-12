using FluentAssertions;

using FluentSerializer.Core.Comparing;

using Xunit;


namespace FluentSerializer.Core.Tests.Tests.Comparing;

public sealed partial class DataNodeComparerTests
{

	[Fact]
	public void CompareObject_BothAreNull_ReturnsTrue()
	{
		// Arrange
		TestDataNode? a = null;
		TestDataNode? b = null;

		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void CompareObject_OneIsNull_ReturnsFalse()
	{
		// Arrange
		TestDataNode? a = null;
		TestDataNode? b = null;
		var c = new TestDataNode("WithValue");

		var sut = DataNodeComparer.Default;

		// Act
		var resultLeft = sut.Equals(a, c);
		var resultRight = sut.Equals(c, b);

		// Assert
		resultLeft.Should().BeFalse();
		resultRight.Should().BeFalse();
	}

	[Fact]
	public void CompareObject_AreEqual_ReturnsTrue()
	{
		// Arrange
		var a = new TestDataNode("Same");
		var b = new TestDataNode("Same");

		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.Equals(a, b);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void CompareObject_AreNotEqual_ReturnsFalse()
	{
		// Arrange
		var a = new TestDataNode("A");
		var b = new TestDataNode("B");
		var b2 = new TestDataNode("B")
		{
			Value = "B2"
		};

		var sut = DataNodeComparer.Default;

		// Act
		var resultName = sut.Equals(a, b);
		var resultValue = sut.Equals(b, b2);

		// Assert
		resultName.Should().BeFalse();
		resultValue.Should().BeFalse();
	}
}
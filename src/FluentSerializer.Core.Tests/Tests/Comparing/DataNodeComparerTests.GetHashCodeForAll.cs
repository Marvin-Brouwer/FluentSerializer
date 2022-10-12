using FluentAssertions;

using FluentSerializer.Core.Comparing;

using Xunit;


namespace FluentSerializer.Core.Tests.Tests.Comparing;

public sealed partial class DataNodeComparerTests
{
	[Fact]
	public void GetHashCodeForAll_OneValue_ReturnsHashCode()
	{
		// Arrange
		var node = new TestDataNode("One");

		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.GetHashCodeForAll(node);

		// Assert
		result.Should().Be(sut.GetHashCodeForAll(node));
	}

	[Fact]
	public void GetHashCodeForAll_TwoValues_ReturnsHashCode()
	{
		// Arrange
		var node1 = new TestDataNode("One");
		var node2 = new TestDataNode("Two");

		var sut = DataNodeComparer.Default;

		// Act
		var result1 = sut.GetHashCodeForAll(node1);
		var result2 = sut.GetHashCodeForAll(node2);

		// Assert
		result1.Should().NotBe(result2);
		result2.Should().NotBe(result1);
	}

	[Fact]
	public void GetHashCodeForAll_ThreeValues_ReturnsHashCode()
	{
		// Arrange
		var node1 = new TestDataNode("One");
		var node2 = new TestDataNode("Two");
		var node3 = new TestDataNode("Tree");

		var sut = DataNodeComparer.Default;

		// Act
		var result1 = sut.GetHashCodeForAll(node1);
		var result2 = sut.GetHashCodeForAll(node2);
		var result3 = sut.GetHashCodeForAll(node3);

		// Assert
		result1.Should().NotBe(result2);
		result1.Should().NotBe(result3);
		result2.Should().NotBe(result1);
		result2.Should().NotBe(result3);
		result3.Should().NotBe(result1);
		result3.Should().NotBe(result2);
	}

	[Fact]
	public void GetHashCodeForAll_ManyValues_ReturnsHashCode()
	{
		// Arrange
		var node1 = new TestDataNode("One");
		var node2 = new TestDataNode("Two");
		var node3 = new TestDataNode("Tree");
		var node4 = new TestDataNode("Four and more");

		var sut = DataNodeComparer.Default;

		// Act
		var result1 = sut.GetHashCodeForAll(node1);
		var result2 = sut.GetHashCodeForAll(node2);
		var result3 = sut.GetHashCodeForAll(node3);
		var result4 = sut.GetHashCodeForAll(node4);

		// Assert
		result1.Should().NotBe(result2);
		result1.Should().NotBe(result3);
		result1.Should().NotBe(result4);
		result2.Should().NotBe(result1);
		result2.Should().NotBe(result3);
		result2.Should().NotBe(result4);
		result3.Should().NotBe(result1);
		result3.Should().NotBe(result2);
		result3.Should().NotBe(result4);
		result4.Should().NotBe(result1);
		result4.Should().NotBe(result2);
		result4.Should().NotBe(result3);
	}
}
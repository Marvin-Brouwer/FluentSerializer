using FluentAssertions;

using FluentSerializer.Core.Comparing;
using FluentSerializer.Core.DataNodes;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;


namespace FluentSerializer.Core.Tests.Tests.Comparing;

public sealed partial class DataNodeComparerTests
{
	/// <summary>
	/// The hashcode for any <c>null</c> value
	/// </summary>
	private const int NullValueHashCode = 0;

	/// <summary>
	/// The hashcode for collections without values
	/// </summary>
	private static readonly int NoItemsHashCode = new HashCode().ToHashCode();

	[Fact]
	public void GetHashCodeForAll_Null_ReturnsZero()
	{
		// Arrange
		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.GetHashCodeForAll<IDataNode?>(null);

		// Assert
		result.Should().Be(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_Primitive_ReturnsHashCode()
	{
		// Arrange
		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.GetHashCodeForAll(6);

		// Assert
		result.Should().NotBe(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_String_ReturnsHashCode()
	{
		// Arrange
		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.GetHashCodeForAll("SomeString");

		// Assert
		result.Should().NotBe(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_IDataNodeWithNullValue_ReturnsZero()
	{
		// Arrange
		var data = new Mock<IDataValue>(MockBehavior.Loose);

		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.GetHashCodeForAll(data.Object);

		// Assert
		result.Should().Be(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_IDataNodeWithValue_ReturnsHashCode()
	{
		// Arrange
		var data = new Mock<IDataValue>(MockBehavior.Loose);
		data
			.SetupGet(node => node.Value)
			.Returns("SomeValue");

		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.GetHashCodeForAll(data.Object);

		// Assert
		result.Should().NotBe(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_EmptyCollection_ReturnsHashCode()
	{
		// Arrange
		var data = Enumerable.Empty<IDataNode>();

		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.GetHashCodeForAll(data);

		// Assert
		result.Should().NotBe(NullValueHashCode);
		result.Should().Be(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_Collection_ReturnsHashCode()
	{
		// Arrange
		var data = new List<IDataNode>
		{
			new TestDataNode("Test")
		};

		var sut = DataNodeComparer.Default;

		// Act
		var result = sut.GetHashCodeForAll(data);

		// Assert
		result.Should().Be(sut.GetHashCodeForAll(data));
		result.Should().NotBe(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

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
		result.Should().NotBe(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
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
		result1.Should().NotBe(NullValueHashCode);
		result1.Should().NotBe(NoItemsHashCode);

		result2.Should().NotBe(result1);
		result2.Should().NotBe(NullValueHashCode);
		result2.Should().NotBe(NoItemsHashCode);
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
		result1.Should().NotBe(NullValueHashCode);
		result1.Should().NotBe(NoItemsHashCode);

		result2.Should().NotBe(result1);
		result2.Should().NotBe(result3);
		result2.Should().NotBe(NullValueHashCode);
		result2.Should().NotBe(NoItemsHashCode);

		result3.Should().NotBe(result1);
		result3.Should().NotBe(result2);
		result3.Should().NotBe(NullValueHashCode);
		result3.Should().NotBe(NoItemsHashCode);
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
		result1.Should().NotBe(NullValueHashCode);
		result1.Should().NotBe(NoItemsHashCode);

		result2.Should().NotBe(result1);
		result2.Should().NotBe(result3);
		result2.Should().NotBe(result4);
		result2.Should().NotBe(NullValueHashCode);
		result2.Should().NotBe(NoItemsHashCode);

		result3.Should().NotBe(result1);
		result3.Should().NotBe(result2);
		result3.Should().NotBe(result4);
		result3.Should().NotBe(NullValueHashCode);
		result3.Should().NotBe(NoItemsHashCode);

		result4.Should().NotBe(result1);
		result4.Should().NotBe(result2);
		result4.Should().NotBe(result3);
		result4.Should().NotBe(NullValueHashCode);
		result4.Should().NotBe(NoItemsHashCode);
	}
}
using FluentAssertions;

using FluentSerializer.Core.DataNodes;

using System;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.DataNodes;

public sealed partial class SingleItemCollectionTests
{
	[Fact]
	public void Empty_SingleItem_ShouldBeNull()
	{
		// Arrange
		var sut = SingleItemCollection.Empty<TestClass>();

		// Act
		var result = sut.SingleItem;

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public void Empty_FirstItem_ThrowsOutOfRangeException()
	{
		// Arrange
		var sut = SingleItemCollection.Empty<TestClass>();

		// Act
		var result = () => sut[0];

		// Assert
		result.Should().ThrowExactly<IndexOutOfRangeException>();
	}

	[Fact]
	public void Empty_SecondItem_ThrowsOutOfRangeException()
	{
		// Arrange
		var sut = SingleItemCollection.Empty<TestClass>();

		// Act
		var result = () => sut[1];

		// Assert
		result.Should().ThrowExactly<IndexOutOfRangeException>();
	}

	[Fact]
	public void Foreach_ResultsInZeroIterations()
	{
		// Arrange
		var sut = SingleItemCollection.Empty<TestClass>();
		var iterationAmount = 0;

		foreach (var _ in sut)
		{
			iterationAmount++;
		}

		// Assert
		iterationAmount.Should().Be(0);
	}

	[Fact]
	public void Empty_Count_ShouldBeZero()
	{
		// Arrange
		var sut = SingleItemCollection.Empty<TestClass>();

		// Act
		var result = sut.Count;

		// Assert
		result.Should().Be(0);
	}
}
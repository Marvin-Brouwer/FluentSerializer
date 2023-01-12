using FluentAssertions;

using FluentSerializer.Core.DataNodes;

using System;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.DataNodes;

public sealed partial class SingleItemCollectionTests
{
	[Fact]
	public void ForItem_SingleItem_ShouldBeExpected()
	{
		// Arrange
		var expected = new TestClass();
		var sut = SingleItemCollection.ForItem(expected);

		// Act
		var result = sut.SingleItem;

		// Assert
		result.Should().Be(expected);
	}

	[Fact]
	public void ForItem_FirstItem_ShouldBeExpected()
	{
		// Arrange
		var expected = new TestClass();
		var sut = SingleItemCollection.ForItem(expected);

		// Act
		var result = sut[0];

		// Assert
		result.Should().Be(expected);
	}

	[Fact]
	public void ForItem_SecondItem_ThrowsOutOfRangeException()
	{
		// Arrange
		var expected = new TestClass();
		var sut = SingleItemCollection.ForItem(expected);

		// Act
		var result = () => sut[1];

		// Assert
		result.Should().ThrowExactly<IndexOutOfRangeException>();
	}

	[Fact]
	public void ForItem_Foreach_ResultsInOneIteration()
	{
		// Arrange
		var expected = new TestClass();
		var sut = SingleItemCollection.ForItem(expected);
		var iterationAmount = 0;

		foreach (var _ in sut)
		{
			iterationAmount++;
		}

		// Assert
		iterationAmount.Should().Be(1);
	}

	[Fact]
	public void ForItem_Count_ShouldBeOne()
	{
		// Arrange
		var expected = new TestClass();
		var sut = SingleItemCollection.ForItem(expected);

		// Act
		var result = sut.Count;

		// Assert
		result.Should().Be(1);
	}
}
using FluentAssertions;

using FluentSerializer.Core.Comparing;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Text;

using System;

using Xunit;

// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods

namespace FluentSerializer.Core.Tests.Tests.Comparing;

public sealed class DataNodeComparerTests
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

internal sealed class TestDataNode : IDataNode
{
	public string Name { get; }
	public string Value { get; init; }

	public TestDataNode(string value)
	{
		Name = value;
		Value = value;
	}

	public override int GetHashCode()
	{
		return DataNodeComparer.Default.GetHashCodeForAll(
			nameof(TestDataNode), Name, Value
		);
	}

	public bool Equals(IDataNode? other)
	{
		throw new NotSupportedException("The equals will be tested through the Comparer");
	}

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		throw new NotSupportedException("Out of test scope");
	}
}
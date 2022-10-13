using FluentAssertions;

using FluentSerializer.Core.Comparing;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.TestUtils.Extensions;

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
	public void GetHashCodeForAll_GeneratedValue_ReturnsHashCode()
	{
		// Arrange
		const int amountOfOverloads = 4;
		var nodes = Enumerable
			.Range(0, amountOfOverloads)
			.Select(item => new TestDataNode($"Item_{item}"))
			.ToArray();

		var sut = DataNodeComparer.Default;

		// Act
		var results = new[]
		{
			// Test all Generic versions
			sut.GetHashCodeForAll(nodes[0]),
			sut.GetHashCodeForAll(nodes[0], nodes[1]),
			sut.GetHashCodeForAll(nodes[0], nodes[1], nodes[2]),

			// Test the dynamic version
			sut.GetHashCodeForAll(nodes.ToArray<object?>())
		};

		// Assert
		results.Should().AllBeUnique();
		results.Should().AllSatisfy(result => result.Should().NotBe(NullValueHashCode));
		results.Should().AllSatisfy(result => result.Should().NotBe(NoItemsHashCode));

		// This test should fail if not all overloads are covered.
		// If anyone ever adds an (n)th overload for performance gain, we need a new result added.
		results.Should().HaveCount(amountOfOverloads);
		sut.GetType().GetMethods()
			.Where(method => method.Name.Equals(nameof(sut.GetHashCodeForAll), StringComparison.Ordinal))
			.Should()
			.HaveCount(amountOfOverloads);
	}
}
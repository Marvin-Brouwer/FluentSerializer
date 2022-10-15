using FluentAssertions;

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
		// Act
		var result = Sut.GetHashCodeForAll<IDataNode?>(null);

		// Assert
		result.Should().Be(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_Primitive_ReturnsHashCode()
	{
		// Act
		var result = Sut.GetHashCodeForAll(6);

		// Assert
		result.Should().NotBe(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_String_ReturnsHashCode()
	{
		// Act
		var result = Sut.GetHashCodeForAll("SomeString");

		// Assert
		result.Should().NotBe(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_IDataNodeWithNullValue_ReturnsZero()
	{
		// Arrange
		var data = new Mock<IDataValue>(MockBehavior.Loose);

		// Act
		var result = Sut.GetHashCodeForAll(data.Object);

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

		// Act
		var result = Sut.GetHashCodeForAll(data.Object);

		// Assert
		result.Should().NotBe(NullValueHashCode);
		result.Should().NotBe(NoItemsHashCode);
	}

	[Fact]
	public void GetHashCodeForAll_EmptyCollection_ReturnsHashCode()
	{
		// Arrange
		var data = Enumerable.Empty<IDataNode>();

		// Act
		var result = Sut.GetHashCodeForAll(data);

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

		// Act
		var result = Sut.GetHashCodeForAll(data);

		// Assert
		result.Should().Be(Sut.GetHashCodeForAll(data));
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

		// Act
		var results = new[]
		{
			// Test all Generic versions
			Sut.GetHashCodeForAll(nodes[0]),
			Sut.GetHashCodeForAll(nodes[0], nodes[1]),
			Sut.GetHashCodeForAll(nodes[0], nodes[1], nodes[2]),

			// Test the dynamic version
			Sut.GetHashCodeForAll(nodes.ToArray<object?>())
		};

		// Assert
		results.Should().AllBeUnique();
		results.Should().AllSatisfy(result => result.Should().NotBe(NullValueHashCode));
		results.Should().AllSatisfy(result => result.Should().NotBe(NoItemsHashCode));

		// This test should fail if not all overloads are covered.
		// If anyone ever adds an (n)th overload for performance gain, we need a new result added.
		results.Should().HaveCount(amountOfOverloads);
		Sut.GetType().GetMethods()
			.Where(method => method.Name.Equals(nameof(Sut.GetHashCodeForAll), StringComparison.Ordinal))
			.Should()
			.HaveCount(amountOfOverloads);
	}
}
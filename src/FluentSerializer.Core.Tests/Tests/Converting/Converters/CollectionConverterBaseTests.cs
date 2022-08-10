using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

public sealed class CollectionConverterBaseTests
{
	[Theory,
		Trait("Category", "UnitTest"),
		InlineData(typeof(IEnumerable)), InlineData(typeof(int[])),
		InlineData(typeof(List<int>)), InlineData(typeof(ArrayList))]
	public void CanConvert_IsEnumerable_ReturnsTrue(Type input)
	{
		// Arrange
		var sut = new TestConverter();

		// Act
		var result = sut.CanConvert(in input);

		// Assert
		result.Should().BeTrue();
	}

	[Theory,
		Trait("Category", "UnitTest"),
		InlineData(typeof(bool)), InlineData(typeof(int)), InlineData(typeof(string))]
	public void CanConvert_IsNotEnumerableOrIsString_ReturnsFalse(Type input)
	{
		// Arrange
		var sut = new TestConverter();

		// Act
		var result = sut.CanConvert(in input);

		// Assert
		result.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetEnumerableInstance_ReturnsExpectedListType()
	{
		// Arrange
		var input1 = typeof(IEnumerable);
		var input2 = typeof(int[]);
		var input3 = typeof(List<int>);
		var input4 = typeof(ArrayList);

		var sut = new TestConverter();

		// Act
		var result1 = sut.GetEnumerableInstance(in input1);
		var result2 = sut.GetEnumerableInstance(in input2);
		var result3 = sut.GetEnumerableInstance(in input3);
		var result4 = sut.GetEnumerableInstance(in input4);

		// Assert
		result1.Should().BeOfType<List<object>>();
		result2.Should().BeOfType<List<int>>();
		input2.IsArray.Should().BeTrue();
		result3.Should().BeOfType<List<int>>();
		result4.Should().BeOfType<ArrayList>();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void FinalizeEnumerableInstance_ReturnsExpectedCollectionType()
	{
		// Arrange
		var input = new List<int>();

		var type1 = typeof(IEnumerable);
		var type2 = typeof(int[]);
		var type3 = typeof(List<int>);
		var type4 = typeof(ArrayList);

		var sut = new TestConverter();

		// Act
		var result1 = sut.FinalizeEnumerableInstance(input, in type1);
		var result2 = sut.FinalizeEnumerableInstance(input, in type2);
		var result3 = sut.FinalizeEnumerableInstance(input, in type3);
		var result4 = sut.FinalizeEnumerableInstance(input, in type4);

		// Assert
		result1.Should().BeOfType<List<int>>();
		result2.Should().BeOfType<int[]>();
		result3.Should().BeOfType<List<int>>();
		result4.Should().BeOfType<ArrayList>();
	}

	/// <inheritdoc />
	[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "TestImplementation")]
	private class TestConverter : CollectionConverterBase
	{
		/// <inheritdoc cref="CollectionConverterBase.GetEnumerableInstance" />
		public new IList GetEnumerableInstance(in Type targetType)
		{
			return CollectionConverterBase.GetEnumerableInstance(in targetType);
		}


		/// <inheritdoc cref="CollectionConverterBase.FinalizeEnumerableInstance" />
		public new IList? FinalizeEnumerableInstance(in IList? collection, in Type targetType)
		{
			return CollectionConverterBase.FinalizeEnumerableInstance(in collection, in targetType);
		}
	}
}

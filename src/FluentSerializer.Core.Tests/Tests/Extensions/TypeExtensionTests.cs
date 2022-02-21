using FluentAssertions;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Extensions;

public sealed class TypeExtensionTests
{
	[Fact,
	 Trait("Category", "UnitTest")]
	public void EqualsTopLevel_Equals_ReturnsTrue()
	{
		// Arrange
		var inputAConcrete = typeof(int);
		var inputBConcrete = typeof(int);
		var inputAGeneric = typeof(IEnumerable<int>);
		var inputBGeneric = typeof(IList<int>);
		var inputBGeneric2 = typeof(IEnumerable<string>);

		// Act
		var resultConcrete = inputAConcrete.EqualsTopLevel(in inputBConcrete);
		var resultGeneric = inputAGeneric.EqualsTopLevel(in inputBGeneric);
		var resultGeneric2 = inputAGeneric.EqualsTopLevel(in inputBGeneric2);

		// Assert
		resultConcrete.Should().BeTrue();
		resultGeneric.Should().BeTrue();
		resultGeneric2.Should().BeTrue();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void EqualsTopLevel_NotEquals_ReturnsFalse()
	{
		// Arrange
		var inputAConcrete = typeof(int);
		var inputBConcrete = typeof(bool);
		var inputAGeneric = typeof(IEnumerable<int>);
		var inputBGeneric = typeof(IProgress<bool>);

		// Act
		var resultConcrete = inputAConcrete.EqualsTopLevel(in inputBConcrete);
		var resultGeneric = inputAGeneric.EqualsTopLevel(in inputBGeneric);

		// Assert
		resultConcrete.Should().BeFalse();
		resultGeneric.Should().BeFalse();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void Implements()
	{
		// Arrange
		var inputCheck = typeof(IEnumerable);
		var input1 = typeof(int[]);
		var input2 = typeof(bool);

		// Act
		var result1 = input1.Implements(inputCheck);
		var result2 = input2.Implements(inputCheck);

		// Assert
		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void GetEnumerableInstance()
	{
		// Arrange
		var input1 = typeof(IEnumerable);
		var input2 = typeof(int[]);
		var input3 = typeof(List<int>);
		var input4 = typeof(ArrayList);

		// Act
		var result1 = input1.GetEnumerableInstance();
		var result2 = input2.GetEnumerableInstance();
		var result3 = input3.GetEnumerableInstance();
		var result4 = input4.GetEnumerableInstance();

		// Assert
		result1.Should().BeOfType<List<object>>();
		result2.Should().BeOfType<List<int>>();
		input2.IsArray.Should().BeTrue();
		result2.ToArray().Should().BeOfType<int[]>();
		result3.Should().BeOfType<List<int>>();
		result4.Should().BeOfType<ArrayList>();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void IsEnumerable()
	{
		// Arrange
		var input1 = typeof(string[]);
		var input2 = typeof(bool);
		var input3 = typeof(string);

		// Act
		var result1 = input1.IsEnumerable();
		var result2 = input2.IsEnumerable();
		var result3 = input3.IsEnumerable();

		// Assert
		result1.Should().BeTrue();
		result2.Should().BeFalse();
		result3.Should().BeFalse();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void IsNullable()
	{
		// Arrange
		var input1 = typeof(bool?);
		var input2 = typeof(bool);

		// Act
		var result1 = input1.IsNullable();
		var result2 = input2.IsNullable();

		// Assert
		result1.Should().BeTrue();
		result2.Should().BeFalse();
	}
}
using FluentAssertions;

using FluentSerializer.Core.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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

	#region IsNullable ValueType

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Struct_ForType_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestStruct?);
		var nonNullableInput = typeof(TestStruct);

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Struct_ForProperty_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestStruct).GetProperty(nameof(TestStruct.TestPropertyNullable))!;
		var nonNullableInput = typeof(TestStruct).GetProperty(nameof(TestStruct.TestProperty))!;

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Struct_ForField_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestStruct).GetField("_testFieldNullable", BindingFlags.NonPublic | BindingFlags.Instance)!;
		var nonNullableInput = typeof(TestStruct).GetField("_testField", BindingFlags.NonPublic | BindingFlags.Instance)!;

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Struct_ForParameter_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestStruct).GetMethod(nameof(TestStruct.TestMethodNullable))!.GetParameters()[0];
		var nonNullableInput = typeof(TestStruct).GetMethod(nameof(TestStruct.TestMethod))!.GetParameters()[0];

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Struct_ForReturnParameter_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestStruct).GetMethod(nameof(TestStruct.TestMethodNullable))!.ReturnParameter;
		var nonNullableInput = typeof(TestStruct).GetMethod(nameof(TestStruct.TestMethod))!.ReturnParameter;

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	#endregion

	#region IsNullable ReferenceType

	/// <remarks>
	/// You cannot resolve the type of a nullable reference instance.
	/// That type will always be either <c>null</c> or the non-nullable version of the instance.
	/// </remarks>
	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Class_ForType_ReturnsNullable()
	{
		// Arrange
		var nonNullableInput = typeof(TestClass);

		// Act
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nonNullableResult.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Class_ForProperty_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestClass).GetProperty(nameof(TestClass.TestPropertyNullable))!;
		var nonNullableInput = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Class_ForField_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestClass).GetField("_testFieldNullable", BindingFlags.NonPublic | BindingFlags.Instance)!;
		var nonNullableInput = typeof(TestClass).GetField("_testField", BindingFlags.NonPublic | BindingFlags.Instance)!;

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Class_ForParameter_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestClass).GetMethod(nameof(TestClass.TestMethodNullable))!.GetParameters()[0];
		var nonNullableInput = typeof(TestClass).GetMethod(nameof(TestClass.TestMethod))!.GetParameters()[0];

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Class_ForReturnParameter_ReturnsNullable()
	{
		// Arrange
		var nullableInput = typeof(TestClass).GetMethod(nameof(TestClass.TestMethodNullable))!.ReturnParameter;
		var nonNullableInput = typeof(TestClass).GetMethod(nameof(TestClass.TestMethod))!.ReturnParameter;

		// Act
		var nullableResult = nullableInput.IsNullable();
		var nonNullableResult = nonNullableInput.IsNullable();

		// Assert
		nullableResult.Should().BeTrue();
		nonNullableResult.Should().BeFalse();
	}

	#endregion

	[Fact,
		Trait("Category", "UnitTest")]
	public void IsNullable_Null_Throws()
	{
		// Arrange
		var typeInput = (Type)null!;
		var propertyInput = (PropertyInfo)null!;
		var fieldInput = (FieldInfo)null!;
		var parameterInput = (ParameterInfo)null!;

		// Act
		var typeResult = () => typeInput.IsNullable();
		var propertyResult = () => propertyInput.IsNullable();
		var fieldResult = () => fieldInput.IsNullable();
		var parameterResult = () => parameterInput.IsNullable();

		// Assert
		typeResult.Should().ThrowExactly<ArgumentNullException>();
		propertyResult.Should().ThrowExactly<ArgumentNullException>();
		fieldResult.Should().ThrowExactly<ArgumentNullException>();
		parameterResult.Should().ThrowExactly<ArgumentNullException>();
	}

	private readonly struct TestStruct
	{
		private readonly int _testField = 1;
		private readonly int? _testFieldNullable = 0;

		public TestStruct() { }

		public int TestProperty => _testField + 0;
		public int? TestPropertyNullable => _testFieldNullable + 0;

		public static int TestMethod(in int testParameter) => testParameter;
		public static int? TestMethodNullable(in int? testParameter) => testParameter;
	}

	private sealed class TestClass
	{
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable S1144 // Unused private types or members should be removed
		private readonly TestClass _testField = new ();
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning restore IDE0052 // Remove unread private members
		private readonly TestClass? _testFieldNullable = new();

		public TestClass TestProperty => _testFieldNullable ?? new TestClass();
		public TestClass? TestPropertyNullable => _testFieldNullable ?? new TestClass();

		public static TestClass TestMethod(in TestClass testParameter) => testParameter;
		public static TestClass? TestMethodNullable(in TestClass? testParameter) => testParameter;
	}
}
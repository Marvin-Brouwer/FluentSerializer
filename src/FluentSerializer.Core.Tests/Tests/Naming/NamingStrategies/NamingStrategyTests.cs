using FluentAssertions;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Naming.NamingStrategies;

public sealed class NamingStrategyTests
{
	private static readonly Mock<INamingContext> _namingContextMock = new();

	[Theory,
		Trait("Category", "UnitTest"),
		MemberData(nameof(ValidNamingRequests))]
	public void ValidString_GetName_ConvertsName(
		in Type typeInput, in PropertyInfo propertyInput,
		in INamingStrategy sut, in string expectedClassName, in string expectedPropertyName)
	{
		// Act
		var typeResult = sut.GetName(in typeInput, _namingContextMock.Object);
		var propertyResult = sut.GetName(in propertyInput, propertyInput.PropertyType, _namingContextMock.Object);

		// Assert
		typeResult.Should().BeEquivalentTo(expectedClassName);
		propertyResult.Should().BeEquivalentTo(expectedPropertyName);
	}

	[Theory,
		Trait("Category", "UnitTest"),
		MemberData(nameof(ValidNamingRequestsNew))]
	public void ValidString_GetName_ConvertsName_New(
		in Type typeInput, in PropertyInfo propertyInput,
		in INewNamingStrategy sut, in string expectedClassName, in string expectedPropertyName)
	{
		// Act
		var typeResult = sut.GetName(in typeInput, _namingContextMock.Object);
		var propertyResult = sut.GetName(in propertyInput, propertyInput.PropertyType, _namingContextMock.Object);

		// Assert
		typeResult.ToString().Should().BeEquivalentTo(expectedClassName);
		propertyResult.ToString().Should().BeEquivalentTo(expectedPropertyName);
	}

	public static IEnumerable<object[]> ValidNamingRequests()
	{
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;

		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.CamelCase(), "classNameWithMultipleParts", "propertyNameWithMultipleParts"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.PascalCase(), "ClassNameWithMultipleParts", "PropertyNameWithMultipleParts"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.LowerCase(), "classnamewithmultipleparts", "propertynamewithmultipleparts"
		};
		// todo generic
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.KebabCase(), "class-name-with-multiple-parts", "property-name-with-multiple-parts"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.SnakeCase(), "class_name_with_multiple_parts", "property_name_with_multiple_parts"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Equal("Override")(), "Override", "Override"
		};
	}

	public static IEnumerable<object[]> ValidNamingRequestsNew()
	{
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;

		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.CamelCaseNew(), "classNameWithMultipleParts", "propertyNameWithMultipleParts"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.PascalCaseNew(), "ClassNameWithMultipleParts", "PropertyNameWithMultipleParts"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.KebabCaseNew(), "class-name-with-multiple-parts", "property-name-with-multiple-parts"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.SnakeCaseNew(), "class_name_with_multiple_parts", "property_name_with_multiple_parts"
		};

		typeInput = typeof(ClassNameWith_strangeName);
		propertyInput = typeInput.GetProperty(nameof(ClassNameWith_strangeName.PropertyNameWith_strangeName))!;

		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.CamelCaseNew(), "classNameWithStrangeName", "propertyNameWithStrangeName"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.PascalCaseNew(), "ClassNameWithStrangeName", "PropertyNameWithStrangeName"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.SnakeCaseNew(), "class_name_with_strange_name", "property_name_with_strange_name"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.KebabCaseNew(), "class-name-with-strange-name", "property-name-with-strange-name"
		};

		typeInput = typeof(ClassNameWithGeneric<>);
		propertyInput = typeInput.GetProperty(nameof(ClassNameWithGeneric<object>.Property))!;

		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.CamelCaseNew(), "classNameWithGeneric", "property"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.PascalCaseNew(), "ClassNameWithGeneric", "Property"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.SnakeCaseNew(), "class_name_with_generic", "property"
		};
		yield return new object[] {
			typeInput, propertyInput,
			Names.Use.KebabCaseNew(), "class-name-with-generic", "property"
		};
	}

	private sealed class ClassNameWithMultipleParts
	{
		public int PropertyNameWithMultipleParts { get; set; }
	}

	private sealed class ClassNameWith_strangeName
	{
		public int PropertyNameWith_strangeName { get; set; }
	}
	private sealed class ClassNameWithGeneric<T>
	{
		public int Property { get; set; }
	}
}
using FluentAssertions;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Naming.NamingStrategies;

public sealed class NamingStrategyTests
{
	private static readonly Mock<INamingContext> _namingContextMock = new();

	[Theory,
		Trait("Category", "UnitTest"),
	 MemberData(nameof(ValidNamingRequests))]
	public void ValidString_GetName_ConvertsName(in INamingStrategy sut, in string expectedClassName, in string expectedPropertyName)
	{
		// Arrange
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;

		// Act
		var typeResult = sut.GetName(in typeInput, _namingContextMock.Object);
		var propertyResult = sut.GetName(in propertyInput, propertyInput.PropertyType, _namingContextMock.Object);

		// Assert
		typeResult.Should().BeEquivalentTo(expectedClassName);
		propertyResult.Should().BeEquivalentTo(expectedPropertyName);
	}

	private static IEnumerable<object[]> ValidNamingRequests()
	{
		yield return new object[] {
			Names.Use.CamelCase(), "classNameWithMultipleParts", "propertyNameWithMultipleParts"
		};
		yield return new object[] {
			Names.Use.PascalCase(), "ClassNameWithMultipleParts", "PropertyNameWithMultipleParts"
		};
		yield return new object[] {
			Names.Use.LowerCase(), "classnamewithmultipleparts", "propertynamewithmultipleparts"
		};
		yield return new object[] {
			Names.Use.KebabCase(), "class-name-with-multiple-parts", "property-name-with-multiple-parts"
		};
		yield return new object[] {
			Names.Use.SnakeCase(), "class_name_with_multiple_parts", "property_name_with_multiple_parts"
		};

		yield return new object[] { Names.Are("Override")(), "Override", "Override" };
	}

	private sealed class ClassNameWithMultipleParts
	{
		public int PropertyNameWithMultipleParts { get; set; }
	}
}
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

public sealed class NamingStrategyTests_KebabCase : NamingStrategyTests
{
	protected override INamingStrategy Sut => Names.Use.KebabCase();
	protected override INewNamingStrategy SutNew => Names.Use.KebabCaseNew();

	public static IEnumerable<object[]> ValidNamingRequests()
	{
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;

		yield return new object[] {
			typeInput, propertyInput,
			"class-name-with-multiple-parts", "property-name-with-multiple-parts"
		};
	}

	public static IEnumerable<object[]> ValidNamingRequestsNew()
	{
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;

		yield return new object[] {
			typeInput, propertyInput,
			"class-name-with-multiple-parts", "property-name-with-multiple-parts"
		};

		typeInput = typeof(ClassNameWith_strangeName);
		propertyInput = typeInput.GetProperty(nameof(ClassNameWith_strangeName.PropertyNameWith_strangeName))!;

		yield return new object[] {
			typeInput, propertyInput,
			"class-name-with-strange-name", "property-name-with-strange-name"
		};

		typeInput = typeof(ClassNameWithGeneric<>);
		propertyInput = typeInput.GetProperty(nameof(ClassNameWithGeneric<object>.Property))!;

		yield return new object[] {
			typeInput, propertyInput,
			"class-name-with-generic", "property"
		};
	}

	[Theory,
		Trait("Category", "UnitTest"),
		MemberData(nameof(ValidNamingRequests))]
	public override void ValidString_GetName_ConvertsName(
		in Type typeInput, in PropertyInfo propertyInput,
		in string expectedClassName, in string expectedPropertyName)
	{
		base.ValidString_GetName_ConvertsName(
			in typeInput, in propertyInput, in expectedClassName, in expectedPropertyName);
	}

	[Theory,
		Trait("Category", "UnitTest"),
		MemberData(nameof(ValidNamingRequestsNew))]
	public override void ValidString_GetName_ConvertsName_New(
		in Type typeInput, in PropertyInfo propertyInput,
		in string expectedClassName, in string expectedPropertyName)
	{
		base.ValidString_GetName_ConvertsName_New(
			in typeInput, in propertyInput, in expectedClassName, in expectedPropertyName);
	}
}
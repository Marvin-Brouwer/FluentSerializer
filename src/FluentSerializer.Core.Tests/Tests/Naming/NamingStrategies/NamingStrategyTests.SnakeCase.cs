using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Naming.NamingStrategies;

public sealed class NamingStrategyTests_SnakeCase : NamingStrategyTests
{
	protected override INamingStrategy Sut => Names.Use.SnakeCase();

	public static IEnumerable<object[]> ValidNamingRequests()
	{
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;

		yield return new object[] {
			typeInput, propertyInput,
			"class_name_with_multiple_parts", "property_name_with_multiple_parts"
		};

		typeInput = typeof(ClassNameWith_strangeName);
		propertyInput = typeInput.GetProperty(nameof(ClassNameWith_strangeName.PropertyNameWith_strangeName))!;

		yield return new object[] {
			typeInput, propertyInput,
			"class_name_with_strange_name", "property_name_with_strange_name"
		};

		typeInput = typeof(ClassNameWithGeneric<>);
		propertyInput = typeInput.GetProperty(nameof(ClassNameWithGeneric<object>.Property))!;

		yield return new object[] {
			typeInput, propertyInput,
			"class_name_with_generic", "property"
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
}
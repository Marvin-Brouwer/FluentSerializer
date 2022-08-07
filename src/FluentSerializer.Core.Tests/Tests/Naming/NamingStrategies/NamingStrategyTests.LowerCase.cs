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

public sealed class NamingStrategyTests_LowerCase : NamingStrategyTests
{
	protected override INamingStrategy Sut => Names.Use.LowerCase();
	protected override INewNamingStrategy SutNew => throw new NotSupportedException();

	// TODO Generic option
	public static IEnumerable<object[]> ValidNamingRequests()
	{
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;

		yield return new object[] {
			typeInput, propertyInput,
			"classnamewithmultipleparts", "propertynamewithmultipleparts"
		};
	}

	public static IEnumerable<object[]> ValidNamingRequestsNew() => throw new NotSupportedException();

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
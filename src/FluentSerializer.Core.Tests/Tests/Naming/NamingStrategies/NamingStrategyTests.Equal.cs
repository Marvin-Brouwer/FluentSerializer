using FluentAssertions;

using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Naming.NamingStrategies;

public sealed class NamingStrategyTests_Equal : NamingStrategyTests
{
	protected override INamingStrategy Sut => Names.Equal("Override")();

	public static IEnumerable<object[]> ValidNamingRequests()
	{
		var typeInput = typeof(ClassNameWithMultipleParts);
		var propertyInput = typeInput.GetProperty(nameof(ClassNameWithMultipleParts.PropertyNameWithMultipleParts))!;

		yield return new object[] {
			typeInput, propertyInput,
			"Override", "Override"
		};
	}

	[Theory,
		MemberData(nameof(ValidNamingRequests)),
		Trait("Category", "UnitTest")]
	public override void ValidString_GetName_ConvertsName(
		in Type typeInput, in PropertyInfo propertyInput,
		in string expectedClassName, in string expectedPropertyName)
	{
		base.ValidString_GetName_ConvertsName(
			in typeInput, in propertyInput, in expectedClassName, in expectedPropertyName);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void InvalidString_GetName_Throws()
	{
		// Arrange
		const string name = "This name definitely <contains> illegal characters: &&&";

		// Act
		var result = () => Names.Equal(name);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName(nameof(name));
	}
}
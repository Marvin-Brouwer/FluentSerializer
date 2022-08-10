using Ardalis.GuardClauses;

using FluentAssertions;

using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Collections.Generic;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Extensions;

/// <summary>
/// Currently, fields and methods aren't supported. They may be at some point though.
/// </summary>
public sealed class ExpressionExtensionTests
{
	[Theory,
		Trait("Category", "UnitTest"),
		MemberData(nameof(ValidCharacters))]
	public void SafeGetName_NameValid_ReturnsName(string input)
	{
		// Arrange
		var classType = typeof(TestClass);
		var property = classType.GetProperty(nameof(TestClass.Id))!;
		var propertyType = property.PropertyType;

		var sut = new CustomNamingStrategy(in input);

		// Act
		var resultClassName = sut.SafeGetName(in classType, default!);
		var resultPropertyName = sut.SafeGetName(in property, in propertyType, default!);
		var resultGuard = () => Guard.Against.InvalidName(in input);

		// Assert
		resultClassName.Should().Be(input);
		resultPropertyName.Should().Be(input);
		resultGuard.Should().NotThrow();
	}

	[Theory,
		Trait("Category", "UnitTest"),
		MemberData(nameof(InvalidCharacters))]
	public void SafeGetName_NameInValid_Throws(string input)
	{
		// Arrange
		var classType = typeof(TestClass);
		var property = classType.GetProperty(nameof(TestClass.Id))!;
		var propertyType = property.PropertyType;

		var sut = new CustomNamingStrategy(in input);

		// Act
		var resultClassName = () => sut.SafeGetName(in classType, default!);
		var resultPropertyName = () => sut.SafeGetName(in property, in propertyType, default!);
		var resultGuard = () => Guard.Against.InvalidName(in input);

		// Assert
		resultClassName.Should().Throw<ArgumentException>()
			.WithMessage("Input * was not in required format *")
			.Which.ParamName.Should().BeEquivalentTo("resolvedName");
		resultPropertyName.Should().Throw<ArgumentException>()
			.WithMessage("Input * was not in required format *")
			.Which.ParamName.Should().BeEquivalentTo("resolvedName");
		resultGuard.Should().Throw<ArgumentException>()
			.WithMessage("Input * was not in required format *")
			.Which.ParamName.Should().BeEquivalentTo("input");
	}

	[Theory,
		Trait("Category", "UnitTest"),
	 InlineData(""), InlineData(" "), InlineData("\t"), InlineData("\r"), InlineData("\n")]
	public void SafeGetName_NameNullOrWitheSpace_Throws(string input)
	{
		// Arrange
		var classType = typeof(TestClass);
		var property = classType.GetProperty(nameof(TestClass.Id))!;
		var propertyType = property.PropertyType;

		var sut = new CustomNamingStrategy(in input);

		// Act
		var resultClassName = () => sut.SafeGetName(in classType, default!);
		var resultPropertyName = () => sut.SafeGetName(in property, in propertyType, default!);
		var resultGuard = () => Guard.Against.InvalidName(in input);

		// Assert
		resultClassName.Should().Throw<ArgumentException>()
			.WithMessage("Required input * was empty. *")
			.Which.ParamName.Should().BeEquivalentTo("resolvedName");
		resultPropertyName.Should().Throw<ArgumentException>()
			.WithMessage("Required input * was empty. *")
			.Which.ParamName.Should().BeEquivalentTo("resolvedName");
		resultGuard.Should().Throw<ArgumentException>()
			.WithMessage("Required input * was empty. *")
			.Which.ParamName.Should().BeEquivalentTo("input");
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void SafeGetName_NameNull_Throws()
	{
		// Arrange
		string? input = null;
		var classType = typeof(TestClass);
		var property = classType.GetProperty(nameof(TestClass.Id))!;
		var propertyType = property.PropertyType;

		var sut = new CustomNamingStrategy(in input!);

		// Act
		var resultClassName = () => sut.SafeGetName(in classType, default!);
		var resultPropertyName = () => sut.SafeGetName(in property, in propertyType, default!);
		var resultGuard = () => Guard.Against.InvalidName(in input);

		// Assert
		resultClassName.Should().Throw<ArgumentException>()
			.WithMessage("Required input resolvedName was empty. *")
			.Which.ParamName.Should().BeEquivalentTo("resolvedName");
		resultPropertyName.Should().Throw<ArgumentException>()
			.WithMessage("Required input resolvedName was empty. *")
			.Which.ParamName.Should().BeEquivalentTo("resolvedName");
		resultGuard.Should().Throw<ArgumentException>()
			.WithMessage("Value cannot be null. *")
			.Which.ParamName.Should().BeEquivalentTo("input");
	}

	public static IEnumerable<object[]> ValidCharacters()
	{
		for (char letter = 'A'; letter <= 'Z'; letter++)
		{
			yield return new[] { char.ToLowerInvariant(letter).ToString() };
			yield return new[] { letter.ToString() };
		}
		for (int number = 0; number <= 9; number++)
		{
			yield return new[] { number.ToString() };
		}
		const string validSpecialCharacters = "_+-";
		foreach (var character in validSpecialCharacters)
		{
			yield return new[] { character.ToString() };
		}
	}

	public static IEnumerable<object[]> InvalidCharacters()
	{
		const string invalidCharacters = "!@#$%^&*()=,.?/\\[]{}<>|";
		foreach (var character in invalidCharacters)
		{
			yield return new[] { character.ToString() };
		}
	}


	private sealed class TestClass
	{
		public int Id { get; init; } = default!;
	}
}
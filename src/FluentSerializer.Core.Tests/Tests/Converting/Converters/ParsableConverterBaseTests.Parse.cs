using FluentAssertions;

using System;
using System.IO;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.ToString(bool)"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed partial class ParsableConverterBaseTests
{
	[Theory,
		MemberData(nameof(GenerateParsableData)),
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_Parse_Parsable_ReturnsValue(object expected, string input)
	{
		// Act
		var canConvert = Sut.Parse.CanConvert(expected.GetType());
		var result = Sut.Parse.ConvertToNullableDataType(input, expected.GetType());

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		InlineData(typeof(string)), InlineData(typeof(int)), InlineData(typeof(bool)),
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_Parse_Parsable_EmptyValue_ReturnsNull(Type type)
	{
		// Act
		var result = Sut.Parse.ConvertToNullableDataType(string.Empty, type);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_Parse_Parsable_IncorrectFormat_Throws()
	{
		// Arrange
		var input = "SomeText";

		// Act
		var result = () => Sut.Parse.ConvertToNullableDataType(input, typeof(int));

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage($"The input string '{input}' was not in a correct format.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_Parse_NonParsable_Throws()
	{
		// Arrange
		var input = "Doesn't matter";

		// Act
		var canConvert = Sut.Parse.CanConvert(typeof(Stream));
		var result = () => Sut.Parse.ConvertToNullableDataType(input, typeof(Stream));

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<EntryPointNotFoundException>()
			.WithMessage("The method 'Parse' was not found on IParsable type");
	}
}


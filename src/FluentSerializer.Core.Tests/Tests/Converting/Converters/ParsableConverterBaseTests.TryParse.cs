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
	public void ConvertToNullableDataType_TryParse_Parsable_ReturnsValue(object expected, string input)
	{
		// Act
		var canConvert = Sut.TryParse.CanConvert(expected.GetType());
		var result = Sut.TryParse.ConvertToNullableDataType(input, expected.GetType());

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		InlineData(typeof(string)), InlineData(typeof(int)), InlineData(typeof(bool)),
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_TryParse_Parsable_EmptyValue_ReturnsNull(Type type)
	{
		// Act
		var result = Sut.TryParse.ConvertToNullableDataType(string.Empty, type);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_TryParse_Parsable_IncorrectFormat_ReturnsNull()
	{
		// Arrange
		const string input = "SomeText";

		// Act
		var result = Sut.TryParse.ConvertToNullableDataType(input, typeof(int));

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_TryParse_NonParsable_Throws()
	{
		// Arrange
		const string input = "Doesn't matter";

		// Act
		var canConvert = Sut.TryParse.CanConvert(typeof(Stream));
		var result = () => Sut.TryParse.ConvertToNullableDataType(input, typeof(Stream));

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<EntryPointNotFoundException>()
			.WithMessage("The method 'TryParse' was not found on IParsable type");
	}
}

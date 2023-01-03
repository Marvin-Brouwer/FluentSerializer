using FluentAssertions;

using FluentSerializer.Core.Tests.ObjectMother;

using System;
using System.Data;
using System.IO;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

using Sut = FluentSerializer.Json.Converting.Converters.ParsableConverter;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.Tostring"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed partial class ParsableConverterTests
{
	#region Serialize
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Serialize_Parse_ThrowsNotSupported()
	{
		// Arrange
		const int input = 0;

		// Act
		var canConvert = SutParse().CanConvert(input.GetType());
		var result = () => SutParse().Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should()
			.ThrowExactly<NotSupportedException>();
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Parse_EmptyValue_ReturnsDefault(object requested, string unused)
	{
		_ = unused;

		// Arrange
		var input = Value(string.Empty);
		var expected = (object?)null;

		_contextMock
			.WithPropertyType(requested.GetType());

		// Act
		var canConvert = SutParse().CanConvert(requested.GetType());
		var result = SutParse().Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Parse_Parsible_ReturnsValue(object expected, string inputValue)
	{
		// Arrange
		var input = Value(inputValue);

		_contextMock
			.WithPropertyType(expected.GetType());

		// Act
		var canConvert = SutParse(TestCulture).CanConvert(expected.GetType());
		var result = SutParse(TestCulture).Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	/// <summary>
	/// This is an interesting scenario.
	/// Since a string without quotes is considered invalid JSON the serializer should never get this far in the first place.
	/// Because of that the converter just plainly ignores the fact that it doesn't have quotes and snips of the outer characters.
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Parse_Parsible_UnquotedString_ReturnsTruncatedValue()
	{
		// Arrange
		var input = Value("This string has no quotes");
		const string expected = "his string has no quote";

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = SutParse().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should().Be(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Parse_Parsible_EmptyString_ReturnsEmptyString()
	{
		// Arrange
		var input = Value("\"\"");
		var expected = string.Empty;

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = SutParse().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should().Be(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Parse_Parsible_StringTooShort_Throws()
	{
		// Arrange
		var input = Value("S"); // <-- not surrounded with quotes

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => SutParse().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<DataException>();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Parse_Parsible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => SutParse().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage($"The input string '{input.Value}' was not in a correct format.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Parse_NonParsible_Throws()
	{
		// Arrange
		var input = Value("Doesn't matter");

		_contextMock
			.WithPropertyType(typeof(Stream));

		// Act
		var canConvert = SutParse().CanConvert(typeof(Stream));
		var result = () => SutParse().Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<EntryPointNotFoundException>()
			.WithMessage("The method 'Parse' was not found on IParsable type");
	}
	#endregion
}

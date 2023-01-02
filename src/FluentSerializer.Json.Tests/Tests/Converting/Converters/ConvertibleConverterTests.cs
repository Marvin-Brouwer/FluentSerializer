using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using Moq;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.Tostring"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed class ConvertibleConverterTests
{
	private readonly ConvertibleConverter _sut;
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public ConvertibleConverterTests()
	{
		_sut = new ConvertibleConverter();
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { 1, "1" };
		yield return new object[] { true, "True" };
		// JSON strings need to be wrapped in quotes
		yield return new object[] { "string", "\"string\"" };
	}

	#region Serialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(null, ""), InlineData("", "\"\"")]
	public void Serialize_NullOrEmpty_ReturnsEmptyString(string input, string expectedValue)
	{
		// Arrange
		var expected = Value(expectedValue);

		// Act
		var canConvert = _sut.CanConvert(typeof(string));
		var result = _sut.Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Serialize_NonConvertible_ReturnsToString()
	{
		// Arrange
		using var input = new MemoryStream(0);
		var expected = Value(input.ToString());

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = _sut.Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void SerializeAttributeConvertible_ReturnsString(object input, string expectedValue)
	{
		// Arrange
		var expected = Value(expectedValue);

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = _sut.Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_EmptyValue_ReturnsDefault(object requested, string unused)
	{
		_ = unused;

		// Arrange
		var input = Value(string.Empty);
		var expected = (object?)null;

		_contextMock
			.WithPropertyType(requested.GetType());

		// Act
		var canConvert = _sut.CanConvert(requested.GetType());
		var result = _sut.Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(object expected, string inputValue)
	{
		// Arrange
		var input = Value(inputValue);

		_contextMock
			.WithPropertyType(expected.GetType());

		// Act
		var canConvert = _sut.CanConvert(expected.GetType());
		var result = _sut.Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	/// <remarks>
	/// This is an interesting scenario.
	/// Since a string without quotes is considered invalid JSON the serializer should never get this far in the first place.
	/// Because of that the converter just plainly ignores the fact that it doesn't have quotes and snips of the outer characters.
	/// </remarks>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_UnquotedString_ReturnsTruncatedValue()
	{
		// Arrange
		var input = Value("This string has no quotes");
		var expected = "his string has no quote";

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = _sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should().Be(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_EmptyString_ReturnsEmptyString()
	{
		// Arrange
		var input = Value("\"\"");
		var expected = string.Empty;

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = _sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should().Be(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_StringTooShort_Throws()
	{
		// Arrange
		var input = Value("S"); // <-- not surrounded with quotes

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => _sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<DataException>();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => _sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage($"The input string '{input.Value}' was not in a correct format.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_NonConvertible_Throws()
	{
		// Arrange
		var input = Value("Doesn't matter");

		_contextMock
			.WithPropertyType(typeof(Stream));

		// Act
		var canConvert = _sut.CanConvert(typeof(Stream));
		var result = () => _sut.Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<InvalidCastException>()
			.WithMessage("Invalid cast from 'System.String' to 'System.IO.Stream'.");
	}
	#endregion
}


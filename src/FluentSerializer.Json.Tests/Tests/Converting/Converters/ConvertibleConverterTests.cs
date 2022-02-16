using FluentAssertions;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.TestUtils.ObjectMother;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using FluentSerializer.Json.Tests.ObjectMother;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.Tostring"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </remarks>
public sealed class ConvertibleConverterTests
{
	private readonly ConvertibleConverter _sut;
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;
	private readonly Mock<IAdvancedJsonSerializer> _serializerMock;

	public ConvertibleConverterTests()
	{
		_sut = new ConvertibleConverter();
		_serializerMock = new Mock<IAdvancedJsonSerializer>();
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(_serializerMock);
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
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
	public void Deserialize_Convertable_ReturnsValue(object expected, string inputValue)
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

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertable_IncorrectFormat_Throws()
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
			.WithMessage("Input string was not in a correct format.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_NonConvertable_Throws()
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

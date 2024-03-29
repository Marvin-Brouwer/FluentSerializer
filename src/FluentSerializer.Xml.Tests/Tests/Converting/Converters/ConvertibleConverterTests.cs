using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;

using Moq;

using System;
using System.Collections.Generic;
using System.IO;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.ToString()"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed class ConvertibleConverterTests
{
	private readonly ConvertibleConverter _sut;
	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public ConvertibleConverterTests()
	{
		_sut = new ConvertibleConverter();
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock);
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { 1, "1" };
		yield return new object[] { true, "True" };
		yield return new object[] { "string", "string" };
	}

	#region Serialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		InlineData(null), InlineData("")]
	public void Serialize_NullOrEmpty_ReturnsEmptyString(string input)
	{
		// Arrange
		var expected = Text(string.Empty);

		// Act
		var canConvert = _sut.CanConvert(typeof(string));
		var result = ((IXmlConverter<IXmlText>)_sut).Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_NonConvertible_ReturnsToString()
	{
		// Arrange
		using var input = new MemoryStream(0);
		var expected = Text(input.ToString());

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = ((IXmlConverter<IXmlText>)_sut).Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Serialize_Convertible_ReturnsString(object input, string expectedValue)
	{
		// Arrange
		var expected = Text(expectedValue);

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = ((IXmlConverter<IXmlText>)_sut).Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_EmptyValue_ReturnsDefault(object requested, string unused)
	{
		_ = unused;

		// Arrange
		var input = Text(string.Empty);
		var expected = (object?)null;

		_contextMock
			.WithPropertyType(requested.GetType());

		// Act
		var canConvert = _sut.CanConvert(requested.GetType());
		var result = ((IXmlConverter<IXmlText>)_sut).Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(object expected, string inputValue)
	{
		// Arrange
		var input = Text(inputValue);

		_contextMock
			.WithPropertyType(expected.GetType());

		// Act
		var canConvert = _sut.CanConvert(expected.GetType());
		var result = ((IXmlConverter<IXmlText>)_sut).Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Text("SomeText");

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => ((IXmlConverter<IXmlText>)_sut).Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage($"The input string '{input.Value}' was not in a correct format.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_NonConvertible_Throws()
	{
		// Arrange
		var input = Text("Doesn't matter");

		_contextMock
			.WithPropertyType(typeof(Stream));

		// Act
		var canConvert = _sut.CanConvert(typeof(Stream));
		var result = () => ((IXmlConverter<IXmlText>)_sut).Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<InvalidCastException>()
			.WithMessage("Invalid cast from 'System.String' to 'System.IO.Stream'.");
	}
	#endregion
}


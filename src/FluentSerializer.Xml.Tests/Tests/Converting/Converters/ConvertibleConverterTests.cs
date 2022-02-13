using FluentAssertions;
using FluentSerializer.Core.Context;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;
using FluentSerializer.Xml.Tests.ObjectMother;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.Tostring"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </remarks>
public sealed class ConvertibleConverterTests
{
	private readonly ConvertibleConverter _sut;
	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;
	private readonly Mock<IAdvancedXmlSerializer> _serializerMock;

	public ConvertibleConverterTests()
	{
		_sut = new ConvertibleConverter();
		_serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(_serializerMock);
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { 1, "1" };
		yield return new object[] { true, "True" };
		yield return new object[] { "string", "string" };
	}

	#region Serialize
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_NullOrEmpty_ReturnsEmptyString()
	{
		// Arrange
		var expected = Text(string.Empty);

		var inputEmpty = string.Empty;
		var inputNull = (int?)null;

		// Act
		var canConvertEmpty = _sut.CanConvert(inputEmpty.GetType());
		var resultEmpty = ((IXmlConverter<IXmlText>)_sut).Serialize(inputEmpty, _contextMock.Object);

		var canConvertNull = _sut.CanConvert(typeof(int?));
		var resultNull = ((IXmlConverter<IXmlText>)_sut).Serialize(inputNull!, _contextMock.Object);

		// Assert
		canConvertEmpty.Should().BeTrue();
		resultEmpty.Should().BeEquivalentTo(expected);

		// Evidently IConvertable is not assignable to `null` so this returns false
		canConvertNull.Should().BeFalse();
		resultNull.Should().BeEquivalentTo(expected);
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
	public void SerializeAttributeConvertible_ReturnsString(object input, string expectedValue)
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
	public void Deserialize_Convertable_ReturnsValue(object expected, string inputValue)
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
	public void Deserialize_Convertable_IncorrectFormat_Throws()
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
			.WithMessage("Input string was not in a correct format.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_NonConvertable_Throws()
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


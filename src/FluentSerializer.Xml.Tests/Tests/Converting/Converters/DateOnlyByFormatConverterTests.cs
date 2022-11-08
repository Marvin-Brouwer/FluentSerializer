using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;

using Moq;

using System;
using System.Collections.Generic;
using System.Globalization;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="DateOnly.ToString(string?)"/>
/// and <see cref="DateOnly.ParseExact(string, string, IFormatProvider?, DateTimeStyles)"/>
/// </summary>
public sealed class DateOnlyByFormatConverterTests
{
	private static readonly DateOnly DateOnlyValue = DateOnly.Parse("2096-04-20", CultureInfo.InvariantCulture);
	
	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public DateOnlyByFormatConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(DateOnlyValue)));
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "yyyy-MM-dd", "2096-04-20", CultureInfo.InvariantCulture };
		yield return new object[] { "M/d/yyyy", "4/20/2096", new CultureInfo("en-US") };
		yield return new object[] { "dd-MM-yyyy", "20-04-2096", new CultureInfo("nl-NL") };
	}

	#region Initialization

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Initialize_NullValues_Throws()
	{
		// Arrange
		var format = "g";
		var cultureInfo = CultureInfo.InvariantCulture;
		var dateTimeStyle = DateTimeStyles.AllowWhiteSpaces;

		// Act
		var result1 = () => new DateOnlyByFormatConverter(null!, cultureInfo, dateTimeStyle);
		var result2 = () => new DateOnlyByFormatConverter(format, null!, dateTimeStyle);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(format));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(cultureInfo));
	}

	#endregion

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void SerializePattern_ReturnsString(string pattern, string expectedValue, CultureInfo cultureInfo)
	{
		// Arrange
		var expectedText = Text(expectedValue);
		var expectedAttribute = Attribute(nameof(DateOnlyValue), expectedValue);
		var expectedElement = Element(nameof(DateOnlyValue), expectedText);

		var sut = new DateOnlyByFormatConverter(pattern, cultureInfo, DateTimeStyles.AllowWhiteSpaces);

		// Act
		var canConvert = sut.CanConvert(DateOnlyValue.GetType());
		var textResult = ((IConverter<IXmlText, IXmlNode>)sut).Serialize(DateOnlyValue, _contextMock.Object)!;
		var attributeResult = ((IConverter<IXmlAttribute, IXmlNode>)sut).Serialize(DateOnlyValue, _contextMock.Object)!;
		var elementResult = ((IConverter<IXmlElement, IXmlNode>)sut).Serialize(DateOnlyValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		textResult.Should().BeEquatableTo(expectedText);
		attributeResult.Should().BeEquatableTo(expectedAttribute);
		elementResult.Should().BeEquatableTo(expectedElement);
	}
	#endregion

	#region Deserialize

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(string pattern, string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		var textInput = Text(inputValue);
		var attributeInput = Attribute(nameof(DateOnlyValue), inputValue);
		var elementInput = Element(nameof(DateOnlyValue), textInput);

		var sut = new DateOnlyByFormatConverter(pattern, cultureInfo, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(DateOnlyValue.GetType());

		// Act
		var canConvert = sut.CanConvert(DateOnlyValue.GetType());
		var textResult = (DateOnly)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (DateOnly)((IConverter<IXmlAttribute, IXmlNode>)sut).Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (DateOnly)((IConverter<IXmlElement, IXmlNode>)sut).Deserialize(elementInput, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		textResult.Should().Be(DateOnlyValue);
		attributeResult.Should().Be(DateOnlyValue);
		elementResult.Should().Be(DateOnlyValue);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Text("SomeText");
		var sut = new DateOnlyByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateOnly?)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid DateOnly.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_EmptyString_Throws()
	{
		// Arrange
		var input = Text("");
		var sut = new DateOnlyByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => (DateOnly?)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String '' was not recognized as a valid DateOnly.");
	}

	#endregion
}


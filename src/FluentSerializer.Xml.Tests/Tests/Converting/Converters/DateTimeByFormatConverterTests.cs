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
/// Basically test if this converter behaves exactly like <see cref="DateTime.ToString(string?)"/>
/// and <see cref="DateTime.ParseExact(string, string, IFormatProvider?, DateTimeStyles)"/>
/// </summary>
public sealed class DateTimeByFormatConverterTests
{
	private static readonly DateTime DateTimeValue = DateTime.Parse(
		"2096-04-20T04:20:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public DateTimeByFormatConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(DateTimeValue)));
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "yyyy-MM-dd HH:mm:ss", "2096-04-20 04:20:00", CultureInfo.InvariantCulture };
		yield return new object[] { "d", "4/20/2096", new CultureInfo("en-US") };
		yield return new object[] { "g", "4/20/2096 4:20 AM", new CultureInfo("en-US") };
		yield return new object[] { "g", "20-04-2096 04:20", new CultureInfo("nl-NL") };
	}

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void SerializePattern_ReturnsString(string pattern, string expectedValue, CultureInfo cultureInfo)
	{
		// Arrange
		var expectedText = Text(expectedValue);
		var expectedAttribute = Attribute(nameof(DateTimeValue), expectedValue);
		var expectedElement = Element(nameof(DateTimeValue), expectedText);

		var sut = new DateTimeByFormatConverter(pattern, cultureInfo, DateTimeStyles.AssumeUniversal);

		// Act
		var canConvert = sut.CanConvert(DateTimeValue.GetType());
		var textResult = ((IConverter<IXmlText, IXmlNode>)sut).Serialize(DateTimeValue, _contextMock.Object)!;
		var attributeResult = ((IConverter<IXmlAttribute, IXmlNode>)sut).Serialize(DateTimeValue, _contextMock.Object)!;
		var elementResult = ((IConverter<IXmlElement, IXmlNode>)sut).Serialize(DateTimeValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		textResult.Should().BeEquatableTo(expectedText);
		attributeResult.Should().BeEquatableTo(expectedAttribute);
		elementResult.Should().BeEquatableTo(expectedElement);
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(string pattern, string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		var textInput = Text(inputValue);
		var attributeInput = Attribute(nameof(DateTimeValue), inputValue);
		var elementInput = Element(nameof(DateTimeValue), textInput);

		var sut = new DateTimeByFormatConverter(pattern, cultureInfo, DateTimeStyles.None);

		_contextMock
			.WithPropertyType(DateTimeValue.GetType());

		// Act
		var canConvert = sut.CanConvert(DateTimeValue.GetType());
		var textResult = (DateTime)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (DateTime)((IConverter<IXmlAttribute, IXmlNode>)sut).Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (DateTime)((IConverter<IXmlElement, IXmlNode>)sut).Deserialize(elementInput, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		if (pattern == "d")
		{
			textResult.Should().BeSameDateAs(DateTimeValue);
			attributeResult.Should().BeSameDateAs(DateTimeValue);
			elementResult.Should().BeSameDateAs(DateTimeValue);
		}
		else
		{
			textResult.Should().Be(DateTimeValue);
			attributeResult.Should().Be(DateTimeValue);
			elementResult.Should().Be(DateTimeValue);
		}
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Text("SomeText");
		var sut = new DateTimeByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.None);

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateTime?)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid DateTime.");
	}
	#endregion
}


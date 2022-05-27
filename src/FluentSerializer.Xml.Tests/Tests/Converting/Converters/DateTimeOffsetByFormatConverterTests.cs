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
/// Basically test if this converter behaves exactly like <see cref="DateTimeOffset.ToString(string?)"/>
/// and <see cref="DateTimeOffset.ParseExact(string, string, IFormatProvider?, DateTimeStyles)"/>
/// </summary>
public sealed class DateTimeOffsetByFormatConverterTests
{
	private static readonly DateTimeOffset DateTimeOffsetValue = DateTimeOffset.Parse(
		"2096-04-20T04:20:00+00:00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public DateTimeOffsetByFormatConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(DateTimeOffsetValue)));
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "yyyy-MM-dd HH:mm:ss zzz", "2096-04-20 04:20:00 +00:00", CultureInfo.InvariantCulture };
		yield return new object[] { "M/d/yyyy zzz", "4/20/2096 +00:00", new CultureInfo("en-US") };
		yield return new object[] { "M/d/yyyy h:mm tt zzz", "4/20/2096 4:20 AM +00:00", new CultureInfo("en-US") };
		yield return new object[] { "dd-MM-yyyy HH:mm zzz", "20-04-2096 04:20 +00:00", new CultureInfo("nl-NL") };
	}

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void SerializePattern_ReturnsString(string pattern, string expectedValue, CultureInfo cultureInfo)
	{
		// Arrange
		var expectedText = Text(expectedValue);
		var expectedAttribute = Attribute(nameof(DateTimeOffsetValue), expectedValue);
		var expectedElement = Element(nameof(DateTimeOffsetValue), expectedText);

		var sut = new DateTimeOffsetByFormatConverter(pattern, cultureInfo, DateTimeStyles.AssumeUniversal);

		// Act
		var canConvert = sut.CanConvert(DateTimeOffsetValue.GetType());
		var textResult = ((IConverter<IXmlText, IXmlNode>)sut).Serialize(DateTimeOffsetValue, _contextMock.Object)!;
		var attributeResult = ((IConverter<IXmlAttribute, IXmlNode>)sut).Serialize(DateTimeOffsetValue, _contextMock.Object)!;
		var elementResult = ((IConverter<IXmlElement, IXmlNode>)sut).Serialize(DateTimeOffsetValue, _contextMock.Object)!;

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
		var attributeInput = Attribute(nameof(DateTimeOffsetValue), inputValue);
		var elementInput = Element(nameof(DateTimeOffsetValue), textInput);

		var sut = new DateTimeOffsetByFormatConverter(pattern, cultureInfo, DateTimeStyles.None);

		_contextMock
			.WithPropertyType(DateTimeOffsetValue.GetType());

		// Act

		// Act
		var canConvert = sut.CanConvert(DateTimeOffsetValue.GetType());
		var textResult = (DateTimeOffset)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (DateTimeOffset)((IConverter<IXmlAttribute, IXmlNode>)sut).Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (DateTimeOffset)((IConverter<IXmlElement, IXmlNode>)sut).Deserialize(elementInput, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();

		if (pattern == "M/d/yyyy zzz")
		{
			textResult.Should().BeSameDateAs(DateTimeOffsetValue);
			attributeResult.Should().BeSameDateAs(DateTimeOffsetValue);
			elementResult.Should().BeSameDateAs(DateTimeOffsetValue);
		}
		else
		{
			textResult.Should().Be(DateTimeOffsetValue);
			attributeResult.Should().Be(DateTimeOffsetValue);
			elementResult.Should().Be(DateTimeOffsetValue);
		}
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Text("SomeText");
		var sut = new DateTimeOffsetByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.None);

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateTimeOffset?)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid DateTime.");
	}
	#endregion
}


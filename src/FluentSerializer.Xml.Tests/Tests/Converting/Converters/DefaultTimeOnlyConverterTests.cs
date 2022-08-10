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
/// Basically test if this converter behaves exactly like <see cref="TimeOnly.ToString()"/>
/// and <see cref="TimeOnly.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultTimeOnlyConverterTests
{
	private static readonly string TimeOnlyString = "04:20:00";
	private static readonly TimeOnly TimeOnlyValue = TimeOnly.Parse(TimeOnlyString, CultureInfo.InvariantCulture);

	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public DefaultTimeOnlyConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(TimeOnlyValue)));
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "04:20:00", CultureInfo.InvariantCulture };
		yield return new object[] { "4:20 AM", new CultureInfo("en-US") };
		yield return new object[] { "04:20", new CultureInfo("nl-NL") };
	}

	public static IEnumerable<object[]> GenerateCultureOptions()
	{
		yield return new object[] { CultureInfo.InvariantCulture };
		yield return new object[] { new CultureInfo("en") };
		yield return new object[] { new CultureInfo("en-US") };
		yield return new object[] { new CultureInfo("nl-NL") };
	}

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateCultureOptions))]
	public void SerializePattern_ReturnsString(CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var expectedText = Text(TimeOnlyString);
		var expectedAttribute = Attribute(nameof(TimeOnlyValue), TimeOnlyString);
		var expectedElement = Element(nameof(TimeOnlyValue), expectedText);

		var sut = new DefaultTimeOnlyConverter();

		// Act
		var canConvert = sut.CanConvert(TimeOnlyValue.GetType());
		var textResult = ((IConverter<IXmlText, IXmlNode>)sut).Serialize(TimeOnlyValue, _contextMock.Object)!;
		var attributeResult = ((IConverter<IXmlAttribute, IXmlNode>)sut).Serialize(TimeOnlyValue, _contextMock.Object)!;
		var elementResult = ((IConverter<IXmlElement, IXmlNode>)sut).Serialize(TimeOnlyValue, _contextMock.Object)!;

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
	public void Deserialize_Convertible_ReturnsValue(string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var textInput = Text(inputValue);
		var attributeInput = Attribute(nameof(TimeOnlyValue), inputValue);
		var elementInput = Element(nameof(TimeOnlyValue), textInput);

		var sut = new DefaultTimeOnlyConverter();

		_contextMock
			.WithPropertyType(TimeOnlyValue.GetType());

		// Act
		var canConvert = sut.CanConvert(TimeOnlyValue.GetType());
		var textResult = (TimeOnly)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (TimeOnly)((IConverter<IXmlAttribute, IXmlNode>)sut).Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (TimeOnly)((IConverter<IXmlElement, IXmlNode>)sut).Deserialize(elementInput, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		textResult.Should().Be(TimeOnlyValue);
		attributeResult.Should().Be(TimeOnlyValue);
		elementResult.Should().Be(TimeOnlyValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Text("SomeText");
		var sut = new DefaultTimeOnlyConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (TimeOnly?)((IConverter<IXmlText, IXmlNode>)sut).Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid TimeOnly.");
	}
	#endregion
}


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
/// Basically test if this converter behaves exactly like <see cref="DateOnly.ToString()"/>
/// and <see cref="DateOnly.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultDateOnlyConverterTests
{
	private static readonly string DateOnlyString = "2096-04-20";
	private static readonly DateOnly DateOnlyValue = DateOnly.Parse(DateOnlyString, CultureInfo.InvariantCulture);

	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public DefaultDateOnlyConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(DateOnlyValue)));
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "2096-04-20", CultureInfo.InvariantCulture };
		yield return new object[] { "4/20/2096", new CultureInfo("en-US") };
		yield return new object[] { "20-04-2096", new CultureInfo("nl-NL") };
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
		var expectedText = Text(DateOnlyString);
		var expectedAttribute = Attribute(nameof(DateOnlyValue), DateOnlyString);
		var expectedElement = Element(nameof(DateOnlyValue), expectedText);

		var sut = new DefaultDateOnlyConverter();

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
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var textInput = Text(inputValue);
		var attributeInput = Attribute(nameof(DateOnlyValue), inputValue);
		var elementInput = Element(nameof(DateOnlyValue), textInput);

		var sut = new DefaultDateOnlyConverter();

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
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Text("SomeText");
		var sut = new DefaultDateOnlyConverter();

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
		var sut = new DefaultDateOnlyConverter();

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


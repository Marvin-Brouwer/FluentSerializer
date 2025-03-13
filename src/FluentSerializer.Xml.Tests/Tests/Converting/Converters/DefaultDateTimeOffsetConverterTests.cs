using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;
using FluentSerializer.Xml.Tests.Extensions;

using Moq;

using System;
using System.Collections.Generic;
using System.Globalization;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="DateTimeOffset.ToString()"/>
/// and <see cref="DateTimeOffset.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultDateTimeOffsetConverterTests
{
	private static readonly string DateTimeOffsetString = "2096-04-20T04:20:00+00:00";
	private static readonly DateTimeOffset DateTimeOffsetValue = DateTimeOffset.Parse(
		DateTimeOffsetString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public DefaultDateTimeOffsetConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(DateTimeOffsetValue)));
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { true, "2096-04-20 04:20:00 +0", CultureInfo.InvariantCulture };
		yield return new object[] { false, "4/20/2096 +0", new CultureInfo("en-US") };
		yield return new object[] { true, "4/20/2096 4:20 AM +0", new CultureInfo("en-US") };
		yield return new object[] { true, "20-04-2096 04:20 +0", new CultureInfo("nl-NL") };
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
		var expectedText = Text(DateTimeOffsetString);
		var expectedAttribute = Attribute(nameof(DateTimeOffsetValue), DateTimeOffsetString);
		var expectedElement = Element(nameof(DateTimeOffsetValue), expectedText);

		var sut = new DefaultDateTimeOffsetConverter();

		// Act
		var canConvert = sut.CanConvert(DateTimeOffsetValue.GetType());
		var textResult = sut.ForText().Serialize(DateTimeOffsetValue, _contextMock.Object)!;
		var attributeResult = sut.ForAttribute().Serialize(DateTimeOffsetValue, _contextMock.Object)!;
		var elementResult = sut.ForElement().Serialize(DateTimeOffsetValue, _contextMock.Object)!;

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
	public void Deserialize_Convertible_ReturnsValue(bool hasTime, string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var textInput = Text(inputValue);
		var attributeInput = Attribute(nameof(DateTimeOffsetValue), inputValue);
		var elementInput = Element(nameof(DateTimeOffsetValue), textInput);

		var sut = new DefaultDateTimeOffsetConverter();

		_contextMock
			.WithPropertyType(DateTimeOffsetValue.GetType());

		// Act
		var canConvert = sut.CanConvert(DateTimeOffsetValue.GetType());
		var textResult = (DateTimeOffset)sut.ForText().Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (DateTimeOffset)sut.ForAttribute().Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (DateTimeOffset)sut.ForElement().Deserialize(elementInput, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();

		if (!hasTime)
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
		var sut = new DefaultDateTimeOffsetConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateTimeOffset?)sut.ForText().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("The string 'SomeText' was not recognized as a valid DateTime. There is an unknown word starting at index '0'.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_EmptyString_Throws()
	{
		// Arrange
		var input = Text("");
		var sut = new DateTimeOffsetByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => (DateOnly?)sut.ForText().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String '' was not recognized as a valid DateTime.");
	}

	#endregion
}


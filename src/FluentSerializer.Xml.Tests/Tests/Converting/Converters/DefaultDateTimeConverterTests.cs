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
/// Basically test if this converter behaves exactly like <see cref="DateTime.ToString()"/>
/// and <see cref="DateTime.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultDateTimeConverterTests
{
	private static readonly string DateTimeString = "2096-04-20T04:20:00Z";
	private static readonly DateTime DateTimeValue = DateTime.Parse(
		DateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public DefaultDateTimeConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(DateTimeValue)));
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { true, "2096-04-20 04:20:00", CultureInfo.InvariantCulture };
		yield return new object[] { false, "4/20/2096", new CultureInfo("en-US") };
		yield return new object[] { true, "4/20/2096 4:20 AM", new CultureInfo("en-US") };
		yield return new object[] { true, "20-04-2096 04:20", new CultureInfo("nl-NL") };
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
		var expectedText = Text(DateTimeString);
		var expectedAttribute = Attribute(nameof(DateTimeValue), DateTimeString);
		var expectedElement = Element(nameof(DateTimeValue), expectedText);

		var sut = new DefaultDateTimeConverter();

		// Act
		var canConvert = sut.CanConvert(DateTimeValue.GetType());
		var textResult = sut.ForText().Serialize(DateTimeValue, _contextMock.Object)!;
		var attributeResult = sut.ForAttribute().Serialize(DateTimeValue, _contextMock.Object)!;
		var elementResult = sut.ForElement().Serialize(DateTimeValue, _contextMock.Object)!;

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
		var attributeInput = Attribute(nameof(DateTimeValue), inputValue);
		var elementInput = Element(nameof(DateTimeValue), textInput);

		var sut = new DefaultDateTimeConverter();

		_contextMock
			.WithPropertyType(DateTimeValue.GetType());

		// Act
		var canConvert = sut.CanConvert(DateTimeValue.GetType());
		var textResult = (DateTime)sut.ForText().Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (DateTime)sut.ForAttribute().Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (DateTime)sut.ForElement().Deserialize(elementInput, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();

		if (!hasTime)
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
		var sut = new DefaultDateTimeConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateTime?)sut.ForText().Deserialize(input, _contextMock.Object);

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
		var sut = new DefaultDateTimeConverter();

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


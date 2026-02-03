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
using System.Runtime.InteropServices;

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

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "yyyy-MM-dd HH:mm:ss zzz", "2096-04-20 04:20:00 +00:00", CultureInfo.InvariantCulture };
		yield return new object[] { "M/d/yyyy zzz", "4/20/2096 +00:00", new CultureInfo("en-US", useUserOverride: false) };
		yield return new object[] { "M/d/yyyy h:mm tt zzz", "4/20/2096 4:20 AM +00:00", new CultureInfo("en-US", useUserOverride: false) };
		yield return new object[] { "dd-MM-yyyy HH:mm zzz", "20-04-2096 04:20 +00:00", new CultureInfo("nl-NL", useUserOverride: false) };
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
		var result1 = () => new DateTimeOffsetByFormatConverter(null!, cultureInfo, dateTimeStyle);
		var result2 = () => new DateTimeOffsetByFormatConverter(format, null!, dateTimeStyle);

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
		var textResult = sut.ForText().Serialize(DateTimeOffsetValue, _contextMock.Object)!;
		var attributeResult = sut.ForAttribute().Serialize(DateTimeOffsetValue, _contextMock.Object)!;
		var elementResult = sut.ForElement().Serialize(DateTimeOffsetValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		textResult.Should().BeEquatableTo(expectedText, true, true);
		attributeResult.Should().BeEquatableTo(expectedAttribute, true, true);
		elementResult.Should().BeEquatableTo(expectedElement, true, true);
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
		var textResult = (DateTimeOffset)sut.ForText().Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (DateTimeOffset)sut.ForAttribute().Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (DateTimeOffset)sut.ForElement().Deserialize(elementInput, _contextMock.Object)!;

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
		var result = () => (DateTimeOffset?)sut.ForText().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid DateTime.");
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


using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;
using FluentSerializer.Xml.Tests.Extensions;

using Microsoft.VisualStudio.TestPlatform.TestHost;

using Moq;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="TimeOnly.ToString(string?)"/>
/// and <see cref="TimeOnly.ParseExact(string, string, IFormatProvider?, DateTimeStyles)"/>
/// </summary>
public sealed class TimeOnlyByFormatConverterTests
{
	private static readonly TimeOnly TimeOnlyValue = TimeOnly.Parse("04:20:00", CultureInfo.InvariantCulture);

	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public TimeOnlyByFormatConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(TimeOnlyValue)));
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "HH:mm:ss", "04:20:00", CultureInfo.InvariantCulture };
		yield return new object[] { "h:mm tt", "4:20 AM", new CultureInfo("en-US", useUserOverride: false) };
		yield return new object[] { "HH:mm", "04:20", new CultureInfo("nl-NL", useUserOverride: false) };
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
		var result1 = () => new TimeOnlyByFormatConverter(null!, cultureInfo, dateTimeStyle);
		var result2 = () => new TimeOnlyByFormatConverter(format, null!, dateTimeStyle);

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
		// https://github.com/dotnet/runtime/issues/113478
		expectedValue = expectedValue.Replace(" ", " ");

		// Arrange
		var expectedText = Text(expectedValue); 
		var expectedAttribute = Attribute(nameof(TimeOnlyValue), expectedValue);
		var expectedElement = Element(nameof(TimeOnlyValue), expectedText);

		var sut = new TimeOnlyByFormatConverter(pattern, cultureInfo, DateTimeStyles.AllowWhiteSpaces);

		// Act
		var canConvert = sut.CanConvert(TimeOnlyValue.GetType());
		var textResult = sut.ForText().Serialize(TimeOnlyValue, _contextMock.Object)!;
		var attributeResult = sut.ForAttribute().Serialize(TimeOnlyValue, _contextMock.Object)!;
		var elementResult = sut.ForElement().Serialize(TimeOnlyValue, _contextMock.Object)!;

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
		var attributeInput = Attribute(nameof(TimeOnlyValue), inputValue);
		var elementInput = Element(nameof(TimeOnlyValue), textInput);

		var sut = new TimeOnlyByFormatConverter(pattern, cultureInfo, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(TimeOnlyValue.GetType());

		// Act
		var canConvert = sut.CanConvert(TimeOnlyValue.GetType());
		var textResult = (TimeOnly)sut.ForText().Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (TimeOnly)sut.ForAttribute().Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (TimeOnly)sut.ForElement().Deserialize(elementInput, _contextMock.Object)!;

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
		var sut = new TimeOnlyByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (TimeOnly?)sut.ForText().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid TimeOnly.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_EmptyString_Throws()
	{
		// Arrange
		var input = Text("");
		var sut = new TimeOnlyByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => (DateOnly?)sut.ForText().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String '' was not recognized as a valid TimeOnly.");
	}

	#endregion
}


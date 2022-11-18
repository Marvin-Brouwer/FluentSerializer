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
/// Basically test if this converter behaves exactly like <see cref="TimeSpan.ToString()"/>
/// and <see cref="TimeSpan.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultTimeSpanConverterTests
{
	private static readonly string TimeSpanString = "04:20:00";
	private static readonly TimeSpan TimeSpanValue = TimeSpan.Parse(TimeSpanString, CultureInfo.InvariantCulture);

	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;

	public DefaultTimeSpanConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>();
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Equal(nameof(TimeSpanValue)));
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "04:20:00" };
		yield return new object[] { "04:20" };
	}

	#region Serialize

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void SerializePattern_ReturnsString()
	{
		// Arrange
		var expectedText = Text(TimeSpanString);
		var expectedAttribute = Attribute(nameof(TimeSpanValue), TimeSpanString);
		var expectedElement = Element(nameof(TimeSpanValue), expectedText);

		var sut = new DefaultTimeSpanConverter();

		// Act
		var canConvert = sut.CanConvert(TimeSpanValue.GetType());
		var textResult = sut.ForText().Serialize(TimeSpanValue, _contextMock.Object)!;
		var attributeResult = sut.ForAttribute().Serialize(TimeSpanValue, _contextMock.Object)!;
		var elementResult = sut.ForElement().Serialize(TimeSpanValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		textResult.Should().BeEquatableTo(expectedText);
		attributeResult.Should().BeEquatableTo(expectedAttribute);
		elementResult.Should().BeEquatableTo(expectedElement);
	}
	#endregion

	#region Deserialize
	[Theory,
		MemberData(nameof(GenerateConvertibleData)),
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_ReturnsValue(string inputValue)
	{
		// Arrange
		var textInput = Text(inputValue);
		var attributeInput = Attribute(nameof(TimeSpanValue), inputValue);
		var elementInput = Element(nameof(TimeSpanValue), textInput);

		var sut = new DefaultTimeSpanConverter();

		_contextMock
			.WithPropertyType(TimeSpanValue.GetType());

		// Act
		var canConvert = sut.CanConvert(TimeSpanValue.GetType());
		var textResult = (TimeSpan)sut.ForText().Deserialize(textInput, _contextMock.Object)!;
		var attributeResult = (TimeSpan)sut.ForAttribute().Deserialize(attributeInput, _contextMock.Object)!;
		var elementResult = (TimeSpan)sut.ForElement().Deserialize(elementInput, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		textResult.Should().Be(TimeSpanValue);
		attributeResult.Should().Be(TimeSpanValue);
		elementResult.Should().Be(TimeSpanValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Text("SomeText");
		var sut = new DefaultTimeSpanConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (TimeSpan?)sut.ForText().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid TimeSpan.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Convertible_EmptyString_Throws()
	{
		// Arrange
		var input = Text("");
		var sut = new DefaultTimeSpanConverter();

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => (DateOnly?)sut.ForText().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String '' was not recognized as a valid TimeSpan.");
	}

	#endregion
}


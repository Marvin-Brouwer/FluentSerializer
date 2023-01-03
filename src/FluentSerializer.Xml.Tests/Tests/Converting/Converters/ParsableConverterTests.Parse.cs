using FluentAssertions;

using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;

using System;
using System.Data;
using System.IO;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

using Sut = FluentSerializer.Xml.Converting.Converters.ParsableConverter;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.Tostring"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed partial class ParsableConverterTests
{
	#region Serialize
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Parse_ThrowsNotSupported()
	{
		// Arrange
		const int input = 0;

		// Act
		var canConvert = SutParse().CanConvert(input.GetType());
		var resultText = () => SutParse().As<IXmlConverter<IXmlText>>().Serialize(input, _contextMock.Object);
		var resultAttribute = () => SutParse().As<IXmlConverter<IXmlAttribute>>().Serialize(input, _contextMock.Object);
		var resultElement = () => SutParse().As<IXmlConverter<IXmlElement>>().Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		resultText.Should()
			.ThrowExactly<NotSupportedException>();
		resultAttribute.Should()
			.ThrowExactly<NotSupportedException>();
		resultElement.Should()
			.ThrowExactly<NotSupportedException>();
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Parse_EmptyText_ReturnsDefault(object requested, string unused)
	{
		_ = unused;

		// Arrange
		var input = Text(string.Empty);
		var expected = (object?)null;

		_contextMock
			.WithPropertyType(requested.GetType());

		// Act
		var canConvert = SutParse().CanConvert(requested.GetType());
		var result = SutParse().As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Parse_Parsible_ReturnsText(object expected, string inputText)
	{
		// Arrange
		var input = Text(inputText);

		_contextMock
			.WithPropertyType(expected.GetType());

		// Act
		var canConvert = SutParse(TestCulture).CanConvert(expected.GetType());
		var result = SutParse(TestCulture).As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Parse_Parsible_EmptyString_ReturnsEmptyString()
	{
		// Arrange
		var input = Text(string.Empty);
		const string? expected = null;

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = SutParse().As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should().Be(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Parse_Parsible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Text("SomeText");

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => SutParse().As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage($"The input string '{input.Value}' was not in a correct format.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Parse_NonParsible_Throws()
	{
		// Arrange
		var input = Text("Doesn't matter");

		_contextMock
			.WithPropertyType(typeof(Stream));

		// Act
		var canConvert = SutParse().CanConvert(typeof(Stream));
		var result = () => SutParse().As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<EntryPointNotFoundException>()
			.WithMessage("The method 'Parse' was not found on IParsable type");
	}
	#endregion
}

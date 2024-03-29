using FluentAssertions;

using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

using System;
using System.IO;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="IParsable{TSelf}"/>
/// </summary>
public sealed partial class ParsableConverterTests
{
	#region Serialize
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_TryParse_ThrowsNotSupported()
	{
		// Arrange
		const int input = 0;

		// Act
		var canConvert = SutTryParse().CanConvert(input.GetType());
		var resultText = () => SutTryParse().As<IXmlConverter<IXmlText>>().Serialize(input, _contextMock.Object);
		var resultAttribute = () => SutTryParse().As<IXmlConverter<IXmlAttribute>>().Serialize(input, _contextMock.Object);
		var resultElement = () => SutTryParse().As<IXmlConverter<IXmlElement>>().Serialize(input, _contextMock.Object);

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
	public void Deserialize_TryParse_EmptyText_ReturnsDefault(object requested, string unused)
	{
		_ = unused;

		// Arrange
		var input = Text(string.Empty);
		var expected = (object?)null;

		_contextMock
			.WithPropertyType(requested.GetType());

		// Act
		var canConvert = SutTryParse().CanConvert(requested.GetType());
		var result = SutTryParse().As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_TryParse_Parsible_ReturnsText(object expected, string inputText)
	{
		// Arrange
		var input = Text(inputText);

		_contextMock
			.WithPropertyType(expected.GetType());

		// Act
		var canConvert = SutTryParse(TestCulture).CanConvert(expected.GetType());
		var result = SutTryParse(TestCulture).As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_TryParse_Parsible_EmptyString_ReturnsNull()
	{
		// Arrange
		var input = Text("");
		const string? expected = null;

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = SutTryParse().As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should().Be(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_TryParse_Parsible_IncorrectFormat_ReturnsNull()
	{
		// Arrange
		var input = Text("SomeText");

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = SutTryParse().As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_TryParse_NonParsible_Throws()
	{
		// Arrange
		var input = Text("Doesn't matter");

		_contextMock
			.WithPropertyType(typeof(Stream));

		// Act
		var canConvert = SutTryParse().CanConvert(typeof(Stream));
		var result = () => SutTryParse().As<IXmlConverter<IXmlText>>().Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<EntryPointNotFoundException>()
			.WithMessage("The method 'TryParse' was not found on IParsable type");
	}
	#endregion
}

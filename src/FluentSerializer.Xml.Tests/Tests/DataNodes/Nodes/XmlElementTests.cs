using FluentAssertions;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed class XmlElementTests
{
	private const string XmlElementName = "Element";
	private static readonly IXmlText XmlElementValue = Text("69");

	private ITextWriter _textWriter;

	public XmlElementTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	#region Parse
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Valid_ReturnsElement()
	{
		// Arrange
		var expected = Element(XmlElementName, XmlElementValue);
		var input = $"<Element>{XmlElementValue}</Element>";

		// Act
		var offset = 0;
		var result = new XmlElement(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseXml_ValidWithWhiteSpace_ReturnsElement(string space)
	{
		// Arrange
		var expectedPlain = Element(XmlElementName, XmlElementValue);
		var expectedEmptyPlain = Element(XmlElementName);
		var expectedWithAttribute = Element(XmlElementName, Attribute("attribute", "test"), XmlElementValue);
		var expectedEmptyWithAttribute = Element(XmlElementName, Attribute("attribute", "test"));
		var input1 = $"<{space}Element{space}>{XmlElementValue}</{space}Element{space}>{space}";
		var input2 = $"<{space}Element{space}></{space}Element{space}>{space}";
		var input3 = $"<{space}Element{space}/>{space}";
		var input4 = $"<{space}Element{space}attribute=\"test\"{space}>{XmlElementValue}</{space}Element{space}>{space}";
		var input5 = $"<{space}Element{space}attribute=\"test\"{space}></{space}Element{space}>{space}";
		var input6 = $"<{space}Element{space}attribute=\"test\"{space}/>{space}";

		// Act
		var offset1 = 0;
		var result1 = new XmlElement(input1, ref offset1);
		var offset2 = 0;
		var result2 = new XmlElement(input2, ref offset2);
		var offset3 = 0;
		var result3 = new XmlElement(input3, ref offset3);
		var offset4 = 0;
		var result4 = new XmlElement(input4, ref offset4);
		var offset5 = 0;
		var result5 = new XmlElement(input5, ref offset5);
		var offset6 = 0;
		var result6 = new XmlElement(input6, ref offset6);

		// Assert
		result1.Should().BeEquatableTo(expectedPlain);
		result2.Should().BeEquatableTo(expectedEmptyPlain);
		result3.Should().BeEquatableTo(expectedEmptyPlain);
		result4.Should().BeEquatableTo(expectedWithAttribute);
		result5.Should().BeEquatableTo(expectedEmptyWithAttribute);
		result6.Should().BeEquatableTo(expectedEmptyWithAttribute);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Empty_ReturnsElement()
	{
		// Arrange
		var expected = Element(XmlElementName, Text(null));
		var input = "<Element />";

		// Act
		var offset = 0;
		var result = new XmlElement(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_InvalidName_ReturnsElementWithInvalidName()
	{
		// Arrange
		var input = "<E!!ement$></E!!ement$>";

		// Act
		var offset = 0;
		var result = new XmlElement(input, ref offset);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveInvalidName();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_InvalidStructure_ReturnsElementsWithInvalidName()
	{
		// Arrange
		var input1 = "<element <Element />";
		var input2 = "<element <Element></Element>";

		// Act
		var offset1 = 0;
		var result1 = new XmlElement(input1, ref offset1);
		var offset2 = 0;
		var result2 = new XmlElement(input2, ref offset2);

		// Assert
		// These have an invalid name this will result in properties not being found
		// instead of throwing parsing errors
		result1.Name.Should().Be("element");
		result1.Children[0].Name.Should().Be("<Element");
		result1.Children[0].Should().HaveInvalidName();
		result2.Name.Should().Be("element");
		result2.Children[0].Name.Should().Be("<Elemen");
		result2.Children[0].Should().HaveInvalidName();
		result2.Children[1].Name.Should().BeEmpty();
		result2.Children[1].Should().HaveInvalidName();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Incomplete_Throws()
	{
		// Arrange
		var input = "Element";

		// Act
		var offset = 0;
		var result = () => new XmlElement(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}
	#endregion

	#region ToString
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = Element(XmlElementName, XmlElementValue);
		var expected = $"<Element>{XmlElementValue}</Element>";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void AppendTo_HasNoValue_FormatWriteNull_ReturnsEmptyElement()
	{
		// Arrange
		var input = Element(XmlElementName, Text(null));
		var expected = "<Element />";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void AppendTo_HasNoValue_FormatDontWriteNull_ReturnsEmptyString()
	{
		// Arrange
		var input = Element(XmlElementName, Text(null));
		var expected = string.Empty;

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
	#endregion
}
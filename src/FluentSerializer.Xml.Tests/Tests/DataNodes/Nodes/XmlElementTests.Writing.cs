using FluentSerializer.Core.TestUtils.Extensions;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlElementTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
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
}
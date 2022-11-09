using FluentSerializer.Core.TestUtils.Extensions;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlAttributeTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = Attribute(XmlAttributeName, XmlAttributeValue);
		var expected = $"attribute=\"{XmlAttributeValue}\"";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void AppendTo_HasNoValue_FormatWriteNull_ReturnsEmptyAttribute()
	{
		// Arrange
		var input = Attribute(XmlAttributeName, null);
		var expected = "attribute=\"\"";

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
		var input = Attribute(XmlAttributeName, null);
		var expected = string.Empty;

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
}
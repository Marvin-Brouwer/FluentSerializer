using FluentSerializer.Core.TestUtils.Extensions;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlCharacterDataTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = CData(XmlCharacterDataValue);
		var expected = $"<![CDATA[{XmlCharacterDataValue}]]>";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void AppendTo_HasNoValue_FormatWriteNull_ReturnsEmptyCharacterData()
	{
		// Arrange
		var input = CData(string.Empty);
		var expected = "<![CDATA[]]>";

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
		var input = CData(string.Empty);
		var expected = string.Empty;

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
}
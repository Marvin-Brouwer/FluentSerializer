using FluentAssertions;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes.Nodes;
using System;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed class XmlCharacterDataTests
{
	private const string XmlCharacterDataValue = "\n<p>\n\t69\n\t</p>\n";

	private ITextWriter _textWriter;

	public XmlCharacterDataTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	#region Parse
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Valid_ReturnsCharacterData()
	{
		// Arrange
		var expected = CData(XmlCharacterDataValue);
		var input = $"<![CDATA[{XmlCharacterDataValue}]]>";

		// Act
		var offset = 0;
		var result = new XmlCharacterData(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		InlineData(" "), InlineData("  "), InlineData("\t")]
	public void ParseXml_ValidWithWhiteSpace_ReturnsCharacterData(string space)
	{
		// Arrange
		var expected = CData(XmlCharacterDataValue);
		// No whitespace at start supported
		var input = $"<![CDATA[{XmlCharacterDataValue}]]>{space}";

		// Act
		var offset = 0;
		var result = new XmlCharacterData(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Empty_ReturnsCharacterData()
	{
		// Arrange
		var expected = CData(string.Empty);
		var input = "<![CDATA[]]>";

		// Act
		var offset = 0;
		var result = new XmlCharacterData(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Incomplete_Throws()
	{
		// Arrange
		// Arrange
		var input = $"<![CDATA[{XmlCharacterDataValue}";

		// Act
		var offset = 0;
		var result = () => new XmlCharacterData(input, ref offset);

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
		var input = CData(XmlCharacterDataValue);
		var expected = $"<![CDATA[{XmlCharacterDataValue}]]>";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
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
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
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
	#endregion
}
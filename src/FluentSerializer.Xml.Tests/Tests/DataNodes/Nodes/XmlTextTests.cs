using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes.Nodes;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed class XmlTextTests
{
	private const string XmlTextValue = "69\n69";

	private ITextWriter _textWriter;

	public XmlTextTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	#region Parse
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Valid_ReturnsText()
	{
		// Arrange
		var expected = Text(XmlTextValue);
		var input = $"{XmlTextValue}<";

		// Act
		var offset = 0;
		var result = new XmlText(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseXml_ValidWithWhiteSpace_ReturnsText(string space)
	{
		// Arrange
		var expected = Text(XmlTextValue);
		var input = $"{space}{XmlTextValue}{space}<";

		// Act
		var offset = 0;
		var result = new XmlText(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Empty_ReturnsText()
	{
		// Arrange
		var expected = Text(string.Empty);
		var input = "<";

		// Act
		var offset = 0;
		var result = new XmlText(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
	#endregion

	#region ToString
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = Text(XmlTextValue);
		var expected = XmlTextValue;

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void AppendTo_HasNoValue_FormatWriteNull_ReturnsEmptyText()
	{
		// Arrange
		var input = Text(null);
		var expected = string.Empty;

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
		var input = Text(null);
		var expected = string.Empty;

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
	#endregion
}
using FluentAssertions;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes.Nodes;
using System;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed class XmlCommentTests
{
	private const string XmlCommentValue = "69\n69";

	private ITextWriter _textWriter;

	public XmlCommentTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	#region Parse
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Valid_ReturnsComment()
	{
		// Arrange
		var expected = Comment(XmlCommentValue);
		var input = $"<!-- {XmlCommentValue} -->";

		// Act
		var offset = 0;
		var result = new XmlComment(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML"),
		InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseXml_ValidWithWhiteSpace_ReturnsComment(string space)
	{
		// Arrange
		var expected = Comment(XmlCommentValue);
		var input = $"<!--{space}{XmlCommentValue}{space}-->{space}";

		// Act
		var offset = 0;
		var result = new XmlComment(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Empty_ReturnsComment()
	{
		// Arrange
		var expected = Comment(string.Empty);
		var input = "<!--  -->";

		// Act
		var offset = 0;
		var result = new XmlComment(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Incomplete_Throws()
	{
		// Arrange
		var expected = Comment(string.Empty);
		var input = $"<!-- {XmlCommentValue}";

		// Act
		var offset = 0;
		var result = () => new XmlComment(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}
	#endregion

	#region ToString
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = Comment(XmlCommentValue);
		var expected = $"<!-- {XmlCommentValue} -->";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void AppendTo_HasNoValue_FormatWriteNull_ReturnsEmptyComment()
	{
		// Arrange
		var input = Comment(string.Empty);
		var expected = "<!--  -->";

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
		var input = Comment(string.Empty);
		var expected = string.Empty;

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
	#endregion
}
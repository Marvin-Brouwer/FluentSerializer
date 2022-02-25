using FluentAssertions;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes.Nodes;
using System;
using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed class JsonCommentMultiLineTests
{
	private ITextWriter _textWriter;

	public JsonCommentMultiLineTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	#region Parse
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Valid_ReturnsObject()
	{
		// Arrange
		var expected = MultilineComment("test\ntest");
		var input = "/* test\ntest */";

		// Act
		var offset = 0;
		var result = new JsonCommentMultiLine(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		InlineData(""), InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseJson_ValidWithWhiteSpace_ReturnsObject(string space)
	{
		// Arrange
		var expected = MultilineComment("test\ntest");
		var expectedEmpty = new JsonCommentMultiLine(string.Empty);
		var input1 = $"/*{space}test\ntest{space}*/{space}";
		var input2 = $"/*{space}*/{space}";

		// Act
		var offset1 = 0;
		var result1 = new JsonCommentMultiLine(input1, ref offset1);
		var offset2 = 0;
		var result2 = new JsonCommentMultiLine(input2, ref offset2);

		// Assert
		result1.Should().BeEquatableTo(expected);
		result2.Should().BeEquatableTo(expectedEmpty);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Empty_ReturnsObject()
	{
		// Arrange
		var expected = new JsonCommentMultiLine(string.Empty);
		var input = "/* */";

		// Act
		var offset = 0;
		var result = new JsonCommentMultiLine(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Incomplete_Throws()
	{
		// Arrange
		var input = "/* 14";

		// Act
		var offset = 0;
		var result = () => new JsonCommentMultiLine(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}
	#endregion

	#region ToString
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasCommentMultiLine_FormatWriteNull_ReturnsCommentMultiLine()
	{
		// Arrange
		var input = MultilineComment("test\ntest");
		var expected = "/* test\ntest */";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasNoCommentMultiLine_FormatWriteNull_ReturnsEmptyCommentMultiLine()
	{
		// Arrange
		var input = new JsonCommentMultiLine(string.Empty);
		var expected = "/*  */";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasNoCommentMultiLine_FormatDontWriteNull_ReturnsEmptyString()
	{
		// Arrange
		var input = new JsonCommentMultiLine(string.Empty);
		var expected = string.Empty;

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
	#endregion
}
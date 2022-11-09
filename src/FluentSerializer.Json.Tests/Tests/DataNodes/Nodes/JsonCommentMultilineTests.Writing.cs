using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonCommentMultiLineTests
{
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
}
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonCommentSingleLineTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasCommentSingleLine_FormatWriteNull_ReturnsCommentSingleLine()
	{
		// Arrange
		var input = Comment("test");
		var expected = "// test";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasNoCommentSingleLine_FormatWriteNull_ReturnsEmptyCommentSingleLine()
	{
		// Arrange
		var input = new JsonCommentSingleLine(string.Empty);
		var expected = "// ";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasNoCommentSingleLine_FormatDontWriteNull_ReturnsEmptyString()
	{
		// Arrange
		var input = new JsonCommentSingleLine(string.Empty);
		var expected = string.Empty;

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	// Special case, when formatting is turned of this should print the multiline syntax as a safeguard
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasCommentSingleLine_NoFormatWriteNull_ReturnsCommentMultiLine()
	{
		// Arrange
		var input = Comment("test");
		var expected = "/* test */";

		// Act
		input.AppendTo(ref _textWriter, false, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
}
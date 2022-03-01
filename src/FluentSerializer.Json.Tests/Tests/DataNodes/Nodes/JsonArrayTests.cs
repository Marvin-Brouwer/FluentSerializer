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
public sealed class JsonArrayTests
{
	private ITextWriter _textWriter;

	public JsonArrayTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	#region Parse
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Valid_ReturnsObject()
	{
		// Arrange
		var expected = Array(Object());
		var input = "[ { } ]";

		// Act
		var offset = 0;
		var result = new JsonArray(input, ref offset);

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
		var expected = Array(Object());
		var expectedEmpty = Array();
		var input1 = $"[{space}{{ }}{space}]{space}";
		var input2 = $"[{space}]{space}";

		// Act
		var offset1 = 0;
		var result1 = new JsonArray(input1, ref offset1);
		var offset2 = 0;
		var result2 = new JsonArray(input2, ref offset2);

		// Assert
		result1.Should().BeEquatableTo(expected);
		result2.Should().BeEquatableTo(expectedEmpty);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Empty_ReturnsObject()
	{
		// Arrange
		var expected = Array();
		var input = "[]";

		// Act
		var offset = 0;
		var result = new JsonArray(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Incomplete_Throws()
	{
		// Arrange
		var input = "[ { } ";

		// Act
		var offset = 0;
		var result = () => new JsonArray(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}
	#endregion

	#region ToString
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = Array(Object());
		var expected = "[\n\t{\n\t}\n]";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasNoValue_FormatWriteNull_ReturnsEmptyArray()
	{
		// Arrange
		var input = Array();
		var expected = "[\n]";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void AppendTo_HasNoValue_FormatDontWriteNull_ReturnsEmptyArray()
	{
		// Arrange
		var input = Array();
		var expected = "[\n]";

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
	#endregion
}
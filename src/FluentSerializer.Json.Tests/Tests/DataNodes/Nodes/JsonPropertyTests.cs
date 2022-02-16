using FluentAssertions;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;
using System;
using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed class JsonPropertyTests
{
	private const string JsonPropertyName = "property";
	private static readonly IJsonValue JsonValue = Value("69");

	private ITextWriter _textWriter;

	public JsonPropertyTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	#region Parse
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ParseJson_Valid_ReturnsProperty()
	{
		// Arrange
		var expected = Property(JsonPropertyName, JsonValue);
		var input = $"\"property\": {JsonValue} ";

		// Act
		var offset = 0;
		var result = new JsonProperty(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(""), InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseJson_ValidWithWhiteSpace_ReturnsProperty(string space)
	{
		// Arrange
		var expected = Property(JsonPropertyName, JsonValue);
		var expectedEmpty = Property(JsonPropertyName, Value(null));
		var input1 = $"\"property\"{space}:{space}{JsonValue}{space},{space}";
		var input2 = $"\"property\"{space}:{space}null{space},{space}";

		// Act
		var offset1 = 0;
		var result1 = new JsonProperty(input1, ref offset1);
		var offset2 = 0;
		var result2 = new JsonProperty(input2, ref offset2);

		// Assert
		result1.Should().BeEquatableTo(expected);
		result2.Should().BeEquatableTo(expectedEmpty);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ParseJson_Incomplete_Throws()
	{
		// Arrange
		var input = "\"prop\": \"14";

		// Act
		var offset = 0;
		var result = () => new JsonObject(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<IndexOutOfRangeException>();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ParseJson_InvalidName_ReturnsPropertyWithInvalidName()
	{
		// Arrange
		var input = "\"pro!!ert*\": null,";

		// Act
		var offset = 0;
		var result = new JsonProperty(input, ref offset);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveInvalidName();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ParseJson_InvalidStructure_ReturnsPropertysWithInvalidName()
	{
		// Arrange
		var input1 = "{ { } }";
		var input2 = "\"property: } ], \"prop2\": 0,";

		// Act
		var offset1 = 0;
		var result1 = new JsonProperty(input1, ref offset1);
		var offset2 = 0;
		var result2 = new JsonProperty(input2, ref offset2);

		// Assert
		// These have an invalid name this will result in properties not being found
		// instead of throwing parsing errors
		result1.Name.Should().Be("{ } }");
		result1.Should().HaveInvalidName();
		result2.Name.Should().Be("property: } ],");
		result2.Should().HaveInvalidName();
	}
	#endregion

	#region ToString
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = Property(JsonPropertyName, JsonValue);
		var expected = $"\"property\": {JsonValue}";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void AppendTo_HasNoValue_FormatWriteNull_ReturnsNullValue()
	{
		// Arrange
		var input = Property(JsonPropertyName, Value(null));
		var expected = $"\"property\": null";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void AppendTo_HasNoValue_FormatDontWriteNull_ReturnsEmptyString()
	{
		// Arrange
		var input = Property(JsonPropertyName, Value(null));
		var expected = string.Empty;

		// Act
		input.AppendTo(ref _textWriter, true, 0, false);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
	#endregion
}
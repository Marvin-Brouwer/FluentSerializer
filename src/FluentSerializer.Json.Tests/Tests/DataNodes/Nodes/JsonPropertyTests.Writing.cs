using FluentSerializer.Core.TestUtils.Extensions;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonPropertyTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
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
}
using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonPropertyTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
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

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(""), InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseJson_OnlyWhiteSpace_ReturnsObject(string input)
	{
		// Act
		var offset = 0;
		var result = new JsonProperty(input, ref offset);

		// Assert
		result.Name.Should().BeEquivalentTo("<unknown>");
		result.Value!.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
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
}
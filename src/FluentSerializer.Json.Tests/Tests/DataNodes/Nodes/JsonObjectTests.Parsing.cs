using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonObjectTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Valid_ReturnsObject()
	{
		// Arrange
		var expected = Object(JsonProperty);
		var input = $"{{ {JsonProperty} }}";

		// Act
		var offset = 0;
		var result = new JsonObject(input, ref offset);

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
		var expected = Object(JsonProperty);
		var expectedEmpty = Object();
		var input1 = $"{{{space}{JsonProperty}{space}}}{space}";
		var input2 = $"{{{space}}}{space}";

		// Act
		var offset1 = 0;
		var result1 = new JsonObject(input1, ref offset1);
		var offset2 = 0;
		var result2 = new JsonObject(input2, ref offset2);

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
		// Arrange
		var expected = Object();

		// Act
		var offset = 0;
		var result = new JsonObject(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Empty_ReturnsObject()
	{
		// Arrange
		var expected = Object();
		var input = "{}";

		// Act
		var offset = 0;
		var result = new JsonObject(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Incomplete_Throws()
	{
		// Arrange
		var input = "{ \"prop\": 14, ";

		// Act
		var offset = 0;
		var result = () => new JsonObject(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}
}
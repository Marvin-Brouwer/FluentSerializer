using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonValueTests
{
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ParseJson_Valid_ReturnsObject()
	{
		// Arrange
		var expected = Value("test");
		var input = "test,";

		// Act
		var offset = 0;
		var result = new JsonValue(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(""), InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseJson_ValidWithWhiteSpace_ReturnsObject(string space)
	{
		// Arrange
		var expected = Value("true");
		var expectedString = Value("\"test\"");
		var expectedEmpty = Value(string.Empty);
		var expectedNull = Value(null);
		var input1 = $"true{space},{space}";
		var input2 = $"\"test\"{space},{space}";
		var input3 = $"{space},{space}";
		var input4 = $"null{space},{space}";

		// Act
		var offset1 = 0;
		var result1 = new JsonValue(input1, ref offset1);
		var offset2 = 0;
		var result2 = new JsonValue(input2, ref offset2);
		var offset3 = 0;
		var result3 = new JsonValue(input3, ref offset3);
		var offset4 = 0;
		var result4 = new JsonValue(input4, ref offset4);

		// Assert
		result1.Should().BeEquatableTo(expected);
		result2.Should().BeEquatableTo(expectedString);
		result3.Should().BeEquatableTo(expectedEmpty);
		result4.Should().BeEquatableTo(expectedNull);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(""), InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseJson_OnlyWhiteSpace_ReturnsObject(string input)
	{
		// Arrange
		var expected = string.Empty;

		// Act
		var offset = 0;
		var result = new JsonValue(input, ref offset);

		// Assert
		result.Value.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(",", ""), InlineData("null,", null)]
	public void ParseJson_Empty_ReturnsObject(string? input, string? expectedValue)
	{
		// Arrange
		var expected = Value(expectedValue);

		// Act
		var offset = 0;
		var result = new JsonValue(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	/// <summary>
	/// A value knows it's done by either a comma ',' or a whitespace character for regular values.
	/// Or an end-quote '"' for string values
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ParseJson_IncompleteValue_FailsGracefully()
	{
		// Arrange
		var expected = Value("14");
		var input = "14";

		// Act
		var offset = 0;
		var result = new JsonValue(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	/// <summary>
	/// A value knows it's done by either a comma ',' or a whitespace character for regular values.
	/// Or an endquote '"' for string values
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ParseJson_IncompleteString_Throws()
	{
		// Arrange
		var input = "\"te st";

		// Act
		var offset = 0;
		var result = () => new JsonValue(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}
}
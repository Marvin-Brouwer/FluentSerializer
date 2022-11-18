using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonArrayTests
{
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

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(""), InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseJson_OnlyWhiteSpace_ReturnsObject(string input)
	{
		// Arrange
		var expected = Array();

		// Act
		var offset = 0;
		var result = new JsonArray(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
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
}
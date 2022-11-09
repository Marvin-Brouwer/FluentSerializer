using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonCommentSingleLineTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Valid_ReturnsObject()
	{
		// Arrange
		var expected = Comment("test");
		var input = "// test\n";

		// Act
		var offset = 0;
		var result = new JsonCommentSingleLine(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		InlineData(" "), InlineData("  "), InlineData("\t")]
	public void ParseJson_ValidWithWhiteSpace_ReturnsObject(string space)
	{
		// Arrange
		var expected = Comment("test");
		var expectedEmpty = new JsonCommentSingleLine(string.Empty);
		var input1 = $"//{space}test{space}\n{space}";
		var input2 = $"//{space}\n{space}";

		// Act
		var offset1 = 0;
		var result1 = new JsonCommentSingleLine(input1, ref offset1);
		var offset2 = 0;
		var result2 = new JsonCommentSingleLine(input2, ref offset2);

		// Assert
		result1.Should().BeEquatableTo(expected);
		result2.Should().BeEquatableTo(expectedEmpty);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Empty_ReturnsObject()
	{
		// Arrange
		var expected = new JsonCommentSingleLine(string.Empty);
		var input = "// \n";

		// Act
		var offset = 0;
		var result = new JsonCommentSingleLine(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void ParseJson_Incomplete_Throws()
	{
		// Arrange
		var input = "// 14";

		// Act
		var offset = 0;
		var result = () => new JsonCommentSingleLine(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<IndexOutOfRangeException>();
	}
}
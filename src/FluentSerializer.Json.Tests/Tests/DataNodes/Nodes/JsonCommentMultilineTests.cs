using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonCommentMultiLineTests
{
	private ITextWriter _textWriter;

	public JsonCommentMultiLineTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeComment_ValueNull_ReturnsEmpty()
	{
		// Arrange
		var expected = (string?)null;
		var input = (string)null!;

		// Act
		var result = new JsonCommentMultiLine(in input);

		// Assert
		result.Value.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeComment_ValueEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = string.Empty;
		var input = string.Empty;

		// Act
		var result = new JsonCommentMultiLine(in input);

		// Assert
		result.Value.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeComment_HasValue_ReturnsWithChild()
	{
		// Arrange
		var expected = MultilineComment("I have a value!");
		var input = "I have a value!";

		// Act
		var result = new JsonCommentMultiLine(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}
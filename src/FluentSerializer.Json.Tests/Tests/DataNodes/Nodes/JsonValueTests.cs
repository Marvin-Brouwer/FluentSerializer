using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonValueTests
{
	private ITextWriter _textWriter;

	public JsonValueTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeValue_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = Value(null);
		var input = (string?)null;

		// Act
		var result = new JsonValue(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeValue_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = Value(string.Empty);
		var input = string.Empty;

		// Act
		var result = new JsonValue(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeValue_HasChild_ReturnsWithChild()
	{
		// Arrange
		var expected = Value("I have a value!");
		var input = "I have a value!";

		// Act
		var result = new JsonValue(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}
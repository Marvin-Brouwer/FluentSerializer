using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonPropertyTests
{
	private const string JsonPropertyName = "property";
	private static readonly IJsonValue JsonValue = Value("69");

	private ITextWriter _textWriter;

	public JsonPropertyTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeProperty_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = Property(nameof(Property), Value(null));
		var input = (IJsonPropertyContent?)null!;

		// Act
		var result = new JsonProperty(nameof(Property), in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeProperty_HasChild_ReturnsWithChild()
	{
		// Arrange
		var expected = Property(nameof(Property), Object());
		var input = Object();

		// Act
		var result = new JsonProperty(nameof(Property), input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}
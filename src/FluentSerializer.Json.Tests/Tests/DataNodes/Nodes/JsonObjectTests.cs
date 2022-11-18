using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;

using System.Collections.Generic;
using System.Linq;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonObjectTests
{
	private static readonly IJsonValue JsonValue = Value("69");
	private static readonly IJsonProperty JsonProperty = Property("value", in JsonValue);

	private ITextWriter _textWriter;

	public JsonObjectTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeObject_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = Object();
		var input = (IEnumerable<IJsonObjectContent>?)null!;

		// Act
		var result = new JsonObject(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeObject_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = Object();
		var input1 = Enumerable.Empty<IJsonObjectContent>();
		var input2 = System.Array.Empty<IJsonObjectContent>();

		// Act
		var result1 = new JsonObject(in input1);
		var result2 = new JsonObject(input2);

		// Assert
		result1.Should().BeEquatableTo(expected);
		result2.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeObject_HasChild_ReturnsWithChild()
	{
		// Arrange
		var expected = Object(Property(nameof(Property), Value(null)));
		var input = new[]
		{
			Property(nameof(Property), Value(null))
		};

		// Act
		var result = new JsonObject(input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}
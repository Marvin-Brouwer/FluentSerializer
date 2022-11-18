using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;

using System;

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
	public void InitializeProperty_InvalidName_Throws()
	{
		// Arrange
		var name = "This name is: <invalid>";
		var input = (IJsonPropertyContent?)null!;

		// Act
		var result = () => new JsonProperty(in name, in input);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName(nameof(name));
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
		result.HasValue.Should().BeFalse();
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
		result.HasValue.Should().BeTrue();
	}
}
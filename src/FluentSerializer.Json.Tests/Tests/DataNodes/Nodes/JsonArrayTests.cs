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

public sealed partial class JsonArrayTests
{
	private ITextWriter _textWriter;

	public JsonArrayTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeArray_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = Array();
		var input = (IEnumerable<IJsonArrayContent>?)null!;

		// Act
		var result = new JsonArray(input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeArray_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = Array();
		var input1 = Enumerable.Empty<IJsonArrayContent>();
		var input2 = System.Array.Empty<IJsonArrayContent>();

		// Act
		var result1 = new JsonArray(input1);
		var result2 = new JsonArray(input2);

		// Assert
		result1.Should().BeEquatableTo(expected);
		result2.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeArray_HasChild_ReturnsWithChild()
	{
		// Arrange
		var expected = Array(Object());
		var input = new []
		{
			Object()
		};

		// Act
		var result = new JsonArray(input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}
using FluentSerializer.Core.TestUtils.Extensions;

using Xunit;

namespace FluentSerializer.Json.Tests.Tests;

/// <summary>
/// Testing all nodes combined for the <see cref="JsonParser"/>
/// </summary>
public sealed class JsonParserTests
{
	[Theory,
		Trait("Category", "IntegrationTest"),	Trait("DataFormat", "JSON"),
		InlineData(true), InlineData(false)]
	public void AllObjects_EqualsExpectedInstanceTree(bool format)
	{
		// Arrange
		var expected = AllJsonNodes.GetInstance(format);
		var input = AllJsonNodes.GetJson(format);

		// Act
		var result = JsonParser.Parse(input);

		// Assert
		result.Should().BeEquatableTo(expected, format);
	}
}
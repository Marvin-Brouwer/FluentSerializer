using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using Xunit;

namespace FluentSerializer.Json.Tests.Tests;

/// <summary>
/// Testing all nodes combined for the <see cref="JsonBuilder"/>
/// </summary>
public sealed class JsonBuilderTests
{
	[Theory,
		Trait("Category", "IntegrationTest"),	Trait("DataFormat", "JSON"),
		InlineData(true), InlineData(false)]
	public void AllObjects_EqualsExpectedTextData(bool format)
	{
		// Arrange
		var expected = AllJsonNodes.GetJson(format);
		var input = AllJsonNodes.GetInstance(format);

		// Act
		var result = input.WriteTo(TestStringBuilderPool.Default, format);

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
}
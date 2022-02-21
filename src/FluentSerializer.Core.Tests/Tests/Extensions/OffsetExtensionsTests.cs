using FluentAssertions;
using FluentSerializer.Core.Extensions;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Extensions;

public sealed class OffsetExtensionsTests
{
	[Theory,
	 Trait("Category", "UnitTest"),
	 InlineData("", 69), InlineData("a", 69 + 1), InlineData("bb", 69 + 2)]
	public void AdjustForToken_String_AddsToOffset(string input, int expected)
	{
		// Arrange
		var offset = 69;

		// Act
		offset.AdjustForToken(input);

		// Assert
		offset.Should().Be(expected);
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void AdjustForToken_Char_AddsToOffset()
	{
		// Arrange
		var input = 'a';
		var expected = 69 + 1;

		var offset = 69;

		// Act
		offset.AdjustForToken(input);

		// Assert
		offset.Should().Be(expected);
	}

	[Theory,
	 Trait("Category", "UnitTest"),
	 InlineData(" a", 0 + 1), InlineData("  b", 0 + 2), InlineData("   c", 0 + 3)]
	public void AdjustForWhitespace_AddsToOffset(string input, int expected)
	{
		// Arrange
		var offset = 0;

		// Act
		offset.AdjustForWhiteSpace(input);

		// Assert
		offset.Should().Be(expected);
	}
}
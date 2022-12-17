using FluentAssertions;

using FluentSerializer.Core.Extensions;

using System;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Extensions;

public sealed class StringExtensionTests
{
	[Fact,
		Trait("Category", "UnitTest")]
	public void WithinCapacity_InCapacity_ReturnsTrue()
	{
		// Arrange
		var input = "four".AsSpan();
		var offset = 3;

		// Act
		var result = input.WithinCapacity(in offset);

		// Assert
		result.Should().BeTrue();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithinCapacity_NotInCapacity_ReturnsFalse()
	{
		// Arrange
		var input = "four".AsSpan();
		var offset = 4;

		// Act
		var result = input.WithinCapacity(in offset);

		// Assert
		result.Should().BeFalse();
	}

	[Theory,
		Trait("Category", "UnitTest"),
		InlineData('y', 0, true), InlineData('y', 2, false), InlineData('n', 0, false)]
	public void HasCharacterAtOffset_ReturnsBoolean(char input, int offset, bool expectation)
	{
		// Arrange
		var text = "yes".AsSpan();

		// Act
		var result = text.HasCharacterAtOffset(in offset, in input);

		// Assert
		result.Should().Be(expectation);
	}

	[Theory,
		Trait("Category", "UnitTest"),
		InlineData('y', 'e', 0, true), InlineData('y', 'e', 1, false), InlineData('y', 'n', 0, false)]
	public void HasCharactersAtOffset_ReturnsBoolean(char input1, char input2, int offset, bool expectation)
	{
		// Arrange
		var text = "yes".AsSpan();
		var inputString = string.Concat(input1, input2).AsSpan();

		// Act
		var resultCharacter = text.HasCharactersAtOffset(in offset, in input1, in input2);
		var resultString = text.HasCharactersAtOffset(in offset, in inputString);

		// Assert
		resultCharacter.Should().Be(expectation);
		resultString.Should().Be(expectation);
	}

	[Theory,
		Trait("Category", "UnitTest"),
		InlineData(0, false), InlineData(1, true), InlineData(3, true)]
	public void HasWhiteSpaceAtOffset_ReturnsBoolean(int offset, bool expectation)
	{
		// Arrange
		var text = "a b\n".AsSpan();

		// Act
		var result = text.HasWhitespaceAtOffset(in offset);

		// Assert
		result.Should().Be(expectation);
	}
}
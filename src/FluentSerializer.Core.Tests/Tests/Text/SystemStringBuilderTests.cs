using FluentAssertions;

using FluentSerializer.Core.TestUtils.Helpers;

using System;
using System.Text;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Text;

public sealed class SystemStringBuilderTests
{
	/// <summary>
	/// Because it's virtually impossible to fake/mock/spy sealed classes with no interfaces or virtual members
	/// We'll have to create an instance of this and verify it's values.
	/// </summary>
	private static StringBuilder StringBuilderSpy => new();

	[Fact,
		Trait("Category", "UnitTest")]
	public void TextWriter_AllMethods_ForwardToStringBuilderWithConfigValues()
	{
		// Arrange
		var sut = () => TestStringBuilderPool.CreateSingleInstance(StringBuilderSpy);

		// Act
		var sutSpy1 = sut().Append("Test");
		var sutSpy2 = sut().Append("Test".AsSpan());
		var sutSpy3 = sut().Append('t');
		var sutSpy4 = sut().Append('t', 9);
		var sutSpy5 = sut().AppendLineEnding();
		var sutSpy6 = sut().Append("SomeArbitraryValue").Clear();

		// Assert
		sutSpy1.ToString().Should().BeEquivalentTo("Test");
		sutSpy1.AsSpan().ToArray().Should().HaveCountGreaterThan(0);
		sutSpy2.ToString().Should().BeEquivalentTo("Test");
		sutSpy2.AsSpan().ToArray().Should().HaveCountGreaterThan(0);
		sutSpy3.ToString().Should().BeEquivalentTo("t");
		sutSpy3.AsSpan().ToArray().Should().HaveCountGreaterThan(0);
		sutSpy4.ToString().Should().BeEquivalentTo("ttttttttt");
		sutSpy4.AsSpan().ToArray().Should().HaveCountGreaterThan(0);
		sutSpy5.ToString().Should().BeEquivalentTo(sutSpy4.TextConfiguration.NewLine);
		sutSpy5.AsSpan().ToArray().Should().HaveCountGreaterThan(0);
		sutSpy6.ToString().Should().BeEmpty();
		sutSpy6.AsSpan().ToArray().Should().HaveCount(0);
	}
}
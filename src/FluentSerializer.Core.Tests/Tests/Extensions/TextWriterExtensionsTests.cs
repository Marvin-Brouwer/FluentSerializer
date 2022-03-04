using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using Moq;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Extensions;

public sealed class TextWriterExtensionsTests
{
	[Fact,
		Trait("Category", "UnitTest")]
	public void AppendNode()
	{
		// Arrange
		var sut = TestStringBuilderPool.CreateSingleInstance();

		var nodeMock = new Mock<IDataNode>();
		nodeMock
			.Setup(node => node.AppendTo(ref It.Ref<ITextWriter>.IsAny, true, 0, true))
			.Returns(sut);

		// Act
		var result = sut.AppendNode(nodeMock.Object, true, 0, true);

		// Assert
		result.Should().Be(sut);
		nodeMock
			.Verify(node => node.AppendTo(ref It.Ref<ITextWriter>.IsAny, true, 0, true));
	}

	[Theory,
		Trait("Category", "UnitTest"),
	 InlineData(true, "test\ntest"), InlineData(false, "testtest")]
	public void AppendOptionalNewline(bool format, string expected)
	{
		// Arrange
		var sut = TestStringBuilderPool.CreateSingleInstance();

		// Act
		var result = sut
			.Append("test")
			.AppendOptionalNewline(in format)
			.Append("test")
			.ToString();

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"),
	 InlineData(true, "test\ttest"), InlineData(false, "testtest")]
	public void AppendOptionalIndent(bool format, string expected)
	{
		// Arrange
		var sut = TestStringBuilderPool.CreateSingleInstance();

		// Act
		var result = sut
			.Append("test")
			.AppendOptionalIndent(1, in format)
			.Append("test")
			.ToString();

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ToString_Returns_NodeAppendTo()
	{
		// Arrange
		var sut = TestStringBuilderPool.CreateSingleInstance();

		var nodeMock = new Mock<IDataNode>();
		nodeMock
			.Setup(node => node.AppendTo(ref It.Ref<ITextWriter>.IsAny, true, 0, true))
			.Returns(sut);

		// Act
		var result = TextWriterExtensions.ToString(nodeMock.Object, new TestSerializerConfiguration());

		// Assert
		nodeMock
			.Verify(node => node.AppendTo(ref It.Ref<ITextWriter>.IsAny, true, 0, true));
	}

	private sealed class TestSerializerConfiguration : SerializerConfiguration { }
}
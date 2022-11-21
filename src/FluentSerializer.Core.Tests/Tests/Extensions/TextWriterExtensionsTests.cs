using FluentAssertions;

using FluentSerializer.Core.Comparing;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;

using Moq;

using System;

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
		var result = nodeMock.Object.ToString(new TestSerializerConfiguration());

		// Assert
		// It's empty because string appendage is mocked here.
		result.Should().BeEmpty();
		nodeMock
			.Verify(node => node.AppendTo(ref It.Ref<ITextWriter>.IsAny, true, 0, true));
	}

	/// <remarks>
	/// This just tests the proxying of this extension method.
	/// </remarks>
	[Theory,
		InlineData(true, 0, true), InlineData(false, 0, true), InlineData(true, 1, true), InlineData(false, 1, true),
		InlineData(true, 0, false), InlineData(false, 0, false), InlineData(true, 1, false), InlineData(false, 1, false),
		Trait("Category", "UnitTest")]
	public void AppendNode_VariableSettings_ReturnsExpectedText(bool format, int indent, bool writeNull)
	{
		// Arrange
		var node = new TestNode();
		var sut = TestStringBuilderPool.CreateSingleInstance();

		// Act
		var result = sut.AppendNode(node, format, indent, writeNull);

		// Assert
		result.ToString().Should()
			.Be($"{nameof(format)}={format};" +
			    $"{nameof(indent)}={(format ? indent : 0)};" +
			    $"{nameof(writeNull)}={writeNull};");
	}

	private sealed class TestSerializerConfiguration : SerializerConfiguration { }
	private sealed class TestNode : IDataNode
	{
		public string Name => nameof(TestNode);

		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true) =>
			stringBuilder
				.Append($"{nameof(format)}={format};")
				.Append($"{nameof(indent)}={indent};")
				.Append($"{nameof(writeNull)}={writeNull};");

		public bool Equals(IDataNode? other) => false;
		public HashCode GetNodeHash() => DataNodeComparer.Default.GetHashCodeForAll(Name);
	}
}
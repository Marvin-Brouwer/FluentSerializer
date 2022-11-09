using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlCommentTests
{
	private const string XmlCommentValue = "69\n69";

	private ITextWriter _textWriter;

	public XmlCommentTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeComment_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = Comment(string.Empty);
		var input = (string?)null!;

		// Act
		var result = new XmlComment(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeComment_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = Comment(string.Empty);
		var input = string.Empty;

		// Act
		var result = new XmlComment(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeComment_HasValue_ReturnsWithValue()
	{
		// Arrange
		var expected = Comment(XmlCommentValue);

		// Act
		var result = new XmlComment(XmlCommentValue);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}
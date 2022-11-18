using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlCommentTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Valid_ReturnsComment()
	{
		// Arrange
		var expected = Comment(XmlCommentValue);
		var input = $"<!-- {XmlCommentValue} -->";

		// Act
		var offset = 0;
		var result = new XmlComment(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML"),
		InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseXml_ValidWithWhiteSpace_ReturnsComment(string space)
	{
		// Arrange
		var expected = Comment(XmlCommentValue);
		var input = $"<!--{space}{XmlCommentValue}{space}-->{space}";

		// Act
		var offset = 0;
		var result = new XmlComment(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Empty_ReturnsComment()
	{
		// Arrange
		var expected = Comment(string.Empty);
		var input = "<!--  -->";

		// Act
		var offset = 0;
		var result = new XmlComment(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Incomplete_Throws()
	{
		// Arrange
		var input = $"<!-- {XmlCommentValue}";

		// Act
		var offset = 0;
		var result = () => new XmlComment(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}
}
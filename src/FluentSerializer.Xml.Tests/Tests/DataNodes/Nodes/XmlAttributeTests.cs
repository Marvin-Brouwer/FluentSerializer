using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlAttributeTests
{
	private const string XmlAttributeName = "attribute";
	private const string XmlAttributeValue = "69";

	private ITextWriter _textWriter;

	public XmlAttributeTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeAttribute_InvalidName_Throws()
	{
		// Arrange
		var name = "This name is: <invalid>";
		var input = (string?)null!;

		// Act
		var result = () => new XmlAttribute(in name, in input);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName(nameof(name));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeAttribute_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = Attribute(XmlAttributeName, string.Empty);
		var input = (string?)null!;

		// Act
		var result = new XmlAttribute(XmlAttributeName, in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeAttribute_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = Attribute(XmlAttributeName, string.Empty);
		var input = string.Empty;

		// Act
		var result = new XmlAttribute(XmlAttributeName, in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeAttribute_HasValue_ReturnsWithValue()
	{
		// Arrange
		var expected = Attribute(XmlAttributeName, XmlAttributeValue);

		// Act
		var result = new XmlAttribute(XmlAttributeName, XmlAttributeValue);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}
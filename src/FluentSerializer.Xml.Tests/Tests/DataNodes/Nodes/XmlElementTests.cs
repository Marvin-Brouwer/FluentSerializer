using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;

using System;
using System.Collections.Generic;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlElementTests
{
	private const string XmlElementName = "Element";
	private static readonly IXmlText XmlElementValue = Text("69");

	private ITextWriter _textWriter;

	public XmlElementTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeElement_InvalidName_Throws()
	{
		// Arrange
		var name = "This name is: <invalid>";
		var input = (IEnumerable<IXmlNode>?)null!;

		// Act
		var result = () => new XmlElement(in name, in input);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName(nameof(name));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeElement_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = Element(XmlElementName, Array.Empty<IXmlNode>());
		var input = (IEnumerable<IXmlNode>?)null;

		// Act
		var result = new XmlElement(XmlElementName, in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeElement_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = Element(XmlElementName, Array.Empty<IXmlNode>());
		var input = Array.Empty<IXmlNode>();

		// Act
		var result = new XmlElement(XmlElementName, input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeElement_HasChild_ReturnsWithChild()
	{
		// Arrange
		var expected = Element(XmlElementName, XmlElementValue);
		var input = new[] { XmlElementValue };

		// Act
		var result = new XmlElement(XmlElementName, input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}
using FluentAssertions;

using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;

using System;
using System.Collections.Generic;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlFragmentTests
{
	private static readonly IEnumerable<IXmlElement> XmlFragmentValue = new List<IXmlElement> {
		Element("node", Text("69")),
		Element("node", Text("420"))
	};

	private ITextWriter _textWriter;

	public XmlFragmentTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeFragment_ChildNull_ReturnsEmpty()
	{
		// Arrange
		var input = (IXmlNode)null!;

		// Act
		var result = new XmlFragment(in input);

		// Assert
		result.Children.Should().HaveCount(0);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeFragment_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var input = (IEnumerable<IXmlNode>)null!;

		// Act
		var result = new XmlFragment(in input);

		// Assert
		result.Children.Should().HaveCount(0);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeFragment_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var input = Array.Empty<IXmlNode>();

		// Act
		var result = new XmlFragment(input);

		// Assert
		result.Children.Should().HaveCount(0);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeFragment_HasValue_ReturnsWithValue()
	{
		// Arrange
		var expected = XmlFragmentValue;

		// Act
		var result = new XmlFragment(XmlFragmentValue);

		// Assert
		result.Children!.Should().BeEquivalentTo(expected);
	}
}
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlDocumentTests
{
	private static readonly IXmlElement XmlDocumentValue = Element("rootNode", Text("69"));

	private ITextWriter _textWriter;

	public XmlDocumentTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeDocument_InputNull_ReturnsEmpty()
	{
		// Arrange
		var input = (IXmlElement?)null!;

		// Act
		var result = new XmlDocument(in input);

		// Assert
		result.RootElement!.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void InitializeDocument_HasValue_ReturnsWithValue()
	{
		// Act
		var result = new XmlDocument(XmlDocumentValue);

		// Assert
		result.RootElement!.Should().BeEquatableTo(XmlDocumentValue);
	}
}
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;

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
}
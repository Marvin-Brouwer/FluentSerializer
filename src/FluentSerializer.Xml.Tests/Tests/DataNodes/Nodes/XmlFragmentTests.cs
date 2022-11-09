using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;

using System.Collections.Generic;

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
}
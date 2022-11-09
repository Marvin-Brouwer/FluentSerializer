using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;

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
}
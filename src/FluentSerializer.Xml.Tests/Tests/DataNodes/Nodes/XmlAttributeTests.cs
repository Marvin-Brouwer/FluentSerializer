using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;

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
}
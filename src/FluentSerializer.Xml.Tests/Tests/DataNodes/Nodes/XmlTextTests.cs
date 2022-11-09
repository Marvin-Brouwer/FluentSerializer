using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlTextTests
{
	private const string XmlTextValue = "69\n69";

	private ITextWriter _textWriter;

	public XmlTextTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}
}
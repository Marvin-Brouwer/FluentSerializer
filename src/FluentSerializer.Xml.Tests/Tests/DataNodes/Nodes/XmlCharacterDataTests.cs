using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlCharacterDataTests
{
	private const string XmlCharacterDataValue = "\n<p>\n\t69\n\t</p>\n";

	private ITextWriter _textWriter;

	public XmlCharacterDataTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}
}
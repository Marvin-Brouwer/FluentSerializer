using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlCommentTests
{
	private const string XmlCommentValue = "69\n69";

	private ITextWriter _textWriter;

	public XmlCommentTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}
}
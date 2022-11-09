using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonCommentMultiLineTests
{
	private ITextWriter _textWriter;

	public JsonCommentMultiLineTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}
}
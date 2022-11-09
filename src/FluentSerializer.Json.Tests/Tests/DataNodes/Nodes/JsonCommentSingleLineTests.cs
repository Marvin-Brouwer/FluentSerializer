using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonCommentSingleLineTests
{
	private ITextWriter _textWriter;

	public JsonCommentSingleLineTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}
}
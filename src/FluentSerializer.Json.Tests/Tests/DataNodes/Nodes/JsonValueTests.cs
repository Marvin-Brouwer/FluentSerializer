using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonValueTests
{
	private ITextWriter _textWriter;

	public JsonValueTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}
}
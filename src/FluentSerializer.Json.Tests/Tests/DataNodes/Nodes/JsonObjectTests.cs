using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonObjectTests
{
	private static readonly IJsonValue JsonValue = Value("69");
	private static readonly IJsonProperty JsonProperty = Property("value", in JsonValue);

	private ITextWriter _textWriter;

	public JsonObjectTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}
}
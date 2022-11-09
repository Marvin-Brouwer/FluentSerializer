using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;
public sealed partial class JsonPropertyTests
{
	private const string JsonPropertyName = "property";
	private static readonly IJsonValue JsonValue = Value("69");

	private ITextWriter _textWriter;

	public JsonPropertyTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}
}
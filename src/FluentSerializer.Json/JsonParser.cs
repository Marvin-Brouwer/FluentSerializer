using Ardalis.GuardClauses;
using FluentSerializer.Core.Text.Readers;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;

namespace FluentSerializer.Json;

/// <summary>
/// JSON parsing utility class
/// </summary>
public readonly struct JsonParser
{
	/// <summary>
	/// Parse a string value to a JSON object tree
	/// </summary>
	/// <param name="value">The JSON to parse</param>
	/// <remarks>
	/// This parser will not parse values to C# types, they will all be represented as string.
	/// </remarks>
	public static IJsonObject Parse(string value)
	{
		Guard.Against.NullOrWhiteSpace(value, nameof(value));

		// Todo objectpool
		return new JsonObject(new SimpleTextReader(value));
	}
}
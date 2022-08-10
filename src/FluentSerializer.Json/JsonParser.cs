using Ardalis.GuardClauses;

using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.DataNodes.Nodes;

using System;

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
	public static IJsonObject Parse(in string value)
	{
		Guard.Against.NullOrWhiteSpace(value, nameof(value));

		int offset = 0;
		return new JsonObject(value.AsSpan(), ref offset);
	}
}
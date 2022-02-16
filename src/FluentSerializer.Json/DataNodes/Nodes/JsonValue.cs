using System;
using System.Diagnostics;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonValue"/>
[DebuggerDisplay("{Value,nq}")]
public readonly partial struct JsonValue : IJsonValue
{
	private const string ValueName = "#value";

	/// <inheritdoc />
	public string Name => ValueName;
	/// <inheritdoc />
	public string? Value { get; }

	/// <inheritdoc />
	public bool HasValue => Value is not null && !Value.Equals(JsonCharacterConstants.NullValue, StringComparison.Ordinal);

	/// <inheritdoc cref="JsonBuilder.Value(in string?)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Value"/> method instead of this constructor</b>
	/// </remarks>
	public JsonValue(in string? value)
	{
		Value = value;
	}
}
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonValue"/>
[DebuggerDisplay("{Value,nq}")]
public readonly struct JsonValue : IJsonValue
{
	private static readonly int TypeHashCode = typeof(JsonValue).GetHashCode();

	private const string ValueName = "#value";
	public string Name => ValueName;
	public string? Value { get; }

	public bool HasValue => Value is not null && !Value.Equals(JsonCharacterConstants.NullValue, StringComparison.Ordinal);

	/// <inheritdoc cref="JsonBuilder.Value(string?)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Value"/> method instead of this constructor</b>
	/// </remarks>
	public JsonValue(string? value)
	{
		Value = value;
	}

	/// <inheritdoc cref="IJsonValue"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonValue(in ITokenReader reader)
	{
		var stringValue = false;

		var valueStartOffset = reader.Offset;
		var valueEndOffset = reader.Offset;

		while (reader.CanAdvance())
		{
			if (reader.HasCharacterAtOffset(JsonCharacterConstants.PropertyWrapCharacter) && stringValue) break; 
			if (reader.HasCharacterAtOffset(JsonCharacterConstants.DividerCharacter) && !stringValue) break;
			if (reader.HasCharacterAtOffset(JsonCharacterConstants.ObjectEndCharacter)) break;
			if (reader.HasCharacterAtOffset(JsonCharacterConstants.ArrayEndCharacter)) break;

			reader.Advance();
			valueEndOffset = reader.Offset;

			if (reader.HasCharacterAtOffset(JsonCharacterConstants.PropertyWrapCharacter)) stringValue = true; 
			if (!stringValue && reader.HasWhitespaceAtOffset()) break;
		}

		// Append a '"' if it started with a '"'
		if (stringValue) valueEndOffset++;
		Value = reader.ReadAbsolute(valueStartOffset..valueEndOffset).ToString().Trim();
	}

	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// JSON does not support empty property assignment or array members
		return stringBuilder.Append(Value ?? JsonCharacterConstants.NullValue);
	}

	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	#endregion
}
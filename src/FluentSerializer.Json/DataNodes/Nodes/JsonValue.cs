using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonValue"/>
[DebuggerDisplay("{Value,nq}")]
public readonly struct JsonValue : IJsonValue
{
	private static readonly int TypeHashCode = typeof(JsonValue).GetHashCode();

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

	/// <inheritdoc cref="IJsonValue"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonValue(in ReadOnlySpan<char> text, ref int offset)
	{
		var stringValue = false;

		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter) && stringValue) break; 
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter) && !stringValue) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectEndCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayEndCharacter)) break;

			offset++;
			valueEndOffset = offset;

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter)) stringValue = true; 
			if (!stringValue && text.HasWhitespaceAtOffset(in offset)) break;
		}

		// Append a '"' if it started with a '"'
		if (stringValue) valueEndOffset++;
		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}

	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// JSON does not support empty property assignment or array members
		return stringBuilder.Append(Value ?? JsonCharacterConstants.NullValue);
	}

	#region IEquatable

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	#endregion
}
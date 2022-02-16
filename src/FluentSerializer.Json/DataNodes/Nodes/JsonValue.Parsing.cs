using System;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonValue
{
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

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter)) stringValue = true;

			offset++;
			valueEndOffset = offset;

			if (!stringValue && text.HasWhitespaceAtOffset(in offset)) break;
		}

		// Append a '"' if it started with a '"'
		if (stringValue) valueEndOffset++;

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
		if (Value.Equals(JsonCharacterConstants.NullValue, StringComparison.OrdinalIgnoreCase))
			Value = null;
	}
}
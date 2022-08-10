using FluentSerializer.Core.Extensions;

using System;

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

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter) && stringValue) break; 
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter) && !stringValue) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectEndCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayEndCharacter)) break;

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter)) stringValue = true;

			offset++;

			if (!stringValue && text.HasWhitespaceAtOffset(in offset)) break;
		}

		// Append a '"' if it started with a '"'
		if (stringValue) offset.AdjustForToken(JsonCharacterConstants.PropertyWrapCharacter);

		Value = text[valueStartOffset..offset].ToString().Trim();
		if (Value.Equals(JsonCharacterConstants.NullValue, StringComparison.OrdinalIgnoreCase))
			Value = null;
	}
}
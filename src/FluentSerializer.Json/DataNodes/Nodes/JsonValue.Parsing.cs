using FluentSerializer.Core.Extensions;

using System;
using System.Runtime.CompilerServices;

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

		ParseJsonValue(in text, ref offset, ref stringValue);

		// Append a '"' if it started with a '"'
		if (stringValue) offset.AdjustForToken(JsonCharacterConstants.PropertyWrapCharacter);

		Value = valueStartOffset >= offset
			? string.Empty
			: text[valueStartOffset..offset].ToString().Trim();

		if (Value.Equals(JsonCharacterConstants.NullValue, StringComparison.OrdinalIgnoreCase))
			Value = null;
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void ParseJsonValue(in ReadOnlySpan<char> text, ref int offset, ref bool stringValue)
	{
		while (text.WithinCapacity(in offset))
		{
			if (EndingCharacterAtCurrentOffset(in text, in offset, in stringValue)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter)) stringValue = true;

			offset.Increment();
			if (!text.WithinCapacity(in offset)) break;

			if (!stringValue && text.HasWhitespaceAtOffset(in offset)) break;
		}
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static bool EndingCharacterAtCurrentOffset(in ReadOnlySpan<char> text, in int offset, in bool stringValue)
	{
		if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter) && stringValue) return true;
		if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter) && !stringValue) return true;
		if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectEndCharacter)) return true;
		if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayEndCharacter)) return true;

		return false;
	}
}
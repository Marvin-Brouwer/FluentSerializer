using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonProperty
{
	/// <inheritdoc cref="IJsonObject"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonProperty(in ReadOnlySpan<char> text, ref int offset)
	{
		HasValue = false;

		offset.AdjustForWhiteSpace(in text);
		if (!text.WithinCapacity(in offset))
		{
			_children = Array.Empty<IJsonNode>();
			Name = JsonCharacterConstants.UnknownPropertyName;
			return;
		}

		offset.AdjustForToken(JsonCharacterConstants.PropertyWrapCharacter);

		var nameStartOffset = offset;
		var nameEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter)) break;

			offset++;
			nameEndOffset = offset;
		}

		Name = text[nameStartOffset..nameEndOffset].ToString().Trim();
		_children = Array.Empty<IJsonNode>();

		while (text.WithinCapacity(in offset))
		{
			offset++;

			// Just pretend it's null if no value has been provided
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
			{
				offset++;
				return;
			}

			offset.AdjustForWhiteSpace(in text);

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyAssignmentCharacter)) break;
		}

		ParseJsonProperty(in text, ref offset, ref _children, out bool hasValue);
		HasValue = hasValue;
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void ParseJsonProperty(in ReadOnlySpan<char> text, ref int offset, ref IReadOnlyList<IJsonNode> children, out bool hasValue)
	{
		while (text.WithinCapacity(in offset))
		{
			offset++;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectStartCharacter))
			{
				children = new IJsonNode[]{
					new JsonObject(in text, ref offset)
				};
				hasValue = true;
				return;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayStartCharacter))
			{
				children = new IJsonNode[] {
					new JsonArray(in text, ref offset)
				};
				hasValue = true;
				return;
			}

			if (!text.HasWhitespaceAtOffset(in offset)) break;
			// Just pretend it's null if no value has been provided
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
			{
				children = Array.Empty<IJsonNode>();
				offset++;
				hasValue = false;
				return;
			}
		}

		var jsonValue = new JsonValue(in text, ref offset);
		children = new IJsonNode[]
		{
			jsonValue
		};
		hasValue = jsonValue.HasValue;
	}
}
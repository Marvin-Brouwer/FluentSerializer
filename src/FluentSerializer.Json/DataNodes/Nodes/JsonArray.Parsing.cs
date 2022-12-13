using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonArray
{
	/// <inheritdoc cref="IJsonArray"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonArray(in ReadOnlySpan<char> text, ref int offset)
	{
		var children = new List<IJsonNode>();
		_lastNonCommentChildIndex = null;
		_children = Array.Empty<IJsonNode>();
		var currentChildIndex = 0;

		offset.AdjustForWhiteSpace(in text);
		if (!text.WithinCapacity(in offset)) return; 

		ParseJsonArray(in text, ref offset, ref children, ref currentChildIndex, ref _lastNonCommentChildIndex);
		_children = children.AsReadOnly();
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void ParseJsonArray(in ReadOnlySpan<char> text, ref int offset,
		ref List<IJsonNode> children, ref int currentChildIndex, ref int? lastNonCommentChildIndex)
	{
		lastNonCommentChildIndex = null;

		offset.AdjustForToken(JsonCharacterConstants.ArrayStartCharacter);
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
				offset.AdjustForToken(JsonCharacterConstants.DividerCharacter);

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectStartCharacter))
			{
				children.Add(new JsonObject(in text, ref offset));
				lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayStartCharacter))
			{
				children.Add(new JsonArray(in text, ref offset));
				lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayEndCharacter)) break;

			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.SingleLineCommentMarker))
			{
				children.Add(new JsonCommentSingleLine(in text, ref offset));

				currentChildIndex++;
				continue;
			}
			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.MultiLineCommentStart))
			{
				children.Add(new JsonCommentMultiLine(in text, ref offset));

				currentChildIndex++;
				continue;
			}
			if (!text.HasWhitespaceAtOffset(in offset))
			{
				children.Add(new JsonValue(in text, ref offset));
				lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}

			offset.Increment();
		}
		if (text.WithinCapacity(in offset))
			offset.AdjustForToken(JsonCharacterConstants.ArrayEndCharacter);
	}
}
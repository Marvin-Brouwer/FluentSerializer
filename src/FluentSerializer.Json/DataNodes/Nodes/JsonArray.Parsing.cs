using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;

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
		var currentChildIndex = 0;

		offset.AdjustForToken(JsonCharacterConstants.ArrayStartCharacter);
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
				offset.AdjustForToken(JsonCharacterConstants.DividerCharacter);

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectStartCharacter))
			{
				children.Add(new JsonObject(in text, ref offset));
				_lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayStartCharacter))
			{
				children.Add(new JsonArray(in text, ref offset));
				_lastNonCommentChildIndex = currentChildIndex;

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
				_lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}
			offset++;
		}
		offset.AdjustForToken(JsonCharacterConstants.ArrayEndCharacter);

		_children = children.AsReadOnly();
	}
}
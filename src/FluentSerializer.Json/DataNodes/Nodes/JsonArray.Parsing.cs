using System;
using System.Collections.Generic;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonArray
{
	/// <inheritdoc cref="IJsonArray"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonArray(in ReadOnlySpan<char> text, ref int offset)
	{
		_children = new List<IJsonNode>();
		_lastNonCommentChildIndex = null;
		var currentChildIndex = 0;

		offset.AdjustForToken(JsonCharacterConstants.ArrayStartCharacter);
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectStartCharacter))
			{
				_children.Add(new JsonObject(in text, ref offset));
				_lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayStartCharacter))
			{
				_children.Add(new JsonArray(in text, ref offset));
				_lastNonCommentChildIndex = currentChildIndex;

				currentChildIndex++;
				continue;
			}

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectEndCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayEndCharacter)) break;

			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.SingleLineCommentMarker))
			{
				_children.Add(new JsonCommentSingleLine(in text, ref offset));

				currentChildIndex++;
				continue;
			}
			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.MultiLineCommentStart))
			{
				_children.Add(new JsonCommentMultiLine(in text, ref offset));

				currentChildIndex++;
				continue;
			}
			offset++;
		}
		offset.AdjustForToken(JsonCharacterConstants.ArrayEndCharacter);
	}
}
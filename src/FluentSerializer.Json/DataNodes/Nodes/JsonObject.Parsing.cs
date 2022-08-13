using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonObject
{
	/// <inheritdoc cref="IJsonObject"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonObject(in ReadOnlySpan<char> text, ref int offset)
	{
		_lastPropertyIndex = null;
		var children = new List<IJsonNode>();
		var currentPropertyIndex = 0;

		offset.AdjustForToken(JsonCharacterConstants.ObjectStartCharacter);
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectEndCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayEndCharacter)) break;

			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.SingleLineCommentMarker))
			{
				children.Add(new JsonCommentSingleLine(in text, ref offset));
				currentPropertyIndex++;
				continue;
			}
			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.MultiLineCommentStart))
			{
				children.Add(new JsonCommentMultiLine(in text, ref offset));
				currentPropertyIndex++;
				continue;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyWrapCharacter))
			{
				var jsonProperty = new JsonProperty(in text, ref offset);
				children.Add(jsonProperty);
				_lastPropertyIndex = currentPropertyIndex;

				currentPropertyIndex++;
				continue;
			}

			offset++;
		}
		offset.AdjustForToken(JsonCharacterConstants.ObjectEndCharacter);
		_children = children.AsReadOnly();
	}
}
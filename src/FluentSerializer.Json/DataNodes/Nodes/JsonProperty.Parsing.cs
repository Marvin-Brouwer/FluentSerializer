using FluentSerializer.Core.Extensions;

using System;

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

		while (text.WithinCapacity(in offset))
		{
			offset++;

			// Just pretend it's null if no value has been provided
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
			{
				_children = Array.Empty<IJsonNode>();
				offset++;
				return;
			}

			offset.AdjustForWhiteSpace(in text);

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.PropertyAssignmentCharacter)) break;
		}

		while (text.WithinCapacity(in offset))
		{
			offset++;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectStartCharacter))
			{
				_children = new IJsonNode[]{
					new JsonObject(in text, ref offset)
				};
				HasValue = true;
				return;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayStartCharacter))
			{
				_children = new IJsonNode[] {
					new JsonArray(in text, ref offset)
				};
				HasValue = true;
				return;
			}

			if (!text.HasWhitespaceAtOffset(in offset)) break;
			// Just pretend it's null if no value has been provided
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
			{
				_children = Array.Empty<IJsonNode>();
				offset++;
				return;
			}
		}

		var jsonValue = new JsonValue(in text, ref offset);
		_children = new IJsonNode[]
		{
			jsonValue
		};
		HasValue = jsonValue.HasValue;
	}
}
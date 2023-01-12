using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;

using System;
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
		offset.AdjustForWhiteSpace(in text);
		if (!text.WithinCapacity(in offset))
		{
			_children = SingleItemCollection.Empty<IJsonNode>();
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
		_children = SingleItemCollection.Empty<IJsonNode>();

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

		ParseJsonProperty(in text, ref offset, ref _children);
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void ParseJsonProperty(in ReadOnlySpan<char> text, ref int offset, ref ISingleItemCollection<IJsonNode> children)
	{
		while (text.WithinCapacity(in offset))
		{
			offset++;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ObjectStartCharacter))
			{
				children = SingleItemCollection.ForItem<IJsonNode>(
					new JsonObject(in text, ref offset)
				);
				return;
			}
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.ArrayStartCharacter))
			{
				children = SingleItemCollection.ForItem<IJsonNode>(
					new JsonArray(in text, ref offset)
				);
				return;
			}

			if (!text.HasWhitespaceAtOffset(in offset)) break;
			// Just pretend it's null if no value has been provided
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.DividerCharacter))
			{
				children = SingleItemCollection.Empty<IJsonNode>();
				offset++;
				return;
			}
		}

		var jsonValue = new JsonValue(in text, ref offset);
		children = SingleItemCollection.ForItem<IJsonNode>(jsonValue);
	}
}
using FluentSerializer.Core.Extensions;

using System;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonCommentSingleLine
{
	/// <inheritdoc cref="IJsonComment"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonCommentSingleLine(in ReadOnlySpan<char> text, ref int offset)
	{
		offset.AdjustForToken(JsonCharacterConstants.SingleLineCommentMarker);

		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			offset++;
			valueEndOffset = offset;

			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.LineReturnCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, JsonCharacterConstants.NewLineCharacter)) break;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}
}
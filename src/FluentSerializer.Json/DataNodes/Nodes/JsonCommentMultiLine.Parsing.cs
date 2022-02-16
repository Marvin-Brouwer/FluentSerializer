using System;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonCommentMultiLine
{
	/// <inheritdoc cref="IJsonComment"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public JsonCommentMultiLine(in ReadOnlySpan<char> text, ref int offset)
	{
		offset.AdjustForToken(JsonCharacterConstants.MultiLineCommentStart);

		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			valueEndOffset = offset;

			if (text.HasCharactersAtOffset(in offset, JsonCharacterConstants.MultiLineCommentEnd)) 
			{
				offset.AdjustForToken(JsonCharacterConstants.MultiLineCommentStart);
				break;
			}

			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}
}
using System;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlComment
{
	/// <inheritdoc cref="IXmlComment"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlComment(in ReadOnlySpan<char> text, ref int offset)
	{
		offset.AdjustForToken(XmlCharacterConstants.CommentStart);

		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (offset < text.Length)
		{
			valueEndOffset = offset;
			if (text.HasCharactersAtOffset(in offset, XmlCharacterConstants.CommentEnd))
			{
				offset.AdjustForToken(XmlCharacterConstants.CommentEnd);
				break;
			}
                
			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}
}
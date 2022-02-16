using System;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlCharacterData
{
	/// <inheritdoc cref="IXmlCharacterData"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlCharacterData(in ReadOnlySpan<char> text, ref int offset)
	{
		offset.AdjustForToken(XmlCharacterConstants.CharacterDataStart);

		var valueStartOffset = offset;
		var valueEndOffset = offset;
            
		while (text.WithinCapacity(in offset))
		{
			valueEndOffset = offset;
			if (text.HasCharactersAtOffset(in offset, XmlCharacterConstants.CharacterDataEnd))
			{
				offset.AdjustForToken(XmlCharacterConstants.CharacterDataEnd);
				break;
			}
                
			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString();
	}
}
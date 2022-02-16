using System;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Xml.DataNodes.Nodes;


public readonly partial struct XmlText
{
	/// <inheritdoc cref="IXmlText"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlText(in ReadOnlySpan<char> text, ref int offset)
	{
		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			valueEndOffset = offset;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;

			offset++;
		}

		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
	}
}
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlElement
{
	/// <inheritdoc cref="IXmlElement"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlElement(in ReadOnlySpan<char> text, ref int offset)
	{
		// If we encounter a declaration just ignore it, if this becomes a problem we can start the parse in
		// the document. For now this is fine.
		if (text.HasCharactersAtOffset(in offset, XmlCharacterConstants.DeclarationStart))
		{
			while (text.WithinCapacity(in offset) && !text.HasCharactersAtOffset(in offset, XmlCharacterConstants.DeclarationEnd))
			{
				offset++;
			}
		}
		// Ignore whitespace before tag start
		while (text.WithinCapacity(in offset) && !text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter))
		{
			offset++;
		}
		offset.AdjustForToken(XmlCharacterConstants.TagStartCharacter);
		offset.AdjustForWhiteSpace(in text);

		var attributes = new List<IXmlAttribute>();
		var children = new List<IXmlNode>();

		var nameStartOffset = offset;
		var nameEndOffset = offset;

		var elementClosed = false;
		var tagFinished = false;
		var nameFinished = false;

		while (text.WithinCapacity(in offset))
		{
			nameEndOffset = offset;

			if (text.HasCharactersAtOffset(in offset,
			    XmlCharacterConstants.TagTerminationCharacter,
			    XmlCharacterConstants.TagEndCharacter))
			{
				elementClosed = true;
				offset.AdjustForToken(XmlCharacterConstants.TagTerminationCharacter);
				tagFinished = true;
				offset.AdjustForToken(XmlCharacterConstants.TagEndCharacter);
				break;
			}
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagEndCharacter))
			{
				tagFinished = true;
				offset.AdjustForToken(XmlCharacterConstants.TagEndCharacter);
				break;
			}

			if (nameFinished) break;

			offset++;
			if (text.HasWhitespaceAtOffset(in offset))
			{
				nameFinished = true;
			}
		}
            
		Name = text[nameStartOffset..nameEndOffset].ToString().Trim();

		while (!tagFinished && text.WithinCapacity(in offset))
		{
			if (text.HasCharactersAtOffset(in offset,
				    XmlCharacterConstants.TagTerminationCharacter,
				    XmlCharacterConstants.TagEndCharacter))
			{
				offset.AdjustForToken(XmlCharacterConstants.TagTerminationCharacter);
				elementClosed = true;
				offset.AdjustForToken(XmlCharacterConstants.TagEndCharacter);
				break;
			}
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagEndCharacter))
			{
				offset.AdjustForToken(XmlCharacterConstants.TagEndCharacter);
				break;
			}

			if (text.HasWhitespaceAtOffset(in offset))
			{
				offset++;
				continue;
			}

			attributes.Add(new XmlAttribute(in text, ref offset));
		}

		if (elementClosed)
		{
			_attributes = attributes;
			_children = children;
			return;
		}

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharactersAtOffset(in offset,
				    XmlCharacterConstants.TagStartCharacter,
				    XmlCharacterConstants.TagTerminationCharacter))
			{
				offset.AdjustForToken(XmlCharacterConstants.TagStartCharacter);
				offset.AdjustForToken(XmlCharacterConstants.TagTerminationCharacter);
				break;
			}
			
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter))
			{
				if (text.HasCharactersAtOffset(in offset, XmlCharacterConstants.CommentStart))
				{
					children.Add(new XmlComment(in text, ref offset));
					continue;
				}
				if (text.HasCharactersAtOffset(in offset, XmlCharacterConstants.CharacterDataStart))
				{
					children.Add(new XmlCharacterData(in text, ref offset));
					continue;
				}

				children.Add(new XmlElement(in text, ref offset));
				continue;
			}
			if (text.HasWhitespaceAtOffset(in offset))
			{
				offset++;
				continue;
			}

			children.Add(new XmlText(in text, ref offset));
		}

		// Walk to the end of the current closing tag
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagEndCharacter)) break;
			offset++;
		}
		offset.AdjustForToken(XmlCharacterConstants.TagEndCharacter);

		_attributes = attributes;
		_children = children;
	}
}
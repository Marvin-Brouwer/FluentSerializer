using FluentSerializer.Core.Extensions;
using System;
using System.IO;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlAttribute
{
	/// <inheritdoc cref="IXmlAttribute"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlAttribute(in ReadOnlySpan<char> text, ref int offset)
	{
		var nameStartOffset = offset;
		var nameEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagEndCharacter)) break;
			nameEndOffset = offset;

			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyAssignmentCharacter)) break;
			offset++;
		}

		Name = text[nameStartOffset..nameEndOffset].ToString().Trim();

		offset.AdjustForToken(XmlCharacterConstants.PropertyAssignmentCharacter);
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyWrapCharacter))
			{
				offset.AdjustForToken(XmlCharacterConstants.PropertyWrapCharacter);
				break;
			}
			if (!text.HasWhitespaceAtOffset(in offset)) break;
			offset++;
		}
            
		var valueStartOffset = offset;
		var valueEndOffset = offset;

		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;
			valueEndOffset = offset;

			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyWrapCharacter)) break;
			offset++;
		}
            
		Value = text[valueStartOffset..valueEndOffset].ToString().Trim();
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyWrapCharacter))
			{
				offset.AdjustForToken(XmlCharacterConstants.PropertyWrapCharacter);
				break;
			}
			if (text.HasWhitespaceAtOffset(in offset)) continue;
			throw new InvalidDataException("Attribute incorrectly terminated");
		}
	}
}
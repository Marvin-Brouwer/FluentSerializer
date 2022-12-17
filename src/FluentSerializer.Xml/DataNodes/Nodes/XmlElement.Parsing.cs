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
		_attributes = Array.Empty<IXmlAttribute>();
		_children = Array.Empty<IXmlNode>();

		MoveToElementStart(in text, ref offset);
		offset.AdjustForWhiteSpace(in text);

		Name = string.Empty;
		Name = ParseElementName(in text, ref offset, out var elementClosed, out var tagFinished);

		var attributes = new List<IXmlAttribute>();
		ParseElementAttributes(in text, ref offset, ref attributes, ref elementClosed, ref tagFinished);

		if (elementClosed)
		{
			_attributes = attributes.AsReadOnly();
			return;
		}

		var children = new List<IXmlNode>();
		ParseElementChildren(in text, ref offset, ref children);

		// Walk to the end of the current closing tag
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagEndCharacter)) break;
			offset++;
		}
		offset.AdjustForToken(XmlCharacterConstants.TagEndCharacter);

		_attributes = attributes.AsReadOnly();
		_children = children.AsReadOnly();
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void MoveToElementStart(in ReadOnlySpan<char> text, ref int offset)
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
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static string ParseElementName(
		in ReadOnlySpan<char> text, ref int offset, out bool elementClosed, out bool tagFinished)
	{
		tagFinished = false;
		elementClosed = false;

		var nameStartOffset = offset;
		var nameEndOffset = offset;
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

			offset.Increment();
			if (text.HasWhitespaceAtOffset(in offset))
			{
				nameFinished = true;
			}
		}

		return text[nameStartOffset..nameEndOffset].ToString().Trim();
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void ParseElementAttributes(
		in ReadOnlySpan<char> text, ref int offset, ref List<IXmlAttribute> attributes,
		ref bool elementClosed, ref bool tagFinished)
	{
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
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void ParseElementChildren(in ReadOnlySpan<char> text, ref int offset, ref List<IXmlNode> children)
	{
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
	}
}
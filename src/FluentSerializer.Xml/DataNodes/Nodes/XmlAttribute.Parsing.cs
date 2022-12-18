using FluentSerializer.Core.Extensions;

using System;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlAttribute
{
	/// <inheritdoc cref="IXmlAttribute"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
	/// </remarks>
	public XmlAttribute(in ReadOnlySpan<char> text, ref int offset)
	{
		Name = string.Empty;
		Name = ParseAttributeName(in text, ref offset);

		MoveToAttributeValue(in text, ref offset);

		Value = string.Empty;
		Value = ParseAttributeValue(in text, ref offset);

		MoveToElementOrAttributeEnd(in text, ref offset);
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static string ParseAttributeName(in ReadOnlySpan<char> text, ref int offset)
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

		return text[nameStartOffset..nameEndOffset].ToString().Trim();
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void MoveToAttributeValue(in ReadOnlySpan<char> text, ref int offset)
	{
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
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static string ParseAttributeValue(in ReadOnlySpan<char> text, ref int offset)
	{
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

		return text[valueStartOffset..valueEndOffset].ToString().Trim();
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void MoveToElementOrAttributeEnd(in ReadOnlySpan<char> text, ref int offset)
	{
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagTerminationCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter)) break;
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.PropertyWrapCharacter))
			{
				offset.AdjustForToken(XmlCharacterConstants.PropertyWrapCharacter);
				break;
			}
		}
	}
}
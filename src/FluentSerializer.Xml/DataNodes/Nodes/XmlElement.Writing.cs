using Ardalis.GuardClauses;

using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.Configuration;

using Microsoft.Extensions.ObjectPool;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlElement
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0) =>
		DataNodeExtensions.WriteTo(this, in stringBuilders, in format, in writeNull, in indent);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		const char spacer = ' ';

		Guard.Against.NullOrWhiteSpace(Name, message: "The element was is an illegal state, it contains no Name"
#if NETSTANDARD
			, parameterName: nameof(Name)
#endif
		);

		var childCount = _attributes.Count + _children.Count;
		var childIndent = format ? indent + 1 : 0;

		if (!writeNull && childCount == 0) return stringBuilder;
		if (!writeNull && _children[0] is IXmlText text
			&& string.IsNullOrEmpty(text.Value)) return stringBuilder;

		AppendElementStart(ref stringBuilder, Name);
		if (childCount == 0) return stringBuilder
			.Append(spacer)
			.Append(XmlCharacterConstants.TagTerminationCharacter)
			.Append(XmlCharacterConstants.TagEndCharacter);

		AppendElementAttributes(ref stringBuilder, in _attributes, in format, in indent, in writeNull, in childIndent);
		stringBuilder
			.Append(XmlCharacterConstants.TagEndCharacter);

		AppendElementChildren(ref stringBuilder, in _children, in format, in writeNull, in childIndent, out var textOnly);

		AppendElementEnd(ref stringBuilder, Name, in format, in indent, in textOnly);

		return stringBuilder;
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void AppendElementStart(ref ITextWriter stringBuilder, in string name)
	{
		stringBuilder
			.Append(XmlCharacterConstants.TagStartCharacter)
			.Append(name);
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void AppendElementAttributes(
		ref ITextWriter stringBuilder, in IReadOnlyList<IXmlAttribute> attributes,
		in bool format, in int indent, in bool writeNull, in int childIndent)
	{
		const char spacer = ' ';

		foreach (var attribute in attributes)
		{
			stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in indent, in format)
				.Append(spacer)
				.AppendNode(attribute, in format, in childIndent, in writeNull);
		}
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void AppendElementChildren(
		ref ITextWriter stringBuilder, in IReadOnlyList<IXmlNode> children,
		in bool format, in bool writeNull, in int childIndent, out bool textOnly)
	{
		textOnly = true;

		// Technically this object can have multiple text nodes, only the first folling the start or a node needs indentation
		var firstTextNode = true;

		foreach (var child in children)
		{
			if (child is IXmlElement childElement)
			{
				AppendChildElement(ref stringBuilder, in format, in writeNull, in childIndent, out textOnly, out firstTextNode, in childElement);

				continue;
			}
			if (child is IXmlText textNode)
			{
				AppendTextNode(ref stringBuilder, in format, in writeNull, in childIndent, ref firstTextNode, in textOnly, in textNode);

				continue;
			}
			if (child is IXmlComment commentNode)
			{
				AppendCommentNode(ref stringBuilder, in format, in writeNull, in childIndent, in commentNode);

				continue;
			}
			if (child is IXmlCharacterData cDataNode)
			{
				AppendCharacterDataNode(ref stringBuilder, in format, in writeNull, in childIndent, in cDataNode);
			}
		}
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void AppendChildElement(
		ref ITextWriter stringBuilder, in bool format, in bool writeNull, in int childIndent,
		out bool textOnly, out bool firstTextNode, in IXmlElement childElement)
	{
		textOnly = false;
		firstTextNode = true;

		stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(in childIndent, in format);

		stringBuilder
			.AppendNode(childElement, in format, in childIndent, in writeNull);
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void AppendTextNode(
		ref ITextWriter stringBuilder, in bool format, in bool writeNull, in int childIndent,
		ref bool firstTextNode, in bool textOnly, in IXmlText textNode)
	{
		// Technically this object can have multiple text nodes, only the first folling the start or a node needs indentation
		if (firstTextNode)
		{
			firstTextNode = false;
			if (!textOnly) stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in childIndent, in format);

			stringBuilder
				.AppendNode(textNode, true, in childIndent, in writeNull);

			return;
		}

		stringBuilder
			.AppendNode(textNode, false, in childIndent, in writeNull);
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void AppendCommentNode(
		ref ITextWriter stringBuilder, in bool format, in bool writeNull, in int childIndent, in IXmlComment commentNode)
	{
		stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(in childIndent, in format);

		stringBuilder
			.AppendNode(commentNode, true, in childIndent, in writeNull);
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void AppendCharacterDataNode(
		ref ITextWriter stringBuilder, in bool format, in bool writeNull, in int childIndent, in IXmlCharacterData cDataNode)
	{
		stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(in childIndent, in format);

		stringBuilder
			.AppendNode(cDataNode, true, in childIndent, in writeNull);
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void AppendElementEnd(ref ITextWriter stringBuilder, in string name, in bool format, in int indent, in bool textOnly)
	{
		if (!textOnly) stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(in indent, in format);

		stringBuilder
			.Append(XmlCharacterConstants.TagStartCharacter)
			.Append(XmlCharacterConstants.TagTerminationCharacter)
			.Append(name)
			.Append(XmlCharacterConstants.TagEndCharacter);
	}
}
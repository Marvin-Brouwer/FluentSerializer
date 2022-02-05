using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlElement"/>
[DebuggerDisplay("<{Name,nq} />")]
public readonly struct XmlElement : IXmlElement
{
	private static readonly int TypeHashCode = typeof(XmlElement).GetHashCode();

	/// <inheritdoc />
	public string Name { get; }

	private readonly List<IXmlNode> _children;
	private readonly List<IXmlAttribute> _attributes;

	/// <inheritdoc />
	public IReadOnlyList<IXmlNode> Children
	{
		get
		{
			var childList = new List<IXmlNode>(_attributes);
			childList.AddRange(_children);
			return childList;
		}
	}
	/// <inheritdoc />
	public IXmlAttribute? GetChildAttribute(string name)
	{
		Guard.Against.NullOrWhiteSpace(name, nameof(name));

		foreach (var attribute in _attributes)
		{
			if (string.IsNullOrEmpty(name) || attribute.Name.Equals(name, StringComparison.Ordinal))
				return attribute;
		}

		return default;
	}

	/// <inheritdoc />
	public IEnumerable<IXmlElement> GetChildElements(string? name = null)
	{
		foreach (var child in _children)
		{
			if (child is not IXmlElement element) continue;
			if (string.IsNullOrEmpty(name) || element.Name.Equals(name, StringComparison.Ordinal))
				yield return element;
		}
	}
	/// <inheritdoc />
	public IXmlElement? GetChildElement(string name)
	{
		Guard.Against.NullOrWhiteSpace(name, nameof(name));

		foreach (var child in _children)
		{
			if (child is not IXmlElement element) continue;
			if (string.IsNullOrEmpty(name) || element.Name.Equals(name, StringComparison.Ordinal))
				return element;
		}

		return default;
	}

	/// <inheritdoc />
	public string? GetTextValue()
	{
		string? returnValue = null;
		foreach (var child in _children)
		{
			if (child is not IXmlText element) continue;
			if (string.IsNullOrEmpty(element.Value)) continue;

			returnValue ??= string.Empty;
			returnValue += element.Value;
		}

		return returnValue;
	}

	/// <inheritdoc cref="XmlBuilder.Element(string, IXmlNode[])"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Element(string, IXmlNode[])"/> method instead of this constructor</b>
	/// </remarks>
	public XmlElement(string name) : this(name, new List<IXmlNode>(0)) { }

	/// <inheritdoc cref="XmlBuilder.Element(string, IXmlNode[])"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Element(string, IXmlNode[])"/> method instead of this constructor</b>
	/// </remarks>
	public XmlElement(string name, params IXmlNode[] childNodes) : this(name, childNodes.AsEnumerable()) { }

	/// <inheritdoc cref="XmlBuilder.Element(string, IEnumerable{IXmlNode})"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Element(string, IXmlNode[])"/> method instead of this constructor</b>
	/// </remarks>
	public XmlElement(string name, IEnumerable<IXmlNode> childNodes)
	{
		Guard.Against.InvalidName(name);

		Name = name;
		_attributes = new List<IXmlAttribute>();
		_children = new List<IXmlNode>();

		foreach (var node in childNodes)
		{
			if (node is null) continue;
			if (node is XmlAttribute attribute) _attributes.Add(attribute);
			else _children.Add(node);
		}
	}

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
		// Ignore whitespace
		while (text.WithinCapacity(in offset) && !text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagStartCharacter))
		{
			offset++;
		}

		offset.AdjustForToken(XmlCharacterConstants.TagStartCharacter);

		_attributes = new List<IXmlAttribute>();
		_children = new List<IXmlNode>();

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

			_attributes.Add(new XmlAttribute(in text, ref offset));
		}

		if (elementClosed) return;

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
					_children.Add(new XmlComment(in text, ref offset));
					continue;
				}
				if (text.HasCharactersAtOffset(in offset, XmlCharacterConstants.CharacterDataStart))
				{
					_children.Add(new XmlCharacterData(in text, ref offset));
					continue;
				}

				_children.Add(new XmlElement(in text, ref offset));
				continue;
			}
			if (text.HasWhitespaceAtOffset(in offset))
			{
				offset++;
				continue;
			}

			_children.Add(new XmlText(in text, ref offset));
		}

		// Walk to the end of the current closing tag
		while (text.WithinCapacity(in offset))
		{
			if (text.HasCharacterAtOffset(in offset, XmlCharacterConstants.TagEndCharacter)) break;
			offset++;
		}
		offset.AdjustForToken(XmlCharacterConstants.TagEndCharacter);
	}

	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		const char spacer = ' ';

		var children = Children;
		var childIndent = format ? indent + 1 : 0;

		if (!writeNull && !children.Any()) return stringBuilder;

		stringBuilder
			.Append(XmlCharacterConstants.TagStartCharacter)
			.Append(Name);

		if (!children.Any()) return stringBuilder
			.Append(spacer)
			.Append(XmlCharacterConstants.TagTerminationCharacter)
			.Append(XmlCharacterConstants.TagEndCharacter);

		foreach (var attribute in _attributes)
		{
			stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in indent, in format)
				.Append(spacer)
				.AppendNode(attribute, in format, in childIndent, in writeNull);
		}
		stringBuilder
			.Append(XmlCharacterConstants.TagEndCharacter);

		// Technically this object can have multiple text nodes, only the first needs indentation
		var textOnly = true;
		var firstTextNode = true;
		foreach (var child in _children)
		{
			if (child is IXmlElement childElement) {
				textOnly = false;
				firstTextNode = true;

				stringBuilder
					.AppendOptionalNewline(in format)
					.AppendOptionalIndent(in childIndent, in format);

				stringBuilder
					.AppendNode(childElement, in format, in childIndent, in writeNull);

				continue;
			}
			if (child is IXmlText textNode)
			{
				if (firstTextNode)
				{
					firstTextNode = false;
					if (!textOnly) stringBuilder
						.AppendOptionalNewline(in format)
						.AppendOptionalIndent(in childIndent, in format);

					stringBuilder
						.AppendNode(textNode, true, in childIndent, in writeNull);

					continue;
				}

				stringBuilder
					.AppendNode(textNode, false, in childIndent, in writeNull);

				continue;
			}
			if (child is IXmlComment commentNode)
			{
				stringBuilder
					.AppendOptionalNewline(in format)
					.AppendOptionalIndent(in childIndent, in format);

				stringBuilder
					.AppendNode(commentNode, true, in childIndent, in writeNull);

				continue;
			}
			if (child is IXmlCharacterData cDataNode)
			{
				stringBuilder
					.AppendOptionalNewline(in format)
					.AppendOptionalIndent(in childIndent, in format);

				stringBuilder
					.AppendNode(cDataNode, true, in childIndent, in writeNull);
			}
		}

		if (!textOnly) stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(in indent, in format);

		stringBuilder
			.Append(XmlCharacterConstants.TagStartCharacter)
			.Append(XmlCharacterConstants.TagTerminationCharacter)
			.Append(Name)
			.Append(XmlCharacterConstants.TagEndCharacter);

		return stringBuilder;
	}

	#region IEquatable

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Name, _attributes, _children);

	#endregion
}
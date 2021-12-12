using Ardalis.GuardClauses;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Configuration;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    /// <inheritdoc cref="IXmlElement"/>
    [DebuggerDisplay("<{Name,nq} />")]
    public readonly struct XmlElement : IXmlElement
    {
        private static readonly int TypeHashCode = typeof(XmlElement).GetHashCode();

        public string Name { get; }

        private readonly List<IXmlNode> _children;
        private readonly List<IXmlAttribute> _attributes;

        public IReadOnlyList<IXmlNode> Children
        {
            get
            {
                var childList = new List<IXmlNode>(_attributes);
                childList.AddRange(_children);
                return childList;
            }
        }
        public IXmlAttribute? GetChildAttribute(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));

            return _attributes.FirstOrDefault(attribute => attribute.Name.Equals(name, StringComparison.Ordinal));
        }

        public IEnumerable<IXmlElement> GetChildElements(string? name = null)
        {
            foreach (var child in _children)
            {
                if (child is not IXmlElement element) continue;
                if (string.IsNullOrEmpty(name) || element.Name.Equals(name, StringComparison.Ordinal))
                    yield return element;
            }
        }
        public IXmlElement? GetChildElement(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));

            return GetChildElements(name).FirstOrDefault();
        }

        public string? GetTextValue()
        {
            var textValues = _children
                .Where(child => child is IXmlText)
                .Select(child => ((IXmlText)child).Value ?? string.Empty)
                .ToList();

            if (!textValues.Any()) return default;

            return string.Join(string.Empty, textValues);
        }

        /// <inheritdoc cref="XmlBuilder.Element"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlBuilder.Element"/> method instead of this constructor</b>
        /// </remarks>
        public XmlElement(string name) : this(name, new List<IXmlNode>(0)) { }

        /// <inheritdoc cref="XmlBuilder.Element(string, IXmlNode[])"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlBuilder.Element"/> method instead of this constructor</b>
        /// </remarks>
        public XmlElement(string name, params IXmlNode[] childNodes) : this(name, childNodes.AsEnumerable()) { }

        /// <inheritdoc cref="XmlBuilder.Element(string, IEnumerable{IXmlNode})"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlBuilder.Element"/> method instead of this constructor</b>
        /// </remarks>
        public XmlElement(string name, IEnumerable<IXmlNode> childNodes)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            _attributes = new List<IXmlAttribute>();
            _children = new List<IXmlNode>();

            foreach (var node in childNodes)
            {
                if (node is XmlAttribute attribute) _attributes.Add(attribute);
                else _children.Add(node);
            }
        }

        /// <inheritdoc cref="IXmlElement"/>
        /// <remarks>
        /// <b>Please use <see cref="XmlParser.Parse"/> method instead of this constructor</b>
        /// </remarks>
        public XmlElement(ReadOnlySpan<char> text, ref int offset)
        {
            // If we encounter a declaration just ignore it, if this becomes a problem we can start the parse in
            // the document. For now this is fine.
            if (text.HasStringAtOffset(offset, XmlCharacterConstants.DeclarationStart))
            {
                while (offset < text.Length && !text.HasStringAtOffset(offset, XmlCharacterConstants.DeclarationEnd))
                {
                    offset++;
                }
                while (offset < text.Length && text[offset] != XmlCharacterConstants.TagStartCharacter)
                {
                    offset++;
                }
            }

            offset++;

            _attributes = new List<IXmlAttribute>();
            _children = new List<IXmlNode>();

            var nameStartOffset = offset;
            var nameEndOffset = offset;

            var elementClosed = false;
            var tagFinished = false;
            var nameFinished = false;

            while (offset < text.Length)
            {
                nameEndOffset = offset;
                var character = text[offset];

                if (character == XmlCharacterConstants.TagTerminationCharacter && text[offset + 1] == XmlCharacterConstants.TagEndCharacter)
                {
                    elementClosed = true;
                    offset++;
                    tagFinished = true;
                    offset++;
                    break;
                }
                if (character == XmlCharacterConstants.TagEndCharacter)
                {
                    tagFinished = true;
                    offset++;
                    break;
                }

                if (nameFinished) break;
                offset++;
                if (char.IsWhiteSpace(character))
                {
                    nameFinished = true;
                }
            }
            
            Name = text[nameStartOffset..nameEndOffset].ToString().Trim();

            if (!tagFinished)
                while (offset < text.Length)
                {
                    var character = text[offset];

                    if (character == XmlCharacterConstants.TagTerminationCharacter && text[offset + 1] == XmlCharacterConstants.TagEndCharacter)
                    {
                        offset++;
                        elementClosed = true;
                        offset++;
                        break;
                    }
                    if (character == XmlCharacterConstants.TagEndCharacter)
                    {
                        offset++;
                        break;
                    }

                    if (char.IsWhiteSpace(character))
                    {
                        offset++;
                        continue;
                    }

                    _attributes.Add(new XmlAttribute(text, ref offset));
                }

            if (elementClosed) return;

            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == XmlCharacterConstants.TagStartCharacter && text[offset + 1] == XmlCharacterConstants.TagTerminationCharacter)
                {
                    offset += 2;
                    break;
                }

                if (character == XmlCharacterConstants.TagStartCharacter)
                {
                    if (text.HasStringAtOffset(offset, XmlCharacterConstants.CommentStart))
                    {
                        _children.Add(new XmlComment(text, ref offset));
                        continue;
                    }
                    if (text.HasStringAtOffset(offset, XmlCharacterConstants.CharacterDataStart))
                    {
                        _children.Add(new XmlCharacterData(text, ref offset));
                        continue;
                    }

                    _children.Add(new XmlElement(text, ref offset));
                    continue;
                }
                if (char.IsWhiteSpace(character))
                {
                    offset++;
                    continue;
                }

                _children.Add(new XmlText(text, ref offset));
            }

            // Walk to the end of the current closing tag
            while (offset < text.Length)
            {
                var character = text[offset];
                offset++;

                if (character == XmlCharacterConstants.TagEndCharacter) return;
            }
		}

		public override string ToString() => ((IDataNode)this).ToString(XmlSerializerConfiguration.Default);

		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in uint indent = 0, in bool writeNull = true)
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
					.AppendOptionalNewline(format)
                    .AppendOptionalIndent(indent, format)
                    .Append(spacer)
                    .AppendNode(attribute, format, childIndent, writeNull);
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
						.AppendOptionalNewline(format)
                        .AppendOptionalIndent(childIndent, format);

					stringBuilder
						.AppendNode(childElement, format, childIndent, writeNull);

                    continue;
                }
                if (child is IXmlText textNode)
                {
                    if (firstTextNode)
                    {
                        firstTextNode = false;
                        if (!textOnly) stringBuilder
							.AppendOptionalNewline(format)
                            .AppendOptionalIndent(childIndent, format);

						stringBuilder
							.AppendNode(textNode, true, childIndent, writeNull);

                        continue;
                    }

					stringBuilder
						.AppendNode(textNode, false, childIndent, writeNull);

                    continue;
                }
                if (child is IXmlComment commentNode)
                {
					stringBuilder
						.AppendOptionalNewline(format)
                        .AppendOptionalIndent(childIndent, format);

					stringBuilder
						.AppendNode(commentNode, true, childIndent, writeNull);

                    continue;
                }
                if (child is IXmlCharacterData cDataNode)
                {
					stringBuilder
						.AppendOptionalNewline(format)
                        .AppendOptionalIndent(childIndent, format);

					stringBuilder
						.AppendNode(cDataNode, true, childIndent, writeNull);
                }
            }

            if (!textOnly) stringBuilder
				.AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format);

			stringBuilder
				.Append(XmlCharacterConstants.TagStartCharacter)
                .Append(XmlCharacterConstants.TagTerminationCharacter)
                .Append(Name)
                .Append(XmlCharacterConstants.TagEndCharacter);

            return stringBuilder;
        }

        #region IEquatable

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

        public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Name, _attributes, _children);

        #endregion
    }
}

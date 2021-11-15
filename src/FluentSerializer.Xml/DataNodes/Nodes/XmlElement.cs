using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    [DebuggerDisplay("<{Name,nq} />")]
    public readonly struct XmlElement : IXmlElement
    {
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

        public XmlElement(string name, IEnumerable<IXmlNode> childNodes)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            _attributes = new();
            _children = new();

            foreach (var node in childNodes)
            {
                if (node is XmlAttribute attribute) _attributes.Add(attribute);
                else _children.Add(node);
            }
        }

        // todo support even more whitespace scenarios, probably setup some test cases first
        public XmlElement(ReadOnlySpan<char> text, ref int offset)
        {
            // If we encounter a declaration just ignore it, if this becomes a problem we can start the parse in
            // the document. For now this is fine.
            if (text.HasStringAtOffset(offset, XmlConstants.DeclarationStart))
            {
                while (offset < text.Length && !text.HasStringAtOffset(offset, XmlConstants.DeclarationEnd))
                {
                    offset++;
                }
                while (offset < text.Length && text[offset] != XmlConstants.TagStartCharacter)
                {
                    offset++;
                }
            }

            offset++;

            _attributes = new();
            _children = new();

            var elementClosed = false;
            var tagFinished = false;
            var nameFinished = false;

            var stringBuilder = new StringBuilder(128);
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == XmlConstants.TagTerminationCharacter && text[offset + 1] == XmlConstants.TagEndCharacter)
                {
                    elementClosed = true;
                    offset++;
                    tagFinished = true;
                    offset++;
                    break;
                }
                if (character == XmlConstants.TagEndCharacter)
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
                    continue;
                }

                stringBuilder.Append(character);
            }

            Name = stringBuilder.ToString();
            stringBuilder.Clear();

            if (!tagFinished)
                while (offset < text.Length)
                {
                    var character = text[offset];

                    if (character == XmlConstants.TagTerminationCharacter && text[offset + 1] == XmlConstants.TagEndCharacter)
                    {
                        offset++;
                        elementClosed = true;
                        offset++;
                        break;
                    }
                    if (character == XmlConstants.TagEndCharacter)
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

            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == XmlConstants.TagStartCharacter && text[offset + 1] == XmlConstants.TagTerminationCharacter)
                {
                    offset += 2 + Name.Length + 1;
                    return;
                }

                if (character == XmlConstants.TagStartCharacter)
                {
                    if (text.HasStringAtOffset(offset, XmlConstants.CommentStart))
                    {
                        _children.Add(new XmlComment(text, ref offset));
                        continue;
                    }
                    if (text.HasStringAtOffset(offset, XmlConstants.CharacterDataStart))
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
        }

        public XmlElement(string name) : this(name, new List<IXmlNode>(0)) { }
        public XmlElement(string name, params IXmlNode[] childNodes) : this(name, childNodes.AsEnumerable()) { }


        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            AppendTo(stringBuilder, false);
            return stringBuilder.ToString();
        }

        public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, int indent = 0, bool writeNull = true)
        {
            var stringBuilder = stringBuilders.Get();

            stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
            writer.Write(stringBuilder);

            stringBuilder.Clear();
            stringBuilders.Return(stringBuilder);
        }

        public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char spacer = ' ';

            var children = Children;
            var childIndent = format ? indent + 1 : 0;

            if (!writeNull && !children.Any()) return stringBuilder;

            stringBuilder
                .Append(XmlConstants.TagStartCharacter)
                .Append(Name);

            if (!children.Any()) return stringBuilder
                    .Append(spacer)
                    .Append(XmlConstants.TagTerminationCharacter)
                    .Append(XmlConstants.TagEndCharacter);

            foreach (var attribute in _attributes)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(indent, format)
                    .Append(spacer)
                    .AppendNode(attribute, format, childIndent, writeNull);
            }
            stringBuilder
                .Append(XmlConstants.TagEndCharacter);

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
                .Append(XmlConstants.TagStartCharacter)
                .Append(XmlConstants.TagTerminationCharacter)
                .Append(Name)
                .Append(XmlConstants.TagEndCharacter);

            return stringBuilder;
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IXmlNode xmlNode) return false;

            return Equals(xmlNode);
        }

        public bool Equals(IXmlNode? obj)
        {
            if (obj is not XmlElement otherElement) return false;

            if (!Name.Equals(otherElement.Name, StringComparison.Ordinal)) return false;
            if (_attributes.Count != otherElement._attributes.Count) return false;
            if (_children.Count != otherElement._children.Count) return false;

            if (_attributes.Any() && !_attributes.SequenceEqual(otherElement._attributes)) return false;
            if (_children.Any() && !_children.SequenceEqual(otherElement._children)) return false;

            return true;
        }

        public override int GetHashCode() => HashCode.Combine(Name, _attributes, _children);

        #endregion
    }
}

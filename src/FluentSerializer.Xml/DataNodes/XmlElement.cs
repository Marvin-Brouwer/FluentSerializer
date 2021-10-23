using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Xml.DataNodes
{
    [DebuggerDisplay("<{Name,nq} />")]
    public readonly struct XmlElement : IXmlContainer, IEquatable<IXmlNode>
    {
        private readonly List<XmlAttribute> _attributes;
        private readonly List<XmlElement> _children;
        private readonly List<XmlText> _textNodes;

        public IReadOnlyList<IXmlNode> Children
        {
            get
            {
                var value = new List<IXmlNode>();
                foreach (var attribute in _attributes) value.Add(attribute);
                foreach (var children in _children) value.Add(children);
                foreach (var textNodes in _textNodes) value.Add(textNodes);

                return value.AsReadOnly();
            }
        }

        public string Name { get; }
        public IReadOnlyList<XmlAttribute> AttributeNodes => _attributes.AsReadOnly();
        public IReadOnlyList<XmlElement> ElementNodes => _children.AsReadOnly();
        public IReadOnlyList<XmlText> TextNodes => _textNodes.AsReadOnly();


        public XmlElement(string name, IEnumerable<IXmlNode> childNodes)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            _attributes = new();
            _children = new();
            _textNodes = new();

            foreach(var node in childNodes)
            {
                if (node is XmlAttribute attribute) _attributes.Add(attribute);
                if (node is XmlElement element) _children.Add(element);
                if (node is XmlText textNode) _textNodes.Add(textNode);
            }
        }

        public XmlElement(ReadOnlySpan<char> text, StringBuilder stringBuilder, ref int offset)
        {
            const char tagStartCharacter = '<';
            const char tagEndharacter = '>';
            const char tagTerminateCharacter = '/';

            offset++;
            _attributes = new();
            _children = new();
            _textNodes = new();

            var elementClosed = false;
            var tagFinished = false;
            var nameFinished = false;
            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == tagTerminateCharacter && text[offset +1] == tagEndharacter)
                {
                    elementClosed = true;
                    offset++;
                    tagFinished = true;
                    offset++;
                    break;
                }
                if (character == tagEndharacter)
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

                    if (character == tagTerminateCharacter && text[offset + 1] == tagEndharacter)
                    {
                        offset++;
                        elementClosed = true;
                        offset++;
                        break;
                    }
                    if (character == tagEndharacter)
                    {
                        offset++;
                        break;
                    }

                    if (char.IsWhiteSpace(character))
                    {
                        offset++;
                        continue;
                    }

                    _attributes.Add(new XmlAttribute(text, stringBuilder, ref offset));
                }

            if (elementClosed) return;

            stringBuilder.Clear();
            while (offset < text.Length)
            {
                var character = text[offset];

                if (character == tagStartCharacter && text[offset + 1] == tagTerminateCharacter)
                {
                    offset+= 2 + Name.Length +1;
                    return;
                }
                if (character == tagStartCharacter)
                {
                    _children.Add(new XmlElement(text, stringBuilder, ref offset));
                    continue;
                }
                if (char.IsWhiteSpace(character))
                {
                    offset++;
                    continue;
                }

                _textNodes.Add(new XmlText(text, stringBuilder, ref offset));
            }
        }

        public XmlElement(string name) : this(name, new List<IXmlNode>(0)) { }
        public XmlElement(string name, params IXmlNode[] childNodes) : this(name, childNodes.AsEnumerable()) { }

        public override string ToString() => ToString(false);
        public string ToString(bool format) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            const char tagStartCharacter = '<';
            const char tagTerminaterCharacter = '/';
            const char tagEndCharacter = '>';
            const char spacer = ' ';

            var children = Children;
            var childIndent = format ? indent + 1 : 0;

            if (!writeNull && !children.Any()) return stringBuilder;

            stringBuilder
                .AppendOptionalNewline(false)
                .Append(tagStartCharacter)
                .Append(Name);

            if (!children.Any()) return stringBuilder
                    .Append(spacer)
                    .Append(tagTerminaterCharacter)
                    .Append(tagEndCharacter);

            foreach (var attribute in _attributes)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(indent, format)
                    .Append(spacer)
                    .AppendNode(attribute, format, childIndent);
            }
            stringBuilder
                .Append(tagEndCharacter);

            foreach (var child in _children)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent);
            }
            var first = true;
            foreach (var text in _textNodes)
            {
                if (first)
                {
                    first = false;
                    stringBuilder
                        .AppendOptionalNewline(format)
                        .AppendOptionalIndent(childIndent, format);

                    stringBuilder
                        .AppendNode(text, true, childIndent);
                    continue;
                }

                stringBuilder
                    .AppendNode(text, false, childIndent);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
                .Append(tagStartCharacter)
                .Append(tagTerminaterCharacter)
                .Append(Name)
                .Append(tagEndCharacter);

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

            return Name!.Equals(otherElement.Name, StringComparison.Ordinal)
                && _attributes.SequenceEqual(otherElement._attributes)
                && _children.SequenceEqual(otherElement._children)
                && _textNodes.SequenceEqual(otherElement._textNodes);
        }

        public override int GetHashCode() => HashCode.Combine(Name, _attributes, _children, _textNodes);

        #endregion
    }
}

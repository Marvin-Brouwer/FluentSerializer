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
        private readonly List<IXmlAttribute> _attributes;
        private readonly List<IXmlElement> _children;
        private readonly List<IXmlText> _textNodes;

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
        public IReadOnlyList<IXmlAttribute> AttributeNodes => _attributes.AsReadOnly();
        public IReadOnlyList<IXmlElement> ElementNodes => _children.AsReadOnly();
        public IReadOnlyList<IXmlText> TextNodes => _textNodes.AsReadOnly();

        public XmlElement(string name, IEnumerable<IXmlNode> childNodes)
        {
            Guard.Against.InvalidName(name, nameof(name));

            Name = name;
            _attributes = new();
            _children = new();
            _textNodes = new();

            foreach (var node in childNodes)
            {
                if (node is XmlAttribute attribute) _attributes.Add(attribute);
                if (node is XmlElement element) _children.Add(element);
                if (node is XmlText textNode) _textNodes.Add(textNode);
            }
        }

        // todo support even more whitespace scenarios, probably setup some test cases first
        public XmlElement(ReadOnlySpan<char> text, ref int offset)
        {
            offset++;

            _attributes = new();
            _children = new();
            _textNodes = new();

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

                    _children.Add(new XmlElement(text, ref offset));
                    continue;
                }
                if (char.IsWhiteSpace(character))
                {
                    offset++;
                    continue;
                }

                _textNodes.Add(new XmlText(text, ref offset));
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
                .AppendOptionalNewline(false)
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

            foreach (var child in _children)
            {
                stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format)
                    .AppendNode(child, format, childIndent, writeNull);
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
                        .AppendNode(text, true, childIndent, writeNull);
                    continue;
                }

                stringBuilder
                    .AppendNode(text, false, childIndent, writeNull);
            }

            stringBuilder
                .AppendOptionalNewline(format)
                .AppendOptionalIndent(indent, format)
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

            if (!Name!.Equals(otherElement.Name, StringComparison.Ordinal)) return false;
            if (_attributes.Count != otherElement._attributes.Count) return false;
            if (_children.Count != otherElement._children.Count) return false;
            if (_textNodes.Count != otherElement._textNodes.Count) return false;

            if (_attributes.Any() && !_attributes.SequenceEqual(otherElement._attributes)) return false;
            if (_children.Any() && !_children.SequenceEqual(otherElement._children)) return false;
            if (_textNodes.Any() && !_textNodes.SequenceEqual(otherElement._textNodes)) return false;

            return true;
        }

        public override int GetHashCode() => HashCode.Combine(Name, _attributes, _children, _textNodes);

        #endregion
    }
}

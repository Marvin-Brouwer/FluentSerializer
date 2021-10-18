using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data.Xml
{
    [DebuggerDisplay("<{Name,nq} />")]
    public readonly struct XmlElement : IXmlContainer
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

        public XmlElement(string name) : this(name, new List<IXmlNode>(0)) { }
        public XmlElement(string name, params IXmlNode[] childNodes) : this(name, childNodes.AsEnumerable()) { }

        public override string ToString() => ToString(true);
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
    }
}

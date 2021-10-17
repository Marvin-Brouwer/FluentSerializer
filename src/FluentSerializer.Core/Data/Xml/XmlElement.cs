using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data.Xml
{
    public sealed class XmlElement : IXmlContainer
    {
        private readonly List<XmlAttribute> _attributes;
        private readonly List<XmlElement> _children;
        private readonly List<XmlText> _textNodes;


        public IReadOnlyList<IXmlNode> Children
        {
            get
            {
                var value = new List<IXmlNode>();
                value.AddRange(_attributes);
                value.AddRange(_children);
                value.AddRange(_textNodes);

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

            _attributes.AddRange(childNodes.Where(node => node is XmlAttribute).Cast<XmlAttribute>());
            _children.AddRange(childNodes.Where(node => node is XmlElement).Cast<XmlElement>());
            _textNodes.AddRange(childNodes.Where(node => node is XmlText).Cast<XmlText>());
        }
        public XmlElement(string name) : this(name, new List<IXmlNode>(0)) { }
        public XmlElement(string name, params IXmlNode[] childNodes) : this(name, childNodes.AsEnumerable()) { }

        public override string ToString() => ToString(true);
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
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
                    .AppendOptionalIndent(childIndent, format)
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
            foreach (var text in _textNodes)
            {
                if (text == _textNodes[0])
                {
                    stringBuilder
                        .AppendOptionalNewline(format)
                        .AppendOptionalIndent(childIndent, format);
                }
                stringBuilder
                    .AppendNode(text, text == _textNodes[0], childIndent);
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

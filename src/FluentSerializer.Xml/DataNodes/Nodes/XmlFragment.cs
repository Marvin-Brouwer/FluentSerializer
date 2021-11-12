using FluentSerializer.Core.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    [DebuggerDisplay(FragmentName)]
    public readonly struct XmlFragment : IXmlElement
    {
        private readonly IXmlElement _innerElement;
        public IReadOnlyList<IXmlNode> Children => _innerElement.Children;

        private const string FragmentName = "</>";
        public string Name => FragmentName;


        public IXmlAttribute? GetChildAttribute(string name) => _innerElement.GetChildAttribute(name);

        public IEnumerable<IXmlElement> GetChildElements(string? name = null) => _innerElement.GetChildElements(name);
        public IXmlElement? GetChildElement(string name) => _innerElement.GetChildElement(name);

        public string? GetTextValue() => _innerElement.GetTextValue();

        public XmlFragment(IEnumerable<IXmlNode> elements)
        {
            _innerElement = new XmlElement(nameof(XmlFragment), elements);
        }

        public XmlFragment(params IXmlNode[] childNodes) : this(childNodes.AsEnumerable()) { }

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
            var childIndent = format ? indent + 1 : 0;

            if (!_innerElement.Children.Any()) return stringBuilder;

            var firstNode = true;
            foreach (var child in _innerElement.Children)
            {
                if (!firstNode) stringBuilder
                    .AppendOptionalNewline(format)
                    .AppendOptionalIndent(childIndent, format);

                stringBuilder
                    .AppendNode(child, format, childIndent, writeNull);

                firstNode = false;
            }

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
            if (obj is not XmlFragment otherFragment) return false;

            return _innerElement.Equals(otherFragment._innerElement);
        }

        public override int GetHashCode() => HashCode.Combine(Name, _innerElement.GetHashCode());

        #endregion
    }
}

using FluentSerializer.Core.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    /// <summary>
    /// A special XML element that doesn't print it's container but only its children
    /// </summary>
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

        /// <inheritdoc cref="XmlFragment"/>
        public XmlFragment(IEnumerable<IXmlNode> childNodes)
        {
            _innerElement = new XmlElement(nameof(XmlFragment), childNodes);
        }

        /// <inheritdoc cref="XmlFragment"/>
        public XmlFragment(params IXmlNode[] childNodes) : this(childNodes.AsEnumerable()) { }

        public override string ToString()
        {
            var stringBuilder = new StringFast();
            AppendTo(stringBuilder, false);
            return stringBuilder.ToString();
        }

        public string WriteTo(ObjectPool<StringFast> stringBuilders, bool format = true, bool writeNull = true, int indent = 0)
        {
            var stringBuilder = stringBuilders.Get();
            try
            {
                stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
                return stringBuilder.ToString();
            }
            finally
            {
                stringBuilders.Return(stringBuilder);
            }
        }

        public StringFast AppendTo(StringFast stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
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

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

        public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => _innerElement.GetHashCode();

        #endregion
    }
}

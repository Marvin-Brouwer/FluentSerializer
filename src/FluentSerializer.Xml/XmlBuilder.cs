using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System.Collections.Generic;

namespace FluentSerializer.Xml
{
    // todo inherit doc of interfaces and write docs for interfaces
    public readonly struct XmlBuilder
    {
        public static IXmlElement Element(string name, params IXmlNode[] nodes)
        {
            Guard.Against.InvalidName(name, nameof(name));

            return new XmlElement(name, nodes);
        }

        public static IXmlElement Element(string name, IEnumerable<IXmlNode> nodes)
        {
            Guard.Against.InvalidName(name, nameof(name));

            return new XmlElement(name, nodes);
        }
        public static IXmlAttribute Attribute(string name, string? value)
        {
            Guard.Against.InvalidName(name, nameof(name));

            return new XmlAttribute(name, value);
        }

        public static IXmlText Text(string? value) => new XmlText(value);
        public static IXmlComment Comment(string value) => new XmlComment(value);
    }
}

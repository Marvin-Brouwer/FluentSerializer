using Ardalis.GuardClauses;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System;

namespace FluentSerializer.Xml
{
    // todo inherit doc of interfaces and write docs for interfaces
    public readonly struct XmlParser
    {
        public static IXmlElement Parse(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));

            return Parse(value.AsSpan());
        }

        public static IXmlElement Parse(ReadOnlySpan<char> value)
        {
            Guard.Against.Zero(value.Length, nameof(value));
            Guard.Against.InvalidInput(value.IsEmpty, nameof(value), isEmpty => !isEmpty);

            var offset = 0;
            return new XmlElement(value, ref offset);
        }
    }
}

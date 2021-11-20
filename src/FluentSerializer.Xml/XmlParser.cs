using Ardalis.GuardClauses;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System;

namespace FluentSerializer.Xml
{
    /// <summary>
    /// XML parsing utility class
    /// </summary>
    public readonly struct XmlParser
    {
        /// <summary>
        /// Parse a string value to an XML object tree
        /// </summary>
        /// <param name="value">The XML to parse</param>
        public static IXmlElement Parse(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));

            return Parse(value.AsSpan());
        }

        /// <inheritdoc cref="Parse(string)"/>
        public static IXmlElement Parse(ReadOnlySpan<char> value)
        {
            Guard.Against.Zero(value.Length, nameof(value));
            Guard.Against.InvalidInput(value.IsEmpty, nameof(value), isEmpty => !isEmpty);

            var offset = 0;
            return new XmlElement(value, ref offset);
        }
    }
}

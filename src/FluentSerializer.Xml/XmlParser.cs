using Ardalis.GuardClauses;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System;

namespace FluentSerializer.Xml;

/// <summary>
/// XML parsing utility class
/// </summary>
public readonly struct XmlParser
{
	/// <summary>
	/// Parse a string value to an XML object tree
	/// </summary>
	/// <param name="value">The XML to parse</param>
	public static IXmlElement Parse(in string value)
	{
		Guard.Against.NullOrWhiteSpace(value, nameof(value));

		var offset = 0;
		return new XmlElement(value.AsSpan(), ref offset);
	}
}
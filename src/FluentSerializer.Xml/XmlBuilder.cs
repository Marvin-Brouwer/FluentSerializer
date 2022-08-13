using Ardalis.GuardClauses;

using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;

using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Xml;

/// <summary>
/// XML element builder utility class
/// </summary>
public static class XmlBuilder
{
	/// <inheritdoc cref="IXmlElement"/>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <param name="childNodes">A parameters list of <see cref="IXmlNode"/> as children of this element.</param>
	public static IXmlElement Element(in string name, params IXmlNode[] childNodes)
	{
		Guard.Against.InvalidName(in name);

		return new XmlElement(in name, childNodes);
	}

	/// <inheritdoc cref="IXmlElement"/>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <param name="childNode">An <see cref="IXmlNode"/> as single child of this element.</param>
	public static IXmlElement Element(in string name, in IXmlNode childNode)
	{
		Guard.Against.InvalidName(in name);

		return new XmlElement(in name, new List<IXmlNode>(1) { childNode });
	}

	/// <inheritdoc cref="IXmlElement"/>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	public static IXmlElement Element(in string name)
	{
		Guard.Against.InvalidName(in name);

		return new XmlElement(in name, Enumerable.Empty<IXmlNode>());
	}

	/// <param name="childNodes">A collection of <see cref="IXmlNode"/> as children of this element.</param>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <inheritdoc cref="Element(in string, IXmlNode[])"/>
	public static IXmlElement Element(in string name, in IEnumerable<IXmlNode> childNodes)
	{
		Guard.Against.InvalidName(in name);

		return new XmlElement(in name, in childNodes);
	}

	/// <inheritdoc cref="IXmlAttribute"/>
	/// <param name="name">A valid attribute name, will throw if given anything other than word characters.</param>
	/// <param name="value">The value assigned to the attribute</param>
	public static IXmlAttribute Attribute(in string name, in string? value)
	{
		Guard.Against.InvalidName(in name);

		if (value is null) return new XmlAttribute(in name, in string.Empty);
		return new XmlAttribute(in name, in value);
	}

	/// <inheritdoc cref="IXmlText"/>
	/// <param name="value">The value of this Text node</param>
	public static IXmlText Text(in string? value) => new XmlText(in value);

	/// <inheritdoc cref="IXmlComment"/>
	/// <param name="value">The string value of this comment</param>
	public static IXmlComment Comment(in string value) => new XmlComment(in value);

	/// <inheritdoc cref="IXmlCharacterData"/>
	/// <param name="value">A raw string value to include inside the document</param>
	public static IXmlCharacterData CData(in string value) => new XmlCharacterData(in value);
}
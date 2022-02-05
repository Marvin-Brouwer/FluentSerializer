using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System.Collections.Generic;

namespace FluentSerializer.Xml;

/// <summary>
/// XML element builder utility class
/// </summary>
public readonly struct XmlBuilder
{
	/// <inheritdoc cref="IXmlElement"/>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <param name="childNodes">A parameters list of <see cref="IXmlNode"/> as children of this element.</param>
	public static IXmlElement Element(string name, params IXmlNode[] childNodes)
	{
		Guard.Against.InvalidName(name);

		return new XmlElement(name, childNodes);
	}
	/// <param name="childNodes">A collection of <see cref="IXmlNode"/> as children of this element.</param>
	/// <param name="name">A valid property name, will throw if given anything other than word characters.</param>
	/// <inheritdoc cref="Element(string, IXmlNode[])"/>
	public static IXmlElement Element(string name, IEnumerable<IXmlNode> childNodes)
	{
		Guard.Against.InvalidName(name);

		return new XmlElement(name, childNodes);
	}

	/// <inheritdoc cref="IXmlAttribute"/>
	/// <param name="name">A valid attribute name, will throw if given anything other than word characters.</param>
	/// <param name="value">The value assigned to the attribute</param>
	public static IXmlAttribute Attribute(string name, string? value)
	{
		Guard.Against.InvalidName(name);

		return new XmlAttribute(name, value);
	}

	/// <inheritdoc cref="IXmlText"/>
	/// <param name="value">The value of this Text node</param>
	public static IXmlText Text(string? value) => new XmlText(value);

	/// <inheritdoc cref="IXmlComment"/>
	/// <param name="value">The string value of this comment</param>
	public static IXmlComment Comment(string value) => new XmlComment(value);

	/// <inheritdoc cref="IXmlCharacterData"/>
	/// <param name="value">A raw string value to include inside the document</param>
	public static IXmlCharacterData CData(string value) => new XmlCharacterData(value);
}
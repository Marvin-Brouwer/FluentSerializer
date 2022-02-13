using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <summary>
/// A special XML element that doesn't print it's container but only its children
/// </summary>
[DebuggerDisplay(FragmentName)]
public readonly partial struct XmlFragment : IXmlElement
{
	private const string FragmentName = "</>";

	private readonly IXmlElement _innerElement;

	/// <inheritdoc />
	public IReadOnlyList<IXmlNode> Children => _innerElement.Children;
	/// <inheritdoc />
	public string Name => FragmentName;

	/// <inheritdoc cref="XmlFragment"/>
	public XmlFragment(in IEnumerable<IXmlNode> childNodes)
	{
		_innerElement = XmlBuilder.Element(nameof(XmlFragment), in childNodes);
	}

	/// <inheritdoc cref="XmlFragment"/>
	public XmlFragment(in IXmlNode childNode)
	{
		_innerElement = XmlBuilder.Element(nameof(XmlFragment), in childNode);
	}

	/// <inheritdoc />
	public IXmlAttribute? GetChildAttribute(in string name) => _innerElement.GetChildAttribute(in name);

	/// <inheritdoc />
	public IEnumerable<IXmlElement> GetChildElements(string? name = null) => _innerElement.GetChildElements(name);
	/// <inheritdoc />
	public IXmlElement? GetChildElement(in string name) => _innerElement.GetChildElement(in name);

	/// <inheritdoc />
	public string? GetTextValue() => _innerElement.GetTextValue();
}
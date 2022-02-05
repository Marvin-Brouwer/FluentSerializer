using FluentSerializer.Core.DataNodes;
using FluentSerializer.Xml.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <summary>
/// A special XML element that doesn't print it's container but only its children
/// </summary>
[DebuggerDisplay(FragmentName)]
public readonly struct XmlFragment : IXmlElement
{
	private readonly IXmlElement _innerElement;
	/// <inheritdoc />
	public IReadOnlyList<IXmlNode> Children => _innerElement.Children;

	private const string FragmentName = "</>";
	/// <inheritdoc />
	public string Name => FragmentName;

	/// <inheritdoc />

	public IXmlAttribute? GetChildAttribute(in string name) => _innerElement.GetChildAttribute(in name);

	/// <inheritdoc />
	public IEnumerable<IXmlElement> GetChildElements(string? name = null) => _innerElement.GetChildElements(name);
	/// <inheritdoc />
	public IXmlElement? GetChildElement(in string name) => _innerElement.GetChildElement(in name);

	/// <inheritdoc />
	public string? GetTextValue() => _innerElement.GetTextValue();

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
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		var childIndent = format ? indent + 1 : 0;

		if (!_innerElement.Children.Any()) return stringBuilder;

		var firstNode = true;
		foreach (var child in _innerElement.Children)
		{
			if (!firstNode) stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in childIndent, format);

			stringBuilder
				.AppendNode(child, in format, in childIndent, in writeNull);

			firstNode = false;
		}

		return stringBuilder;
	}

	#region IEquatable

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => _innerElement.GetHashCode();

	#endregion
}
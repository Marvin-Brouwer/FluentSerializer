using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlElement"/>
[DebuggerDisplay("<{Name,nq} />")]
public readonly partial struct XmlElement : IXmlElement
{
	/// <inheritdoc />
	public string Name { get; }

	private readonly List<IXmlNode> _children;
	private readonly List<IXmlAttribute> _attributes;

	/// <inheritdoc />
	public IReadOnlyList<IXmlNode> Children
	{
		get
		{
			var childList = new List<IXmlNode>(_attributes);
			childList.AddRange(_children);
			return childList;
		}
	}

	/// <inheritdoc cref="XmlBuilder.Element(in string, in IEnumerable{IXmlNode})"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Element(in string, in IEnumerable{IXmlNode})"/> method instead of this constructor</b>
	/// </remarks>
	public XmlElement(in string name, in IEnumerable<IXmlNode> childNodes)
	{
		Guard.Against.InvalidName(in name);

		Name = name;
		_attributes = new List<IXmlAttribute>();
		_children = new List<IXmlNode>();

		foreach (var node in childNodes)
		{
			if (node is null) continue;
			if (node is IXmlText text && string.IsNullOrEmpty(text.Value)) continue;
			if (node is XmlAttribute attribute) _attributes.Add(attribute);
			else _children.Add(node);
		}
	}

	/// <inheritdoc />
	public IXmlAttribute? GetChildAttribute(in string name)
	{
		Guard.Against.NullOrWhiteSpace(name, nameof(name));

		foreach (var attribute in _attributes)
		{
			if (string.IsNullOrEmpty(name) || attribute.Name.Equals(name, StringComparison.Ordinal))
				return attribute;
		}

		return default;
	}

	/// <inheritdoc />
	public IEnumerable<IXmlElement> GetChildElements(string? name = null)
	{
		foreach (var child in _children)
		{
			if (child is not IXmlElement element) continue;
			if (string.IsNullOrEmpty(name) || element.Name.Equals(name, StringComparison.Ordinal))
				yield return element;
		}
	}
	/// <inheritdoc />
	public IXmlElement? GetChildElement(in string name)
	{
		Guard.Against.NullOrWhiteSpace(name, nameof(name));

		foreach (var child in _children)
		{
			if (child is not IXmlElement element) continue;
			if (string.IsNullOrEmpty(name) || element.Name.Equals(name, StringComparison.Ordinal))
				return element;
		}

		return default;
	}

	/// <inheritdoc />
	public string? GetTextValue()
	{
		string? returnValue = null;
		foreach (var child in _children)
		{
			if (child is not IXmlText element) continue;
			if (string.IsNullOrEmpty(element.Value)) continue;

			returnValue ??= string.Empty;
			returnValue += element.Value;
		}

		return returnValue;
	}
}
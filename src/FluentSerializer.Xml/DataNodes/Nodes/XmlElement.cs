using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlElement"/>
[DebuggerDisplay("<{Name,nq} />")]
public readonly partial struct XmlElement : IXmlElement
{
	/// <inheritdoc />
	public string Name { get; }

	private readonly IReadOnlyList<IXmlNode> _childNodes = ImmutableArray<IXmlNode>.Empty;
	private readonly IReadOnlyList<IXmlAttribute> _attributes = ImmutableArray<IXmlAttribute>.Empty;

	/// <inheritdoc />
	public IReadOnlyList<IXmlNode> Children { get; } = ImmutableArray<IXmlNode>.Empty;

	/// <inheritdoc cref="XmlBuilder.Element(in string, in IEnumerable{IXmlNode})"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Element(in string, in IEnumerable{IXmlNode})"/> method instead of this constructor</b>
	/// </remarks>
	public XmlElement(in string name, in IEnumerable<IXmlNode> childNodes)
	{
		Guard.Against.InvalidName(in name);

		Name = name;
		var attributes = ImmutableArray.CreateBuilder<IXmlAttribute>();
		var children = ImmutableArray.CreateBuilder<IXmlNode>();
		var allNodes = ImmutableArray.CreateBuilder<IXmlNode>();

		foreach (var node in childNodes)
		{
			if (node is null) continue;
			if (node is IXmlText text && string.IsNullOrEmpty(text.Value)) continue;
			if (node is XmlAttribute attribute) attributes.Add(attribute);
			else children.Add(node);
		}

		_attributes = attributes.ToImmutable();
		_childNodes = children.ToImmutable();

		allNodes.AddRange(attributes);
		allNodes.AddRange(children);
		Children = allNodes.ToImmutable();
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
		foreach (var child in _childNodes)
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

		foreach (var child in _childNodes)
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
		var stringBuilder = new StringBuilder(128);
		var hasText = false;

		foreach (var child in _childNodes)
		{
			if (child is not IXmlText element) continue;
			if (string.IsNullOrEmpty(element.Value)) continue;

			stringBuilder.Append(element.Value);
			hasText = true;
		}

		return hasText
			? stringBuilder.ToString()
			: null;
	}
}
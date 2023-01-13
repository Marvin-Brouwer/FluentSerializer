using Ardalis.GuardClauses;

using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlElement"/>
[DebuggerDisplay("<{Name,nq} />")]
public readonly partial struct XmlElement : IXmlElement
{
	/// <inheritdoc />
	public string Name { get; }

	private readonly IReadOnlyList<IXmlNode> _children;
	private readonly IReadOnlyList<IXmlAttribute> _attributes;

	/// <inheritdoc />
	public IReadOnlyList<IXmlNode> Children
	{
		get
		{
			var children = new ReadOnlyCollectionBuilder<IXmlNode>(_attributes);
			foreach (var child in _children) children.Add(child);

			return children.ToReadOnlyCollection();
		}
	}

	/// <inheritdoc cref="XmlBuilder.Element(in string, in IEnumerable{IXmlNode})"/>
	/// <remarks>
	/// <b>Please use <see cref="XmlBuilder.Element(in string, in IEnumerable{IXmlNode})"/> method instead of this constructor</b>
	/// </remarks>
	public XmlElement(in string name, in IEnumerable<IXmlNode>? childNodes)
	{
		Guard.Against.InvalidName(in name);

		Name = name;

		if (childNodes is null)
		{
			_attributes = Array.Empty<IXmlAttribute>();
			_children = Array.Empty<IXmlNode>();
			return;
		}

		// This does not have the same impact as when you'd have one collection.
		// However, it will still ensure a smaller initial size than the dotnet default on smaller element collections.
		InitializeChildNodes(in childNodes,
			out ReadOnlyCollectionBuilder<IXmlAttribute> attributes,
			out ReadOnlyCollectionBuilder<IXmlNode> children);

		if (childNodes is not null)
			foreach (var node in childNodes)
			{
				if (node is null) continue;
				if (node is IXmlText text && string.IsNullOrEmpty(text.Value)) continue;
				if (node is XmlAttribute attribute) attributes.Add(attribute);
				else children.Add(node);
			}

		_attributes = attributes.ToReadOnlyCollection();
		_children = children.ToReadOnlyCollection();
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static void InitializeChildNodes(in IEnumerable<IXmlNode> childNodes, out ReadOnlyCollectionBuilder<IXmlAttribute> attributes, out ReadOnlyCollectionBuilder<IXmlNode> children)
	{
		if (childNodes is ICollection<IXmlNode> childrenCollection)
		{
			attributes = new ReadOnlyCollectionBuilder<IXmlAttribute>(childrenCollection.Count);
			children = new ReadOnlyCollectionBuilder<IXmlNode>(childrenCollection.Count);
		}
		else if (childNodes is IReadOnlyCollection<IXmlNode> childrenReadonlyCollection)
		{
			attributes = new ReadOnlyCollectionBuilder<IXmlAttribute>(childrenReadonlyCollection.Count);
			children = new ReadOnlyCollectionBuilder<IXmlNode>(childrenReadonlyCollection.Count);
		}
		else
		{
			attributes = new ReadOnlyCollectionBuilder<IXmlAttribute>();
			children = new ReadOnlyCollectionBuilder<IXmlNode>();
		}
	}

	/// <inheritdoc />
	public IXmlAttribute? GetChildAttribute(in string name)
	{
		Guard.Against.NullOrWhiteSpace(name
#if NETSTANDARD
			, nameof(name)
#endif
		);

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
		Guard.Against.NullOrWhiteSpace(name
#if NETSTANDARD
			, nameof(name)
#endif
		);

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
		var stringBuilder = new StringBuilder(64);
		var hasText = false;

		foreach (var child in _children)
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
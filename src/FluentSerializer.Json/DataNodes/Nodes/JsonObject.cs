using Ardalis.GuardClauses;

using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonObject"/>
[DebuggerDisplay("{ObjectName, nq}")]
public readonly partial struct JsonObject : IJsonObject
{
	private const string ObjectName = "{ }";

	/// <inheritdoc />
	public string Name => ObjectName;

	private readonly int? _lastPropertyIndex;
	private readonly IReadOnlyList<IJsonNode> _children;

	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children => _children ?? Array.Empty<IJsonNode>();

	/// <inheritdoc cref="object"/>
	/// <remarks>
	/// <b>Please use <see cref="object"/> method instead of this constructor</b>
	/// </remarks>
	public JsonObject(in IEnumerable<IJsonObjectContent>? properties)
	{
		_lastPropertyIndex = null;

		if (properties is null
			|| properties.Equals(Enumerable.Empty<IJsonObjectContent>())
			|| properties.Equals(Array.Empty<IJsonObjectContent>()))
		{
			_children = Array.Empty<IJsonNode>();
		}
		else
		{
			var currentPropertyIndex = 0;
			var children = properties switch
			{
				ICollection<IJsonObjectContent> { Count: > 0 } collection =>
					new ReadOnlyCollectionBuilder<IJsonNode>(collection.Count),
				IReadOnlyCollection<IJsonObjectContent> { Count: > 0 } collection =>
					new ReadOnlyCollectionBuilder<IJsonNode>(collection.Count),
				_ => new ReadOnlyCollectionBuilder<IJsonNode>()
			};

			foreach (var property in properties)
			{
				children.Add(property);
				if (property is not IJsonComment) _lastPropertyIndex = currentPropertyIndex;
				currentPropertyIndex++;
			}

			_children = children.ToReadOnlyCollection();
		}
	}

	/// <inheritdoc />
	public IJsonProperty? GetProperty(in string name)
	{
		Guard.Against.InvalidName(name);

		foreach (var child in Children)
			if (child.Name.Equals(name, StringComparison.Ordinal)) return child as IJsonProperty;

		return null;
	}
}
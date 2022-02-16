using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonObject"/>
[DebuggerDisplay("{ObjectName, nq}")]
public readonly partial struct JsonObject : IJsonObject
{
	private const string ObjectName = "{ }";

	/// <inheritdoc />
	public string Name => ObjectName;

	private readonly int? _lastPropertyIndex;
	private readonly List<IJsonNode> _children;
	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

	/// <inheritdoc cref="JsonBuilder.Object(in IEnumerable{IJsonObjectContent})"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Object(in IEnumerable{IJsonObjectContent})"/> method instead of this constructor</b>
	/// </remarks>
	public JsonObject(in IEnumerable<IJsonObjectContent>? properties)
	{
		_lastPropertyIndex = null;

		if (properties is null)
		{
			_children = new List<IJsonNode>(0);
		}
		else
		{
			var currentPropertyIndex = 0;
			_children = new List<IJsonNode>();
			foreach (var property in properties)
			{
				_children.Add(property);
				if (property is not IJsonComment) _lastPropertyIndex = currentPropertyIndex;
				currentPropertyIndex++;
			}
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
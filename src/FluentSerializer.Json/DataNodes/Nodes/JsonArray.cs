using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonArray"/>
[DebuggerDisplay("{ArrayName, nq}")]
public readonly partial struct JsonArray : IJsonArray
{
	private const string ArrayName = "[ ]";
	/// <inheritdoc />
	public string Name => ArrayName;

	private readonly int? _lastNonCommentChildIndex;
	private readonly IReadOnlyList<IJsonNode> _children;
	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children => _children ?? Array.Empty<IJsonNode>();

	/// <inheritdoc cref="JsonBuilder.Array(in IEnumerable{IJsonArrayContent})"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Array(in IEnumerable{IJsonArrayContent})"/> method instead of this constructor</b>
	/// </remarks>
	public JsonArray(in IEnumerable<IJsonArrayContent>? elements)
	{
		_lastNonCommentChildIndex = null;

		if (elements is null
		    || elements.Equals(Enumerable.Empty<IJsonArrayContent>())
		    || elements.Equals(Array.Empty<IJsonArrayContent>()))
		{
			_children = Array.Empty<IJsonNode>();
		}
		else
		{
			var currentChildIndex = 0;
			var children = elements is ICollection<IJsonArrayContent> childrenCollection
				? new ReadOnlyCollectionBuilder<IJsonNode>(childrenCollection.Count)
				: new ReadOnlyCollectionBuilder<IJsonNode>();

			foreach (var property in elements)
			{
				children.Add(property);
				if (property is not IJsonComment) _lastNonCommentChildIndex = currentChildIndex;
				currentChildIndex++;
			}

			_children = children.ToReadOnlyCollection();
		}
	}
}
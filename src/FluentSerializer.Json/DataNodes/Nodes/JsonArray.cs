using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonArray"/>
[DebuggerDisplay("{ArrayName, nq}")]
public readonly partial struct JsonArray : IJsonArray
{
	private const string ArrayName = "[ ]";
	/// <inheritdoc />
	public string Name => ArrayName;

	private readonly int? _lastNonCommentChildIndex;

	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children { get; } = ImmutableArray<IJsonNode>.Empty;

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
			Children = ImmutableArray<IJsonNode>.Empty;
		}
		else
		{
			var children = ImmutableArray.CreateBuilder<IJsonNode>();
			var currentChildIndex = 0;
			foreach (var property in elements)
			{
				children.Add(property);
				if (property is not IJsonComment) _lastNonCommentChildIndex = currentChildIndex;
				currentChildIndex++;

			}

			Children = children.ToImmutable();
		}
	}
}
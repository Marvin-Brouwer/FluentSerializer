using System;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonArray"/>
[DebuggerDisplay("{ArrayName, nq}")]
public readonly partial struct JsonArray : IJsonArray
{
	private const string ArrayName = "[ ]";
	/// <inheritdoc />
	public string Name => ArrayName;

	private readonly int? _lastNonCommentChildIndex;
	private readonly List<IJsonNode> _children;
	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children => _children ?? new List<IJsonNode>();

	/// <inheritdoc cref="JsonBuilder.Array(in IEnumerable{IJsonArrayContent})"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Array(in IEnumerable{IJsonArrayContent})"/> method instead of this constructor</b>
	/// </remarks>
	public JsonArray(in IEnumerable<IJsonArrayContent>? elements)
	{
		_lastNonCommentChildIndex = null;

		if (elements is null || elements.Equals(Enumerable.Empty<IJsonArrayContent>()))
		{
			_children = new List<IJsonNode>(0);
		}
		else
		{
			_children = new List<IJsonNode>();
			var currentChildIndex = 0;
			foreach (var property in elements)
			{
				_children.Add(property);
				if (property is not IJsonComment) _lastNonCommentChildIndex = currentChildIndex;
				currentChildIndex++;

			}
		}
	}
}
using System;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonProperty"/>
[DebuggerDisplay("{Name}: {GetDebugValue(), nq},")]
public readonly partial struct JsonProperty : IJsonProperty
{
	[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
	private string GetDebugValue()
	{
		if (_children.Count == 0) return JsonCharacterConstants.NullValue;
		var value = _children[0];

		if (value is JsonValue jsonValue)
			return jsonValue.Value ?? JsonCharacterConstants.NullValue;
		return value.Name;
	}

	/// <inheritdoc />
	public string Name { get; }

	private readonly IReadOnlyList<IJsonNode> _children;

	/// <inheritdoc />
	public bool HasValue { get; }

	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children => _children;

	/// <inheritdoc />
	public IJsonNode? Value => _children.FirstOrDefault();

	/// <inheritdoc cref="JsonBuilder.Property(in string, in IJsonPropertyContent)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Property(in string, in IJsonPropertyContent)"/> method instead of this constructor</b>
	/// </remarks>
	public JsonProperty(in string name, in IJsonPropertyContent? value)
	{
		Guard.Against.InvalidName(in name);

		Name = name;
		HasValue = value is not IJsonValue jsonValue || jsonValue.HasValue;

		_children = value is null
			? Array.Empty<IJsonNode>()
			: new ReadOnlyCollection<IJsonNode>(new IJsonNode[] { value });
	}
}
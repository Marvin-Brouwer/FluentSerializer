using Ardalis.GuardClauses;
using FluentSerializer.Core.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonProperty"/>
[DebuggerDisplay("{Name}: {GetDebugValue(), nq},")]
public readonly partial struct JsonProperty : IJsonProperty
{
	[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
	private string GetDebugValue()
	{
		if (Children.Count == 0) return JsonCharacterConstants.NullValue;
		var value = Children[0];
		if (value is JsonValue jsonValue) return jsonValue.Value ?? JsonCharacterConstants.NullValue;
		return value.Name;
	}

	/// <inheritdoc />
	public string Name { get; }

	/// <inheritdoc />
	public bool HasValue { get; }

	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children { get; } = ImmutableArray<IJsonNode>.Empty;

	/// <inheritdoc />
	public IJsonNode? Value => Children.Count == 0 ? null : Children[0];

	/// <inheritdoc cref="JsonBuilder.Property(in string, in IJsonPropertyContent)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Property(in string, in IJsonPropertyContent)"/> method instead of this constructor</b>
	/// </remarks>
	public JsonProperty(in string name, in IJsonPropertyContent? value)
	{
		Guard.Against.InvalidName(in name);

		Name = name;
		HasValue = value is not IJsonValue jsonValue || jsonValue.HasValue;

		Children = value is null
			? ImmutableArray<IJsonNode>.Empty
			: ImmutableArray.Create(value);
	}
}
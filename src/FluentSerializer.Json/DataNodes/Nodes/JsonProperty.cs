using Ardalis.GuardClauses;

using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonProperty"/>
[DebuggerDisplay("{Name}: {GetDebugValue(), nq},")]
public readonly partial struct JsonProperty : IJsonProperty
{
	[DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough, ExcludeFromCodeCoverage]
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

	private readonly ISingleItemCollection<IJsonNode> _children;

	/// <inheritdoc />
#if NET5_0_OR_GREATER
	[MemberNotNullWhen(true, nameof(Value))]
#endif
	public bool HasValue => !_children.IsEmpty && (_children.SingleItem is not IJsonValue jsonValue || jsonValue.HasValue);

	/// <inheritdoc />
	public IReadOnlyList<IJsonNode> Children => _children;

	/// <inheritdoc />
	public IJsonNode? Value => _children.SingleItem;

	/// <inheritdoc cref="JsonBuilder.Property(in string, in IJsonPropertyContent)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Property(in string, in IJsonPropertyContent)"/> method instead of this constructor</b>
	/// </remarks>
	public JsonProperty(in string name, in IJsonPropertyContent? value)
	{
		Guard.Against.InvalidName(in name);

		Name = name;

		_children = SingleItemCollection.ForItem<IJsonNode>(value ?? new JsonValue(null));
	}
}
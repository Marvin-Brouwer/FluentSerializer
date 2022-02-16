using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonValue
{
	private static readonly int TypeHashCode = typeof(JsonValue).GetHashCode();
	
	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);
}
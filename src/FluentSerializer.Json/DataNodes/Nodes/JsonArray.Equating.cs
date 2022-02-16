using System;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonArray
{
	private static readonly int TypeHashCode = typeof(JsonArray).GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IJsonNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, _children);
}
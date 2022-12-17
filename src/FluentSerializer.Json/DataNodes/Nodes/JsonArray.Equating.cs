using FluentSerializer.Core.DataNodes;

using System;
using System.Linq;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonArray
{
	private static readonly int TypeHashCode = typeof(JsonArray).GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IJsonNode? other)
	{
		if (other is not JsonArray otherArray) return false;
		if (otherArray.Children.Count != Children.Count) return false;

		return Enumerable.SequenceEqual(otherArray._children, _children);
	}

	/// <inheritdoc />
	public HashCode GetNodeHash() => DataNodeHashingHelper.GetHashCodeForAll(TypeHashCode, _children);

	/// <inheritdoc />
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public override int GetHashCode() => GetNodeHash().ToHashCode();

	/// <summary>Indicates whether the current object is equal to another object of the same interface.</summary>
	/// <param name="left">The left side object to compare with this object.</param>
	/// <param name="right">The right side object to compare with this object.</param>
	/// <returns>
	/// <see langword="true" /> if the <paramref name="left" /> object is equal to the <paramref name="right" /> parameter;
	/// otherwise, <see langword="false" />.</returns>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator ==(JsonArray left, IDataNode right) => left.Equals(right);

	/// <summary>Indicates whether the current object is <strong>not</strong> equal to another object of the same interface.</summary>
	/// <param name="left">The left side object to compare with this object.</param>
	/// <param name="right">The right side object to compare with this object.</param>
	/// <returns>
	/// <see langword="false" /> if the <paramref name="left" /> object is equal to the <paramref name="right" /> parameter;
	/// otherwise, <see langword="true" />.</returns>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator !=(JsonArray left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc cref="op_Equality(JsonArray, IDataNode)"/>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator ==(IDataNode left, JsonArray right) => Equals(left, right);

	/// <inheritdoc cref="op_Inequality(JsonArray, IDataNode)" />
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator !=(IDataNode left, JsonArray right) => !Equals(left, right);
}
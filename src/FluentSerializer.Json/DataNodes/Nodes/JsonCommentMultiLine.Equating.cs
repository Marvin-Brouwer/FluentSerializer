using FluentSerializer.Core.DataNodes;

using System;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonCommentMultiLine
{
	private static readonly int TypeHashCode = typeof(JsonCommentMultiLine).GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IJsonNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IJsonNode? other)
	{
		if (other is not JsonCommentMultiLine otherComment) return false;

		return string.Equals(otherComment.Value, Value, StringComparison.OrdinalIgnoreCase);
	}

	/// <inheritdoc />
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public HashCode GetNodeHash() => DataNodeHashingHelper.GetHashCodeForAll(TypeHashCode, Value);

	/// <inheritdoc />
	public override int GetHashCode() => GetNodeHash().ToHashCode();

	/// <summary>Indicates whether the current object is equal to another object of the same interface.</summary>
	/// <param name="left">The left side object to compare with this object.</param>
	/// <param name="right">The right side object to compare with this object.</param>
	/// <returns>
	/// <see langword="true" /> if the <paramref name="left" /> object is equal to the <paramref name="right" /> parameter;
	/// otherwise, <see langword="false" />.</returns>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator ==(JsonCommentMultiLine left, IDataNode right) => left.Equals(right);

	/// <summary>Indicates whether the current object is <strong>not</strong> equal to another object of the same interface.</summary>
	/// <param name="left">The left side object to compare with this object.</param>
	/// <param name="right">The right side object to compare with this object.</param>
	/// <returns>
	/// <see langword="false" /> if the <paramref name="left" /> object is equal to the <paramref name="right" /> parameter;
	/// otherwise, <see langword="true" />.</returns>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator !=(JsonCommentMultiLine left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc cref="op_Equality(JsonCommentMultiLine, IDataNode)"/>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator ==(IDataNode left, JsonCommentMultiLine right) => left.Equals(right);

	/// <inheritdoc cref="op_Inequality(JsonCommentMultiLine, IDataNode)" />
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator !=(IDataNode left, JsonCommentMultiLine right) => !left.Equals(right);
}
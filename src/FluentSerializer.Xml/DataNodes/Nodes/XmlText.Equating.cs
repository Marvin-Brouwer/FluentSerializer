using FluentSerializer.Core.DataNodes;

using System;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlText
{
	private static readonly int TypeHashCode = typeof(XmlText).GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other)
	{
		if (other is not XmlText otherText) return false;

		return string.Equals(otherText.Value ?? string.Empty, Value ?? string.Empty, StringComparison.OrdinalIgnoreCase);
	}

	/// <inheritdoc />
	public HashCode GetNodeHash() => DataNodeHashingHelper.GetHashCodeForAll(TypeHashCode, Value);

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
	public static bool operator ==(XmlText left, IDataNode right) => left.Equals(right);

	/// <summary>Indicates whether the current object is <strong>not</strong> equal to another object of the same interface.</summary>
	/// <param name="left">The left side object to compare with this object.</param>
	/// <param name="right">The right side object to compare with this object.</param>
	/// <returns>
	/// <see langword="false" /> if the <paramref name="left" /> object is equal to the <paramref name="right" /> parameter;
	/// otherwise, <see langword="true" />.</returns>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator !=(XmlText left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc cref="op_Equality(XmlText, IDataNode)"/>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator ==(IDataNode left, XmlText right) => left.Equals(right);

	/// <inheritdoc cref="op_Inequality(XmlText, IDataNode)" />
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public static bool operator !=(IDataNode left, XmlText right) => !left.Equals(right);
}
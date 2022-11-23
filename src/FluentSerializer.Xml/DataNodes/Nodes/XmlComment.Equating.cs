using FluentSerializer.Core.Comparing;
using FluentSerializer.Core.DataNodes;

using System;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlComment
{
	private static readonly int TypeHashCode = typeof(XmlComment).GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public HashCode GetNodeHash() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	/// <inheritdoc />
	public override int GetHashCode() => GetNodeHash().ToHashCode();

	/// <summary>Indicates whether the current object is equal to another object of the same interface.</summary>
	/// <param name="left">The left side object to compare with this object.</param>
	/// <param name="right">The right side object to compare with this object.</param>
	/// <returns>
	/// <see langword="true" /> if the <paramref name="left" /> object is equal to the <paramref name="right" /> parameter;
	/// otherwise, <see langword="false" />.</returns>
	public static bool operator ==(XmlComment left, IDataNode right) => left.Equals(right);

	/// <summary>Indicates whether the current object is <strong>not</strong> equal to another object of the same interface.</summary>
	/// <param name="left">The left side object to compare with this object.</param>
	/// <param name="right">The right side object to compare with this object.</param>
	/// <returns>
	/// <see langword="false" /> if the <paramref name="left" /> object is equal to the <paramref name="right" /> parameter;
	/// otherwise, <see langword="true" />.</returns>
	public static bool operator !=(XmlComment left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc cref="op_Equality(XmlComment, IDataNode)"/>
	public static bool operator ==(IDataNode left, XmlComment right) => Equals(left, right);

	/// <inheritdoc cref="op_Inequality(XmlComment, IDataNode)" />
	public static bool operator !=(IDataNode left, XmlComment right) => !Equals(left, right);
}
using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlElement
{
	private static readonly int TypeHashCode = typeof(XmlElement).GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Name, _attributes, _children);

	/// <inheritdoc />
	public static bool operator ==(XmlElement left, IDataNode right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(XmlElement left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc />
	public static bool operator ==(IDataNode left, XmlElement right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(IDataNode left, XmlElement right) => !left.Equals(right);
}
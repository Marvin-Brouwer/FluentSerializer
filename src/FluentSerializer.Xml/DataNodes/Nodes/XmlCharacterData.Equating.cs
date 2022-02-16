using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlCharacterData
{
	private static readonly int TypeHashCode = typeof(XmlCharacterData).GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	/// <inheritdoc />
	public static bool operator ==(XmlCharacterData left, IDataNode right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(XmlCharacterData left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc />
	public static bool operator ==(IDataNode left, XmlCharacterData right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(IDataNode left, XmlCharacterData right) => !left.Equals(right);
}
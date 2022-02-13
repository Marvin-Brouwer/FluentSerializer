using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlText
{
	private static readonly int TypeHashCode = typeof(XmlText).GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, Value);

	/// <inheritdoc />
	public static bool operator ==(XmlText left, IDataNode right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(XmlText left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc />
	public static bool operator ==(IDataNode left, XmlText right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(IDataNode left, XmlText right) => !left.Equals(right);
}
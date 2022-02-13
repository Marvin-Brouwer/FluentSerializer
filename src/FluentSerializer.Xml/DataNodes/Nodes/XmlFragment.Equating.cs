using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlFragment
{
	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => _innerElement.GetHashCode();

	/// <inheritdoc />
	public static bool operator ==(XmlFragment left, IDataNode right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(XmlFragment left, IDataNode right) => !left.Equals(right);

	/// <inheritdoc />
	public static bool operator ==(IDataNode left, XmlFragment right) => left.Equals(right);

	/// <inheritdoc />
	public static bool operator !=(IDataNode left, XmlFragment right) => !left.Equals(right);
}
namespace FluentSerializer.Core.DataNodes;

/// <summary>
/// Generic representation of a serializable data node that contains a flat value
/// </summary>
public interface IDataValue : IDataNode
{
	/// <summary>
	/// Value of this value node
	/// </summary>
	string? Value { get; }
}
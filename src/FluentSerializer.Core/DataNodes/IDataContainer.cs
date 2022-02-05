using System.Collections.Generic;

namespace FluentSerializer.Core.DataNodes;

/// <summary>
/// Generic representation of a serializable data node that contains other data nodes
/// </summary>
public interface IDataContainer<out TValue> : IDataNode
	where TValue : IDataNode
{
	/// <summary>
	/// The datanodes nested in this data node
	/// </summary>
	IReadOnlyList<TValue> Children { get; }
}
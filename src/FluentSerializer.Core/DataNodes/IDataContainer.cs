using System.Collections.Generic;

namespace FluentSerializer.Core.DataNodes;

public interface IDataContainer<out TValue> : IDataNode
	where TValue : IDataNode
{
	IReadOnlyList<TValue> Children { get; }
}
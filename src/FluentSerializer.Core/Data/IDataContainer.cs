using System.Collections.Generic;

namespace FluentSerializer.Core.Data
{
    public interface IDataContainer<out TValue> : IDataNode
        where TValue : IDataNode
    {
        IReadOnlyList<TValue> Children { get; }
    }
}

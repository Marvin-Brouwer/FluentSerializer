using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Json.DataNodes
{
    public interface IJsonContainer<out TContainer> : IJsonContainer, IDataContainer<IJsonNode>, IJsonNode
        where TContainer : IDataContainer<IJsonNode>
    { }
    public interface IJsonContainer : IDataContainer<IJsonNode>, IJsonNode
    { }
}

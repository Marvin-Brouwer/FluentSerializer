using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Json.DataNodes
{
    public interface IJsonContainer<out TContainer> : IJsonContainer
        where TContainer : IDataContainer<IJsonNode>
    { }
    public interface IJsonContainer : IDataContainer<IJsonNode>, IJsonNode
    { }
}

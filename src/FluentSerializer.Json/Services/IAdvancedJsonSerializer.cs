using System;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Services
{
    public interface IAdvancedJsonSerializer : IJsonSerializer
    {
        object? Deserialize(IJsonContainer element, Type modelType);
        TContainer? SerializeToContainer<TContainer>(object? model, Type modelType)
            where TContainer : IJsonContainer;
    }
}

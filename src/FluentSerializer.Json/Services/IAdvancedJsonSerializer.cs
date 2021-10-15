using System;
using Newtonsoft.Json.Linq;

namespace FluentSerializer.Json.Services
{
    public interface IAdvancedJsonSerializer : IJsonSerializer
    {
        object? Deserialize(JContainer element, Type modelType);
        TContainer? SerializeToContainer<TContainer>(object? model, Type modelType)
            where TContainer : JContainer;
    }
}

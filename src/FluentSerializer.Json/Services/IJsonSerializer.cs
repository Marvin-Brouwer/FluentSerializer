using FluentSerializer.Core.Services;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Services
{
    public interface IJsonSerializer : ISerializer
    {
        JsonSerializerConfiguration JsonConfiguration { get; }

        TModel? Deserialize<TModel>(IJsonContainer element) where TModel: class, new ();
        IJsonContainer? SerializeToContainer<TModel>(TModel model);
    }
}
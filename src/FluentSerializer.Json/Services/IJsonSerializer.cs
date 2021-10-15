using FluentSerializer.Core.Services;
using FluentSerializer.Json.Configuration;
using Newtonsoft.Json.Linq;

namespace FluentSerializer.Json.Services
{
    public interface IJsonSerializer : ISerializer
    {
        JsonSerializerConfiguration XmlConfiguration { get; }

        TModel? Deserialize<TModel>(JContainer element) where TModel: class, new ();
        JContainer? SerializeToContainer<TModel>(TModel model);
    }
}
using System.Text.Json;

namespace FluentSerializer.Json.Services
{
    public interface IJsonSerializer : ISerializer
    {
        string Serialize(JsonDocument dataObject);
        string Serialize(JsonElement dataObject);
        JsonDocument DeserializeToDocument(string dataObject);
        JsonElement DeserializeToElement(string dataObject);
    }
}
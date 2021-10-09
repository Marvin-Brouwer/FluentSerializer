using FluentSerializer.Json.Profiles;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FluentSerializer.Json.Services
{
    public sealed class FluentJsonSerializer : IJsonSerializer
    {
        public FluentJsonSerializer(List<JsonSerializerProfile> profiles)
        {
        }

        public TModel Deserialize<TModel>(string stringData) where TModel : class, new()
        {
            throw new NotImplementedException();
        }

        public JsonDocument DeserializeToDocument(string dataObject)
        {
            throw new NotImplementedException();
        }

        public JsonElement DeserializeToElement(string dataObject)
        {
            throw new NotImplementedException();
        }

        public string Serialize(JsonDocument dataObject)
        {
            throw new NotImplementedException();
        }

        public string Serialize(JsonElement dataObject)
        {
            throw new NotImplementedException();
        }

        public string Serialize<TModel>(TModel model)
        {
            throw new NotImplementedException();
        }
    }
}

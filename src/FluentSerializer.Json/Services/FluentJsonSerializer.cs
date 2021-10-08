using FluentSerializer.Json.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FluentSerializer.Json.Services
{
    public sealed class FluentJsonSerializer : IJsonSerializer
    {
        public FluentJsonSerializer(List<JsonSerializerProfile> profiles)
        {
        }

        public TData Deserialize<TData>(string stringData)
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

        public string Serialize<TData>(TData dataObject)
        {
            throw new NotImplementedException();
        }
    }
}

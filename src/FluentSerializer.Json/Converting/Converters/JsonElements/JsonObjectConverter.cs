using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Json.Dirty;

namespace FluentSerializer.Json.Converting.Converters.JsonElements
{
    public sealed class JsonObjectConverter : IJsonConverter
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Both;

        public bool CanConvert(Type targetType)
        {
            throw new NotImplementedException();
        }

        public JsonWrapper? Serialize(object objectToSerialize, ISerializerContext context)
        {
            throw new NotImplementedException();
        }

        public object? Deserialize(JsonWrapper? objectToDeserialize, ISerializerContext context)
        {
            throw new NotImplementedException();
        }
    }
}

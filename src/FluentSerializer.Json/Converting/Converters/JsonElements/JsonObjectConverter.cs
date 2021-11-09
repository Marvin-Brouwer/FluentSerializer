using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters.JsonElements
{
    // TODO
    public sealed class JsonObjectConverter : IJsonConverter
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Both;

        public bool CanConvert(Type targetType)
        {
            throw new NotImplementedException();
        }

        public IJsonNode? Serialize(object objectToSerialize, ISerializerContext context)
        {
            throw new NotImplementedException();
        }

        public object? Deserialize(IJsonNode objectToDeserialize, ISerializerContext context)
        {
            _ = Object();
            throw new NotImplementedException();
        }
    }
}

using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using Newtonsoft.Json.Linq;

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

        public JToken? Serialize(object objectToSerialize, ISerializerContext context)
        {
            throw new NotImplementedException();
        }

        public object? Deserialize(JToken? objectToDeserialize, ISerializerContext context)
        {
            throw new NotImplementedException();
        }
    }
}

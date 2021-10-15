using System;
using System.Collections.Generic;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Dirty;

namespace FluentSerializer.Json.Converting.Converters
{
    public class CollectionConverter : IJsonConverter
    {
        public virtual SerializerDirection Direction => SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) =>
            !typeof(string).IsAssignableFrom(targetType) &&
            targetType.Implements(typeof(IEnumerable<>));

        public object? Deserialize(JsonWrapper objectToDeserialize, ISerializerContext context)
        {
            throw new NotImplementedException();
        }

        public JsonWrapper Serialize(object objectToSerialize, ISerializerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
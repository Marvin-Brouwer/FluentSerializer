using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Converting
{
    public interface IConverter<TSerialContainer> : IConverter where TSerialContainer : class
    {
        public TSerialContainer? Serialize(object objectToSerialize, ISerializerContext context);
        public object? Deserialize(TSerialContainer objectToDeserialize, ISerializerContext context);
    }

    public interface IConverter
    {
        bool CanConvert(Type targetType);
        SerializerDirection Direction { get; }
    }
}
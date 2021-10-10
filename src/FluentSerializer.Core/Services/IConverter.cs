using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using System;

namespace FluentSerializer.Core.Services
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
using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Core.Converting
{
    public interface IConverter<TSerialContainer> : IConverter where TSerialContainer : IDataNode
    {
        TSerialContainer? Serialize(object? objectToSerialize, ISerializerContext context);
        object? Deserialize(TSerialContainer objectToDeserialize, ISerializerContext context);
    }

    public interface IConverter
    {
        bool CanConvert(Type targetType);
        SerializerDirection Direction { get; }
    }
}
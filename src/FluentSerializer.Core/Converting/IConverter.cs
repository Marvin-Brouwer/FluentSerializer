using System;
using System.Diagnostics.CodeAnalysis;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Core.Converting
{
    public interface IConverter<TSerialContainer> : IConverter where TSerialContainer : IDataNode
    {
		[return: MaybeNull] TSerialContainer? Serialize(object objectToSerialize, ISerializerContext context);

		[return: MaybeNull] object? Deserialize(TSerialContainer objectToDeserialize, ISerializerContext context);
    }

    public interface IConverter
    {
        bool CanConvert(Type targetType);
        SerializerDirection Direction { get; }
    }
}
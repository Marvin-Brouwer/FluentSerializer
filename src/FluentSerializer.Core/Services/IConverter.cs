using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using System.Reflection;

namespace FluentSerializer.Core.Services
{
    public interface IConverter<TSerialContainer> : IConverter where TSerialContainer : class
    {
        public TSerialContainer? Serialize(object objectToSerialize, ISerializerContext context);
        public object? Deserialize(TSerialContainer objectToDeserialize, ISerializerContext context);
    }
    public interface IConverter
    {
        bool CanConvert(PropertyInfo property);
        SerializerDirection Direction { get; }
    }
}
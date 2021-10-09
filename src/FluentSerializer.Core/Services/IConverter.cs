using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using System.Reflection;

namespace FluentSerializer.Core.Services
{
    public interface IConverter<TDestination> : IConverter where TDestination : class
    {
        public TDestination? Serialize(object objectToSerialize, ISerializerContext context);
        public object? Deserialize(TDestination elementToDeserialize, ISerializerContext context);
    }
    public interface IConverter
    {
        bool CanConvert(PropertyInfo property);
        SerializerDirection Direction { get; }
    }
}
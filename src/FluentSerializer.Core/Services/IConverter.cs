using FluentSerializer.Core;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public interface IConverter<TDestination> : IConverter where TDestination : class
    {
        public TDestination? Serialize(TDestination? currentValue, object objectToSerialize, ISerializerContext context);
        public object? Deserialize(object? currentValue, TDestination elementToDeserialize, ISerializerContext context);
    }
    public interface IConverter
    {
        bool CanConvert(PropertyInfo property);
        SerializerDirection Direction { get; }
    }
}
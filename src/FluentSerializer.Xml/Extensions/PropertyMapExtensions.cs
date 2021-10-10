using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Services;
using System.Linq;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Extensions
{
    public static class PropertyMapExtensions
    {
        public static IConverter<TSpecificTarget>? GetMatchingConverter<TSpecificTarget>(
            this IPropertyMap propertyMapping, SerializerDirection direction, ISerializer currentSerializer)
            where TSpecificTarget : XObject
        {
            var converter = propertyMapping.CustomConverter ?? currentSerializer.Configuration.DefaultConverters
                .Where(converter => converter is IConverter<TSpecificTarget>)
                .Where(converter => converter.Direction == SerializerDirection.Both || converter.Direction == direction)
                .FirstOrDefault(converter => converter.CanConvert(propertyMapping.ConcretePropertyType));
            if (converter is null) return null;

            if (!converter.CanConvert(propertyMapping.ConcretePropertyType))
                throw new ConverterNotSupportedException(propertyMapping.Property, converter.GetType(), typeof(TSpecificTarget), direction);
            if (converter is IConverter<TSpecificTarget> specificConverter)
                return specificConverter;

            throw new ConverterNotSupportedException(propertyMapping.Property, converter.GetType(), typeof(TSpecificTarget), direction);
        }
    }
}

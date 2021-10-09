using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Services;
using System;
using System.Linq;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Extensions
{
    public static class PropertyMapExtensions
    {
        public static IConverter<XObject>? GetMatchingConverter<TSpecificTarget>(this IPropertyMap propertyMapping, ISerializer currentSerializer)
            where TSpecificTarget : XObject
        {
            var converter = propertyMapping.CustomConverter ?? currentSerializer.Configuration.DefaultConverters
                .FirstOrDefault(converter => converter.CanConvert(propertyMapping.Property));
            if (converter is null) return null;

            if (!converter.CanConvert(propertyMapping.Property))
                throw new NotSupportedException("Todo custom exception");

            if (propertyMapping.CustomConverter is IConverter<XObject> objectConverter)
                return objectConverter;
            // todo test if possible
            if (propertyMapping.CustomConverter is IConverter<TSpecificTarget> specificConverter)
                return specificConverter as IConverter<XObject> ?? throw new NotSupportedException("Todo custom exception");

            throw new NotSupportedException("Todo custom exception");
        }
    }
}

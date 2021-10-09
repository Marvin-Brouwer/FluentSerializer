using FluentSerializer.Xml.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Extensions
{
    public static class PropertyMapExtensions
    {
        public static IConverter<XObject>? GetMatchingConverter<TSpecificTarget>(this XmlPropertyMap propertyMapping, IXmlSerializer currentSerializer)
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

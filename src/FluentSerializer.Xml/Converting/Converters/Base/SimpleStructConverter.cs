using System;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Xml.Converting.Converters.Base
{
    public abstract class SimpleStructConverter<TObject> : IConverter<XAttribute>, IConverter<XElement>, IConverter<XText>
        where TObject : struct
    {
        public virtual SerializerDirection Direction => SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) => typeof(TObject).IsAssignableFrom(targetType);

        protected abstract string ConvertToString(TObject value);
        protected abstract TObject ConvertToDataType(string currentValue);

        protected virtual TObject? ConvertToNullableDataType(string? currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return ConvertToDataType(currentValue);
        }

        object? IConverter<XAttribute>.Deserialize(XAttribute attributeToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(attributeToDeserialize.Value);
        }

        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(objectToDeserialize.Value);
        }

        object? IConverter<XText>.Deserialize(XText objectToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(objectToDeserialize.Value);
        }

        XAttribute? IConverter<XAttribute>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var value = (TObject)objectToSerialize;
            var attributeName = context.NamingStrategy.SafeGetName(context.Property, context);
            return new XAttribute(attributeName, ConvertToString(value));
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var value = (TObject)objectToSerialize;
            var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
            return new XElement(elementName, ConvertToString(value));
        }

        XText? IConverter<XText>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var value = (TObject)objectToSerialize;
            return new XText(ConvertToString(value));
        }
    }
}
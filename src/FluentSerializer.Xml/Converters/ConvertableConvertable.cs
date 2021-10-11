using System;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;

namespace FluentSerializer.Xml.Converters
{
    public sealed class ConvertibleConverter : IConverter<XAttribute>, IConverter<XElement>, IConverter<XText>
    {
        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(Type targetType) => typeof(IConvertible).IsAssignableFrom(targetType);

        private static string? ConvertToString(object value) => Convert.ToString(value);

        private static object? ConvertToNullableDataType(string? currentValue, Type targetType)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return Convert.ChangeType(currentValue, targetType);
        }

        object? IConverter<XAttribute>.Deserialize(XAttribute attributeToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(attributeToDeserialize.Value, context.PropertyType);
        }

        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(objectToDeserialize.Value, context.PropertyType);
        }

        object? IConverter<XText>.Deserialize(XText objectToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(objectToDeserialize.Value, context.PropertyType);
        }

        XAttribute? IConverter<XAttribute>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var stringValue = ConvertToString(objectToSerialize);
            if (stringValue is null) return null;

            var attributeName = context.NamingStrategy.GetName(context.Property, context);
            return new XAttribute(attributeName, stringValue);
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var stringValue = ConvertToString(objectToSerialize);
            if (stringValue is null) return null;

            var elementName = context.NamingStrategy.GetName(context.Property, context);
            return new XElement(elementName, stringValue);
        }

        XText? IConverter<XText>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var stringValue = ConvertToString(objectToSerialize);
            if (stringValue is null) return null;

            return new XText(stringValue);
        }
    }
}
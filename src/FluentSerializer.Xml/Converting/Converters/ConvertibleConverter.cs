using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Converting.Converters
{
    public sealed class ConvertibleConverter : IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>, IXmlConverter<IXmlText>
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Both;
        public bool CanConvert(Type targetType) => typeof(IConvertible).IsAssignableFrom(targetType);

        private static string? ConvertToString(object value) => Convert.ToString(value);

        private static object? ConvertToNullableDataType(string? currentValue, Type targetType)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return Convert.ChangeType(currentValue, targetType);
        }

        object? IConverter<IXmlAttribute>.Deserialize(IXmlAttribute attributeToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(attributeToDeserialize.Value, context.PropertyType);
        }

        object? IConverter<IXmlElement>.Deserialize(IXmlElement objectToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(objectToDeserialize.GetTextValue(), context.PropertyType);
        }

        object? IConverter<IXmlText>.Deserialize(IXmlText objectToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(objectToDeserialize.Value, context.PropertyType);
        }

        IXmlAttribute? IConverter<IXmlAttribute>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var stringValue = ConvertToString(objectToSerialize);
            if (stringValue is null) return null;

            var attributeName = context.NamingStrategy.SafeGetName(context.Property, context);
            return Attribute(attributeName, stringValue);
        }

        IXmlElement? IConverter<IXmlElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var stringValue = ConvertToString(objectToSerialize);
            if (stringValue is null) return null;

            var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
            return Element(elementName, Text(stringValue));
        }

        IXmlText? IConverter<IXmlText>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var stringValue = ConvertToString(objectToSerialize);
            if (stringValue is null) return null;

            return Text(stringValue);
        }
    }
}
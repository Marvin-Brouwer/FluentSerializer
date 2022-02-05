using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public class StringBitBooleanConverter : IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Both;
        public bool CanConvert(in Type targetType) => typeof(bool).IsAssignableFrom(targetType) 
												   || typeof(bool?).IsAssignableFrom(targetType);

        private static string ConvertToString(in bool currentValue) => currentValue ? "1" : "0";
        private static bool? ConvertToBool(in string? currentValue, in bool? defaultValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return defaultValue;
            if (currentValue.Equals("1", StringComparison.OrdinalIgnoreCase)) return true;
            if (currentValue.Equals("0", StringComparison.OrdinalIgnoreCase)) return false;

            throw new NotSupportedException($"A value of '{currentValue}' is not supported");
        }

        object? IConverter<IXmlAttribute>.Deserialize(in IXmlAttribute attributeToDeserialize, in ISerializerContext context)
        {
            var defaultValue = context.Property.IsNullable() ? default(bool?) : default(bool);
            return ConvertToBool(attributeToDeserialize.Value, defaultValue);
        }

        object? IConverter<IXmlElement>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext context)
        {
            var defaultValue = context.Property.IsNullable() ? default(bool?) : default(bool);
            return ConvertToBool(objectToDeserialize.GetTextValue(), defaultValue);
        }

        IXmlAttribute? IConverter<IXmlAttribute>.Serialize(in object objectToSerialize, in ISerializerContext context)
        {
            var objectBoolean = (bool)objectToSerialize;

            var attributeName = context.NamingStrategy.SafeGetName(context.Property, context);
            var attributeValue = ConvertToString(in objectBoolean);
            return Attribute(in attributeName, in attributeValue);
        }

        IXmlElement? IConverter<IXmlElement>.Serialize(in object objectToSerialize, in ISerializerContext context)
        {
            var objectBoolean = (bool)objectToSerialize;

            var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
            var elementValue = ConvertToString(in objectBoolean);
            return Element(in elementName, Text(in elementValue));
        }
    }
}
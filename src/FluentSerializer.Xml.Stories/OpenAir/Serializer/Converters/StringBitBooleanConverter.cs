using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Converters
{
    public class StringBitBooleanConverter : IConverter<XAttribute>, IConverter<XElement>
    {
        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(PropertyInfo property) => typeof(bool).IsAssignableFrom(property.PropertyType);

        private string ConvertToString(bool currentValue) => currentValue ? "1" : "0";
        private bool ConvertToBool(string? currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;
            if (currentValue.Equals("1", System.StringComparison.OrdinalIgnoreCase)) return true;
            if (currentValue.Equals("0", System.StringComparison.OrdinalIgnoreCase)) return false;

            throw new NotSupportedException($"A value of '{currentValue}' is not supported");
        }

        object? IConverter<XAttribute>.Deserialize(XAttribute attributeToDeserialize, ISerializerContext context)
        {
            return ConvertToBool(attributeToDeserialize.Value);
        }

        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            return ConvertToBool(objectToDeserialize.Value);
        }

        XAttribute? IConverter<XAttribute>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var objectBoolean = (bool?)objectToSerialize ?? default;

            var attributeName = context.NamingStrategy.GetName(context.Property);
            var attributeValue = ConvertToString(objectBoolean);
            return new XAttribute(attributeName, attributeValue);
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var objectBoolean = (bool?)objectToSerialize ?? default;

            var elementName = context.NamingStrategy.GetName(context.Property);
            var elementValue = ConvertToString(objectBoolean);
            return new XElement(elementName, elementValue);
        }
    }
}
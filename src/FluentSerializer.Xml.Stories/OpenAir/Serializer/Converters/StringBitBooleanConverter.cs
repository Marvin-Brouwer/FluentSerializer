using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Profiles;
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

        object? IConverter<XAttribute>.Deserialize(object? currentValue, XAttribute attributeToDeserialize, ISerializerContext context)
        {
            if (currentValue is bool existingBooleanValue)
                return existingBooleanValue && ConvertToBool(attributeToDeserialize.Value);

            return ConvertToBool(attributeToDeserialize.Value);
        }

        object? IConverter<XElement>.Deserialize(object? currentValue, XElement elementToDeserialize, ISerializerContext context)
        {
            if (currentValue is bool existingBooleanValue)
                return existingBooleanValue && ConvertToBool(elementToDeserialize.Value);

            return ConvertToBool(elementToDeserialize.Value);
        }

        XAttribute? IConverter<XAttribute>.Serialize(XAttribute? currentValue, object objectToSerialize, ISerializerContext context)
        {
            var currentBoolean = true;
            if (!string.IsNullOrWhiteSpace(currentValue?.Value))
                currentBoolean = ConvertToBool(currentValue.Value);

            var objectBoolean = (bool?)objectToSerialize ?? default;

            var attributeName = context.NamingStrategy.GetName(context.Property);
            var attributeValue = ConvertToString(currentBoolean && objectBoolean);
            return new XAttribute(attributeName, attributeValue);
        }

        XElement? IConverter<XElement>.Serialize(XElement? currentValue, object objectToSerialize, ISerializerContext context)
        {
            var currentBoolean = true;
            if (!string.IsNullOrWhiteSpace(currentValue?.Value))
                currentBoolean = ConvertToBool(currentValue.Value);

            var objectBoolean = (bool?)objectToSerialize ?? default;

            var elementName = context.NamingStrategy.GetName(context.Property);
            var elementValue = ConvertToString(currentBoolean && objectBoolean);
            return new XElement(elementName, elementValue);
        }
    }
}
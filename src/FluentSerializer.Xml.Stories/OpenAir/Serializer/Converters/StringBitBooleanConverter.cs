using FluentSerializer.Xml.Profiles;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public class StringBitBooleanConverter : ICustomAttributeConverter, ICustomElementConverter
    {
        private readonly SerializerDirection _direction = SerializerDirection.Both;
        SerializerDirection ICustomAttributeConverter.Direction => _direction;
        SerializerDirection ICustomElementConverter.Direction => _direction;

        private bool CanConvert(PropertyInfo property) => typeof(bool).IsAssignableFrom(property.PropertyType);
        bool ICustomAttributeConverter.CanConvert(PropertyInfo property) => CanConvert(property);
        bool ICustomElementConverter.CanConvert(PropertyInfo property) => CanConvert(property);

        private string ConvertToString(bool currentValue) => currentValue ? "1" : "0";
        private bool ConvertToBool(string? currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;
            if (currentValue.Equals("1", System.StringComparison.OrdinalIgnoreCase)) return true;
            if (currentValue.Equals("0", System.StringComparison.OrdinalIgnoreCase)) return false;

            throw new NotSupportedException($"A value of '{currentValue}' is not supported");
        }

        object? ICustomAttributeConverter.Deserialize(object? currentValue, XAttribute attributeToDeserialize, ISerializerContext context)
        {
            if (currentValue is bool existingBooleanValue)
                return existingBooleanValue && ConvertToBool(attributeToDeserialize.Value);

            return ConvertToBool(attributeToDeserialize.Value);
        }

        object? ICustomElementConverter.Deserialize(object? currentValue, XElement elementToDeserialize, ISerializerContext context)
        {
            if (currentValue is bool existingBooleanValue)
                return existingBooleanValue && ConvertToBool(elementToDeserialize.Value);

            return ConvertToBool(elementToDeserialize.Value);
        }

        XAttribute? ICustomAttributeConverter.Serialize(XAttribute? currentValue, object objectToSerialize, ISerializerContext context)
        {
            var currentBoolean = true;
            if (!string.IsNullOrWhiteSpace(currentValue?.Value))
                currentBoolean = ConvertToBool(currentValue.Value);

            var objectBoolean = (bool?)objectToSerialize ?? default;

            var attributeName = context.NamingStrategy.GetName(context.Property);
            var attributeValue = ConvertToString(currentBoolean && objectBoolean);
            return new XAttribute(attributeName, attributeValue);
        }

        XElement? ICustomElementConverter.Serialize(XElement? currentValue, object objectToSerialize, ISerializerContext context)
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
using FluentSerializer.Xml.Profiles;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    internal class StringBitBooleanConverter : ICustomAttributeConverter, ICustomElementConverter
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

        object ICustomAttributeConverter.Deserialize(object? currentValue, XAttribute attributeToDeserialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            if (currentValue is bool existingBooleanValue)
                return existingBooleanValue && ConvertToBool(attributeToDeserialize.Value);

            return ConvertToBool(attributeToDeserialize.Value);
        }

        object ICustomElementConverter.Deserialize(object? currentValue, XElement elementToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            if (currentValue is bool existingBooleanValue)
                return existingBooleanValue && ConvertToBool(elementToSerialize.Value);

            return ConvertToBool(elementToSerialize.Value);
        }

        XAttribute ICustomAttributeConverter.Serialize(XAttribute currentValue, object objectToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            var currentBoolean = true;
            if (!string.IsNullOrWhiteSpace(currentValue.Value))
                currentBoolean = ConvertToBool(currentValue.Value);

            var objectBoolean = (bool?)objectToSerialize ?? default;

            currentValue.Value = ConvertToString(currentBoolean && objectBoolean);

            return currentValue;
        }

        XElement ICustomElementConverter.Serialize(XElement currentValue, object objectToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            var currentBoolean = true;
            if (!string.IsNullOrWhiteSpace(currentValue.Value))
                currentBoolean = ConvertToBool(currentValue.Value);

            var objectBoolean = (bool?)objectToSerialize ?? default;

            currentValue.Value = ConvertToString(currentBoolean && objectBoolean);

            return currentValue;
        }
    }
}
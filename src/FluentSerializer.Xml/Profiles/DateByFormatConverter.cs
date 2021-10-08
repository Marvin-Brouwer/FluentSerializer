using FluentSerializer.Xml.Profiles;
using System;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public sealed class DateByFormatConverter : ICustomAttributeConverter, ICustomElementConverter
    {
        private readonly string _format;
        private readonly CultureInfo _cultureInfo;
        private readonly DateTimeStyles _dateTimeStyle;

        public DateByFormatConverter(string format, CultureInfo cultureInfo, DateTimeStyles dateTimeStyle)
        {
            _format = format;
            _cultureInfo = cultureInfo;
            _dateTimeStyle = dateTimeStyle;
        }

        private readonly SerializerDirection _direction = SerializerDirection.Both;
        SerializerDirection ICustomAttributeConverter.Direction => _direction;
        SerializerDirection ICustomElementConverter.Direction => _direction;

        private bool CanConvert(PropertyInfo property) => typeof(bool).IsAssignableFrom(property.PropertyType);
        bool ICustomAttributeConverter.CanConvert(PropertyInfo property) => CanConvert(property);
        bool ICustomElementConverter.CanConvert(PropertyInfo property) => CanConvert(property);

        private string ConvertToString(DateTime currentValue) => currentValue.ToString(_format, _cultureInfo);
        private DateTime ConvertToDateTime(string? currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return DateTime.ParseExact(currentValue, _format, _cultureInfo, _dateTimeStyle);
        }

        object? ICustomAttributeConverter.Deserialize(object? currentValue, XAttribute attributeToDeserialize, ISerializerContext context)
        {
            return ConvertToDateTime(attributeToDeserialize.Value);
        }

        object? ICustomElementConverter.Deserialize(object? currentValue, XElement elementToDeserialize, ISerializerContext context)
        {
            return ConvertToDateTime(elementToDeserialize.Value);
        }

        XAttribute? ICustomAttributeConverter.Serialize(XAttribute? currentValue, object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize == null) return null;

            var dateValue = (DateTime)objectToSerialize;
            var attributeName = context.NamingStrategy.GetName(context.Property);
            return new XAttribute(attributeName, ConvertToString(dateValue));
        }

        XElement? ICustomElementConverter.Serialize(XElement? currentValue, object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize == null) return null;

            var dateValue = (DateTime)objectToSerialize;
            var elementName = context.NamingStrategy.GetName(context.Property);
            return new XElement(elementName, ConvertToString(dateValue));
        }
    }
}
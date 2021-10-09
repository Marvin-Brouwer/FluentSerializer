using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using System;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public sealed class DateByFormatConverter : IConverter<XAttribute>, IConverter<XElement>
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

        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(PropertyInfo property) => typeof(bool).IsAssignableFrom(property.PropertyType);

        private string ConvertToString(DateTime currentValue) => currentValue.ToString(_format, _cultureInfo);
        private DateTime ConvertToDateTime(string? currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return DateTime.ParseExact(currentValue, _format, _cultureInfo, _dateTimeStyle);
        }

        object? IConverter<XAttribute>.Deserialize(object? currentValue, XAttribute attributeToDeserialize, ISerializerContext context)
        {
            return ConvertToDateTime(attributeToDeserialize.Value);
        }

        object? IConverter<XElement>.Deserialize(object? currentValue, XElement elementToDeserialize, ISerializerContext context)
        {
            return ConvertToDateTime(elementToDeserialize.Value);
        }

        XAttribute? IConverter<XAttribute>.Serialize(XAttribute? currentValue, object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize == null) return null;

            var dateValue = (DateTime)objectToSerialize;
            var attributeName = context.NamingStrategy.GetName(context.Property);
            return new XAttribute(attributeName, ConvertToString(dateValue));
        }

        XElement? IConverter<XElement>.Serialize(XElement? currentValue, object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize == null) return null;

            var dateValue = (DateTime)objectToSerialize;
            var elementName = context.NamingStrategy.GetName(context.Property);
            return new XElement(elementName, ConvertToString(dateValue));
        }
    }
}
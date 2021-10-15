using System;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public class StringBitBooleanConverter : IConverter<XAttribute>, IConverter<XElement>
    {
        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(Type targetType) => typeof(bool).IsAssignableFrom(targetType);

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

            var attributeName = context.NamingStrategy.GetName(context.Property, context);
            var attributeValue = ConvertToString(objectBoolean);
            return new XAttribute(attributeName, attributeValue);
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            var objectBoolean = (bool?)objectToSerialize ?? default;

            var elementName = context.NamingStrategy.GetName(context.Property, context);
            var elementValue = ConvertToString(objectBoolean);
            return new XElement(elementName, elementValue);
        }
    }
}
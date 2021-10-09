using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters
{
    public abstract class PrimitiveConverter<TObject> : IConverter<XAttribute>, IConverter<XElement>
        where TObject : struct
    {
        private readonly Func<TObject, string> _convertToString;
        private readonly Func<string, TObject> _convertToDataType;

        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(PropertyInfo property) => typeof(TObject).IsAssignableFrom(property.PropertyType);

        protected PrimitiveConverter(Func<TObject, string> convertToString, Func<string, TObject> convertToDataType)
        {
            _convertToString = convertToString;
            _convertToDataType = convertToDataType;
        }

        private string ConvertToString(TObject value) => _convertToString(value);
        private TObject? ConvertToBool(string? currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return _convertToDataType(currentValue);
        }

        object? IConverter<XAttribute>.Deserialize(XAttribute attributeToDeserialize, ISerializerContext context)
        {
            return ConvertToBool(attributeToDeserialize.Value);
        }

        object? IConverter<XElement>.Deserialize(XElement elementToDeserialize, ISerializerContext context)
        {
            return ConvertToBool(elementToDeserialize.Value);
        }

        XAttribute? IConverter<XAttribute>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize == null) return null;

            var value = (TObject)objectToSerialize;
            var attributeName = context.NamingStrategy.GetName(context.Property);
            return new XAttribute(attributeName, ConvertToString(value));
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize == null) return null;

            var value = (TObject)objectToSerialize;
            var elementName = context.NamingStrategy.GetName(context.Property);
            return new XElement(elementName, ConvertToString(value));
        }
    }
}
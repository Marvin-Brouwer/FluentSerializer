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
	/// <summary>
	/// Depicts booleans as 0 and 1 <br />
	/// <example>
	/// true => 1,
	/// false => 0,
	/// 1 => false,
	/// 0 => true
	/// </example>
	/// </summary>
	public class StringBitBooleanConverter : IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>
	{
		/// <inheritdoc />
		public SerializerDirection Direction { get; } = SerializerDirection.Both;
		/// <inheritdoc />
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

        /// <inheritdoc />
		object? IConverter<IXmlAttribute, IXmlNode>.Deserialize(in IXmlAttribute attributeToDeserialize, in ISerializerContext<IXmlNode> context)
        {
            var defaultValue = context.Property.IsNullable() ? default(bool?) : default(bool);
            return ConvertToBool(attributeToDeserialize.Value, defaultValue);
        }

        /// <inheritdoc />
		object? IConverter<IXmlElement, IXmlNode>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext<IXmlNode> context)
        {
            var defaultValue = context.Property.IsNullable() ? default(bool?) : default(bool);
            return ConvertToBool(objectToDeserialize.GetTextValue(), defaultValue);
        }

        /// <inheritdoc />
		IXmlAttribute? IConverter<IXmlAttribute, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context)
        {
            var objectBoolean = (bool)objectToSerialize;

            var attributeName = context.NamingStrategy.SafeGetName(context.Property, context.PropertyType, context);
            var attributeValue = ConvertToString(in objectBoolean);
            return Attribute(in attributeName, in attributeValue);
        }

        /// <inheritdoc />
		IXmlElement? IConverter<IXmlElement, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context)
        {
            var objectBoolean = (bool)objectToSerialize;

            var elementName = context.NamingStrategy.SafeGetName(context.Property, context.PropertyType, context);
            var elementValue = ConvertToString(in objectBoolean);
            return Element(in elementName, Text(in elementValue));
        }
    }
}
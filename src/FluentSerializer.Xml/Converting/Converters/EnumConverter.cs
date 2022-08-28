using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Converting.Converters;

/// <inheritdoc cref="EnumConverter(in EnumFormats)" />
public sealed class EnumConverter : EnumConverterBase, IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>, IXmlConverter<IXmlText>
{
	/// <summary>
	/// Converter for <c>enum</c>s with specialized settings
	/// </summary>
	/// <paramref name="enumFormat">The format to use when reading and writing serialized <c>enum</c> values</paramref>
	public EnumConverter(in EnumFormats enumFormat) : base(enumFormat, null) { }

	object? IConverter<IXmlAttribute, IXmlNode>.Deserialize(in IXmlAttribute attributeToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		return ConvertToEnum(attributeToDeserialize.Value, context.PropertyType);
	}

	object? IConverter<IXmlElement, IXmlNode>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		return ConvertToEnum(objectToDeserialize.GetTextValue(), context.PropertyType);
	}

	object? IConverter<IXmlText, IXmlNode>.Deserialize(in IXmlText objectToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		return ConvertToEnum(objectToDeserialize.Value, context.PropertyType);
	}

	IXmlAttribute? IConverter<IXmlAttribute, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize, context.PropertyType)?.value;

		var attributeName = context.NamingStrategy.SafeGetName(context.Property, context.PropertyType, context);
		return Attribute(attributeName, stringValue);
	}

	IXmlElement? IConverter<IXmlElement, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize, context.PropertyType)?.value;

		var elementName = context.NamingStrategy.SafeGetName(context.Property, context.PropertyType, context);
		return Element(in elementName, Text(in stringValue));
	}

	IXmlText? IConverter<IXmlText, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize, context.PropertyType)?.value;

		return Text(in stringValue);
	}
}
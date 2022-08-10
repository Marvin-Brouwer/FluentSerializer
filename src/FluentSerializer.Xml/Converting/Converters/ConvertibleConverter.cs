using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;

using System;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IConvertible"/>
/// </summary>
public sealed class ConvertibleConverter : ConvertibleConverterBase, IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>, IXmlConverter<IXmlText>
{
	object? IConverter<IXmlAttribute, IXmlNode>.Deserialize(in IXmlAttribute attributeToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		return ConvertToNullableDataType(attributeToDeserialize.Value, context.PropertyType);
	}

	object? IConverter<IXmlElement, IXmlNode>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		return ConvertToNullableDataType(objectToDeserialize.GetTextValue(), context.PropertyType);
	}

	object? IConverter<IXmlText, IXmlNode>.Deserialize(in IXmlText objectToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		return ConvertToNullableDataType(objectToDeserialize.Value, context.PropertyType);
	}

	IXmlAttribute? IConverter<IXmlAttribute, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize);

		var attributeName = context.NamingStrategy.SafeGetName(context.Property, context.PropertyType, context);
		return Attribute(attributeName, stringValue);
	}

	IXmlElement? IConverter<IXmlElement, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize);

		var elementName = context.NamingStrategy.SafeGetName(context.Property, context.PropertyType, context);
		return Element(in elementName, Text(in stringValue));
	}

	IXmlText? IConverter<IXmlText, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize);

		return Text(in stringValue);
	}
}
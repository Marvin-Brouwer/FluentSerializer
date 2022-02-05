using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IConvertible"/>
/// </summary>
public sealed class ConvertibleConverter : IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>, IXmlConverter<IXmlText>
{
	/// <inheritdoc />
	public SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => typeof(IConvertible).IsAssignableFrom(targetType);

	private static string ConvertToString(in object value) => Convert.ToString(value);

	private static object? ConvertToNullableDataType(in string? currentValue, in Type targetType)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return Convert.ChangeType(currentValue, targetType);
	}

	object? IConverter<IXmlAttribute>.Deserialize(in IXmlAttribute attributeToDeserialize, in ISerializerContext context)
	{
		return ConvertToNullableDataType(attributeToDeserialize.Value, context.PropertyType);
	}

	object? IConverter<IXmlElement>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext context)
	{
		return ConvertToNullableDataType(objectToDeserialize.GetTextValue(), context.PropertyType);
	}

	object? IConverter<IXmlText>.Deserialize(in IXmlText objectToDeserialize, in ISerializerContext context)
	{
		return ConvertToNullableDataType(objectToDeserialize.Value, context.PropertyType);
	}

	IXmlAttribute? IConverter<IXmlAttribute>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize);

		var attributeName = context.NamingStrategy.SafeGetName(context.Property, context);
		return Attribute(attributeName, stringValue);
	}

	IXmlElement? IConverter<IXmlElement>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize);

		var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
		return Element(in elementName, Text(in stringValue));
	}

	IXmlText? IConverter<IXmlText>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var stringValue = ConvertToString(in objectToSerialize);

		return Text(in stringValue);
	}
}
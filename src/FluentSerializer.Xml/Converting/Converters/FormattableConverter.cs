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
public sealed class FormattableConverter : FormattableConverterBase, IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>, IXmlConverter<IXmlText>
{
	/// <inheritdoc cref="ConvertibleConverter"/>
	public FormattableConverter(in string? format, in IFormatProvider? formatProvider) : base(in format, in formatProvider) { }

	private static object ThrowDeserializeNotSupported() => throw new NotSupportedException("This is a Serialize only converter.");

	object? IConverter<IXmlAttribute, IXmlNode>.Deserialize(in IXmlAttribute attributeToDeserialize, in ISerializerContext<IXmlNode> context) =>
		ThrowDeserializeNotSupported();
	object? IConverter<IXmlElement, IXmlNode>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext<IXmlNode> context) =>
		ThrowDeserializeNotSupported();
	object? IConverter<IXmlText, IXmlNode>.Deserialize(in IXmlText objectToDeserialize, in ISerializerContext<IXmlNode> context) =>
		ThrowDeserializeNotSupported();

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
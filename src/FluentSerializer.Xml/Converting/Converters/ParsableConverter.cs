#if NET7_0_OR_GREATER
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;

using System;
using System.Globalization;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IParsable{TSelf}"/>
/// </summary>
public sealed class ParsableConverter : ParsableConverterBase, IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>, IXmlConverter<IXmlText>
{
	/// <inheritdoc cref="ParsableConverter"/>
	public ParsableConverter(in bool tryParse, in IFormatProvider? formatProvider) : base(in tryParse, in formatProvider) { }

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

	IXmlAttribute? IConverter<IXmlAttribute, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException("This is a Deserialize only converter.");

	IXmlElement? IConverter<IXmlElement, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException("This is a Deserialize only converter.");

	IXmlText? IConverter<IXmlText, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException("This is a Deserialize only converter.");
}
#endif
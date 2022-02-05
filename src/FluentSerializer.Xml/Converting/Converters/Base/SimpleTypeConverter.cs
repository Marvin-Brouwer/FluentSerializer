using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Converting.Converters.Base;

/// <summary>
/// This class contains methods that are used when converting simple types
/// </summary>
public abstract class SimpleTypeConverter<TObject> : IXmlConverter<IXmlAttribute>, IXmlConverter<IXmlElement>, IXmlConverter<IXmlText>
{
	/// <inheritdoc />
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public virtual bool CanConvert(in Type targetType) => typeof(TObject).IsAssignableFrom(targetType);

	/// <summary>
	/// Abstract placeholder for converting to string logic
	/// </summary>
	protected abstract string ConvertToString(in TObject value);
	/// <summary>
	/// Abstract placeholder for converting to object logic
	/// </summary>
	protected abstract TObject ConvertToDataType(in string currentValue);

	/// <summary>
	/// Wrapper around <see cref="ConvertToDataType(in string)"/> to support nullable values
	/// </summary>
	protected virtual TObject? ConvertToNullableDataType(in string? currentValue)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return ConvertToDataType(in currentValue);
	}

	object? IConverter<IXmlAttribute>.Deserialize(in IXmlAttribute attributeToDeserialize, in ISerializerContext context)
	{
		return ConvertToNullableDataType(attributeToDeserialize.Value);
	}

	object? IConverter<IXmlElement>.Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext context)
	{
		return ConvertToNullableDataType(objectToDeserialize.GetTextValue());
	}

	object? IConverter<IXmlText>.Deserialize(in IXmlText objectToDeserialize, in ISerializerContext context)
	{
		return ConvertToNullableDataType(objectToDeserialize.Value);
	}

	IXmlAttribute? IConverter<IXmlAttribute>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		var attributeName = context.NamingStrategy.SafeGetName(context.Property, context);
		return Attribute(in attributeName, ConvertToString(in value));
	}

	IXmlElement? IConverter<IXmlElement>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
		return Element(elementName, Text(ConvertToString(in value)));
	}

	IXmlText? IConverter<IXmlText>.Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		return Text(ConvertToString(in value));
	}
}
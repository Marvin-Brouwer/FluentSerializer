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
	public virtual bool CanConvert(Type targetType) => typeof(TObject).IsAssignableFrom(targetType);

	/// <summary>
	/// Abstract placeholder for converting to string logic
	/// </summary>
	protected abstract string ConvertToString(TObject value);
	/// <summary>
	/// Abstract placeholder for converting to object logic
	/// </summary>
	protected abstract TObject ConvertToDataType(string currentValue);

	/// <summary>
	/// Wrapper around <see cref="ConvertToDataType(string)"/> to support nullable values
	/// </summary>
	protected virtual TObject? ConvertToNullableDataType(string? currentValue)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return ConvertToDataType(currentValue);
	}

	object? IConverter<IXmlAttribute>.Deserialize(IXmlAttribute attributeToDeserialize, ISerializerContext context)
	{
		return ConvertToNullableDataType(attributeToDeserialize.Value);
	}

	object? IConverter<IXmlElement>.Deserialize(IXmlElement objectToDeserialize, ISerializerContext context)
	{
		return ConvertToNullableDataType(objectToDeserialize.GetTextValue());
	}

	object? IConverter<IXmlText>.Deserialize(IXmlText objectToDeserialize, ISerializerContext context)
	{
		return ConvertToNullableDataType(objectToDeserialize.Value);
	}

	IXmlAttribute? IConverter<IXmlAttribute>.Serialize(object objectToSerialize, ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		var attributeName = context.NamingStrategy.SafeGetName(context.Property, context);
		return Attribute(attributeName, ConvertToString(value));
	}

	IXmlElement? IConverter<IXmlElement>.Serialize(object objectToSerialize, ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
		return Element(elementName, Text(ConvertToString(value)));
	}

	IXmlText? IConverter<IXmlText>.Serialize(object objectToSerialize, ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		return Text(ConvertToString(value));
	}
}
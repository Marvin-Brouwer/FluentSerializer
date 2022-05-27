using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System;

namespace FluentSerializer.Xml.Converter.DefaultXml.Converting.Converters;

/// <summary>
/// Converts any raw <see cref="IXmlNode"/>
/// </summary>
public sealed class XmlNodeConverter : IXmlConverter<IXmlElement>
{
	/// <inheritdoc />
	public SerializerDirection Direction { get; } = SerializerDirection.Both;

	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => typeof(IXmlNode).IsAssignableFrom(targetType);

	/// <inheritdoc />
	public int ConverterHashCode { get; } = typeof(IXmlNode).GetHashCode();

	/// <inheritdoc />
	public object? Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext<IXmlNode> context)
	{
		if (objectToDeserialize.GetType().IsAssignableFrom(context.PropertyType))
			throw new NotSupportedException($"Type of '${objectToDeserialize.GetType().FullName}' is not assignable to {context.PropertyType}");

		return objectToDeserialize;
	}

	/// <inheritdoc />
	public IXmlElement? Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		if (objectToSerialize is IXmlNode element) return new XmlFragment(element);

		throw new NotSupportedException($"Type of '${objectToSerialize.GetType().FullName}' could not be converted");
	}
}
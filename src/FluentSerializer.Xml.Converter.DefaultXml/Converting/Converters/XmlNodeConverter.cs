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
	public bool CanConvert(Type targetType) => typeof(IXmlNode).IsAssignableFrom(targetType);

	/// <inheritdoc />
	public object? Deserialize(IXmlElement objectToDeserialize, ISerializerContext context)
	{
		if (context.PropertyType.IsInstanceOfType(objectToDeserialize)) 
			throw new NotSupportedException($"Type of '${objectToDeserialize.GetType().FullName}' is not assignable to {context.PropertyType}");

		return objectToDeserialize;
	}

	/// <inheritdoc />
	public IXmlElement? Serialize(object objectToSerialize, ISerializerContext context)
	{
		if (objectToSerialize is IXmlNode element) return new XmlFragment(element);

		throw new NotSupportedException($"Type of '${objectToSerialize.GetType().FullName}' could not be converted");
	}
}
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converting;

/// <summary>
/// A more specific interface for <see cref="IXmlConverter"/> with an overload to allow for parent access
/// </summary>
public interface IXmlConverter<TDataContainer> : IConverter<TDataContainer>, IXmlConverter
	where TDataContainer : IXmlNode
{
	/// <inheritdoc cref="IConverter{TDataContainer}.Deserialize(TDataContainer, ISerializerContext)"/>
	object? Deserialize(TDataContainer objectToDeserialize, IXmlElement? parent, ISerializerContext context) => 
		Deserialize(objectToDeserialize, context);
}

/// <inheritdoc />
public interface IXmlConverter : IConverter
{
}
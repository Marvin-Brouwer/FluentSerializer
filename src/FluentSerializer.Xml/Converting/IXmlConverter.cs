using FluentSerializer.Core.Converting;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converting;

/// <summary>
/// A more specific interface for <see cref="IXmlConverter"/> allowing you to specify the type of Node to convert.
/// This is useful because a attribute mapping won't accept an <see cref="IXmlConverter{TDataContainer}"/> of anything other than
/// <see cref="IXmlAttribute"/> and the same counts for the other property mapping types.
/// </summary>
public interface IXmlConverter<TDataContainer> : IConverter<TDataContainer, IXmlNode>, IXmlConverter
	where TDataContainer : IXmlNode
{
}

/// <inheritdoc />
public interface IXmlConverter : IConverter
{
}
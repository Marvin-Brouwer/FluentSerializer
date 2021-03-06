using FluentSerializer.Xml.Converter.DefaultXml.Converting.Converters;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converter.DefaultXml.Converting.Extensions;

/// <summary>
/// Extensions to allow for <see cref="IUseXmlConverters"/> to point to an <see cref="IXmlConverter"/>
/// </summary>
public static class ConverterExtensions
{
	private static readonly IXmlConverter<IXmlElement> DefaultXmlConverter = new XmlNodeConverter();

	/// <inheritdoc cref="DefaultXmlConverter" />
	public static IXmlConverter<IXmlElement> Xml(this IUseXmlConverters _) => DefaultXmlConverter;
}
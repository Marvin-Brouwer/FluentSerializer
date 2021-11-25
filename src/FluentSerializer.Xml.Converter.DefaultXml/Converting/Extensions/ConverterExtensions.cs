using FluentSerializer.Xml.Converter.DefaultXml.Converting.Converters;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converter.DefaultXml.Converting.Extensions
{
    public static class ConverterExtensions
    {
        private static readonly IXmlConverter<IXmlElement> DefaultXmlConverter = new XmlNodeConverter();
        public static IXmlConverter<IXmlElement> Xml(this IUseXmlConverters _) => DefaultXmlConverter;
    }
}

using FluentSerializer.Core.Configuration;
using FluentSerializer.Xml.Converters;
using FluentSerializer.Xml.Converters.XNodes;

namespace FluentSerializer.Xml
{
    public static class ConfigurationConstants
    {
        public static SerializerConfiguration GetDefaultXmlConfiguration()
        {
            return new SerializerConfiguration
            {
                FormatOutput = true,
                DefaultConverters =
                {
                    // Primitives
                    new IntegerConverter(),
                    new BooleanConverter(),
                    // TODO add prmiitive converters, date converter, raw string converter and enumerable converter,
                    new DefaultDateConverter(),
                    new StringConverter(),

                    // Collection converters

                    // Spcial XNode types
                    new XCDataConverter(),
                    new XCommentConverter()
                }
            };
        }
    }
}

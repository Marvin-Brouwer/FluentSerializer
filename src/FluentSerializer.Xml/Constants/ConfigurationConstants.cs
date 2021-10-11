using FluentSerializer.Core.Configuration;
using FluentSerializer.Xml.Converters;
using FluentSerializer.Xml.Converters.XNodes;

namespace FluentSerializer.Xml.Constants
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
                    new DefaultDateConverter(),
                    new ConvertibleConverter(),

                    // Collection converters
                    new WrappedCollectionConverter(),

                    // Special XNode types
                    new XObjectConverter()
                }
            };
        }
    }
}

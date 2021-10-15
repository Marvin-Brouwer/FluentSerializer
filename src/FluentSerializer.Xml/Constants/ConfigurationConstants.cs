using FluentSerializer.Core.Configuration;
using FluentSerializer.Xml.Converting;

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
                    UseXmlConverters.DefaultDateConverter,
                    UseXmlConverters.ConvertibleConverter,

                    // Collection converters
                    UseXmlConverters.WrappedCollectionConverter,

                    // Special XNode types
                    UseXmlConverters.XObjectConverter
                }
            };
        }
    }
}

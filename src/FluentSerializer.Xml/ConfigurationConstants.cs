using FluentSerializer.Core.Configuration;

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
                    // todo add converters here
                }
            };
        }
    }
}

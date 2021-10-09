using FluentSerializer.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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

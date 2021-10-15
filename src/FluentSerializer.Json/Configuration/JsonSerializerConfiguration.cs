using System;
using System.Collections.Generic;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.Json.Configuration
{
    public sealed class JsonSerializerConfiguration : SerializerConfiguration
    {
        public static JsonSerializerConfiguration Default { get; } = new JsonSerializerConfiguration();

        public Func<INamingStrategy> DefaultNamingStrategy { get; set; }

        private JsonSerializerConfiguration()
        {
            FormatOutput = true;
            DefaultNamingStrategy = Names.Use.CamelCase;
            DefaultConverters = new List<IConverter>
            {
                UseJsonConverters.DefaultDateConverter,
                UseJsonConverters.ConvertibleConverter,

                // Collection converters
                UseJsonConverters.CollectionConverter,

                // Special JsonObject types
                UseJsonConverters.JsonObjectConverter
            };
        }
    }
}

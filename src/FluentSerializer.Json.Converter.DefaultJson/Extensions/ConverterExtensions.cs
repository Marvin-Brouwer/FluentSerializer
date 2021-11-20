using FluentSerializer.Json.Converter.DefaultJson.Converting.Converters;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.Json.Converter.DefaultJson.Extensions
{
    public static class ConverterExtensions
    {
        private static readonly IJsonConverter DefaultJsonConverter = new JsonNodeConverter();
        public static IJsonConverter Json(this IUseJsonConverters _) => DefaultJsonConverter;
    }
}

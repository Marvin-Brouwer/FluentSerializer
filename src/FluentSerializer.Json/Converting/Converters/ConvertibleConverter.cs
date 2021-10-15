using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using Newtonsoft.Json.Linq;

namespace FluentSerializer.Json.Converting.Converters
{
    public sealed class ConvertibleConverter : IJsonConverter
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Both;
        public bool CanConvert(Type targetType) => typeof(IConvertible).IsAssignableFrom(targetType);

        private static string? ConvertToString(object value) => Convert.ToString(value);

        private static object? ConvertToNullableDataType(string? currentValue, Type targetType)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return Convert.ChangeType(currentValue, targetType);
        }

        public JToken? Serialize(object objectToSerialize, ISerializerContext context)
        {
            var stringValue = ConvertToString(objectToSerialize);
            if (string.IsNullOrWhiteSpace(stringValue)) return new JObject(JValue.CreateNull());

            if (context.PropertyType == typeof(string)) return JValue.CreateString(stringValue);
            if (context.PropertyType.IsClass) return JObject.FromObject(stringValue);

            return JToken.FromObject(stringValue);
        }

        public object? Deserialize(JToken objectToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(objectToDeserialize.ToString(), context.PropertyType);
        }
    }
}
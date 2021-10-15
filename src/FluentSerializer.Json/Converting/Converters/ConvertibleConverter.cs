using System;
using System.Text.Json;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Json.Dirty;

namespace FluentSerializer.Json.Converting.Converters
{
    public sealed class ConvertibleConverter : IJsonConverter
    {
        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(Type targetType) => typeof(IConvertible).IsAssignableFrom(targetType);

        private static string? ConvertToString(object value) => Convert.ToString(value);

        private static object? ConvertToNullableDataType(string? currentValue, Type targetType)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return Convert.ChangeType(currentValue, targetType);
        }

        public JsonWrapper? Serialize(object objectToSerialize, ISerializerContext context)
        {
            var stringValue = ConvertToString(objectToSerialize);
            if (stringValue is null) return null;

            return new JsonWrapper(JsonDocument.Parse(stringValue).RootElement);
        }

        public object? Deserialize(JsonWrapper objectToDeserialize, ISerializerContext context)
        {
            return ConvertToNullableDataType(objectToDeserialize.ToString(), context.ClassType);
        }
    }
}
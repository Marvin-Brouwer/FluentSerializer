using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;
using static FluentSerializer.Json.JsonBuilder;

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

        public IJsonNode? Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize is string stringToSerialize)
                return Value($"\"{stringToSerialize}\"");

            var stringValue = ConvertToString(objectToSerialize);

            return stringValue is null 
                ? null 
                : Value(stringValue);
        }

        public object? Deserialize(IJsonNode objectToDeserialize, ISerializerContext context)
        {
            if (objectToDeserialize is not IDataValue data) return default;
            if (context.PropertyType == typeof(string) && data.Value?.Length > 2)
                return data.Value[1..^1];

            return ConvertToNullableDataType(data.Value, context.PropertyType);
        }
    }
}
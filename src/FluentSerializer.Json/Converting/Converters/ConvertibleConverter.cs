using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;
using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IConvertible"/>
/// </summary>
public sealed class ConvertibleConverter : IJsonConverter
{
	/// <inheritdoc />
	public SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => typeof(IConvertible).IsAssignableFrom(targetType);

	private static string? ConvertToString(in object value) => Convert.ToString(value);

	private static object? ConvertToNullableDataType(in string? currentValue, in Type targetType)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return Convert.ChangeType(currentValue, targetType);
	}

	/// <inheritdoc />
	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		if (objectToSerialize is string stringToSerialize)
			return Value($"\"{stringToSerialize}\"");

		var stringValue = ConvertToString(objectToSerialize);

		return stringValue is null 
			? null 
			: Value(in stringValue);
	}

	/// <inheritdoc />
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IDataValue data) return default;
		if (context.PropertyType == typeof(string) && data.Value?.Length > 2)
			return data.Value[1..^1];

		return ConvertToNullableDataType(data.Value, context.PropertyType);
	}
}
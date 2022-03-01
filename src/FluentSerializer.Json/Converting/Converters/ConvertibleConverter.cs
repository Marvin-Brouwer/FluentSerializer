using System;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IConvertible"/>
/// </summary>
public sealed class ConvertibleConverter : ConvertibleConverterBase, IJsonConverter
{
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
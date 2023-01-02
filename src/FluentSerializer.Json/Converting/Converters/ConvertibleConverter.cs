using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;

using System;
using System.Data;
using System.Globalization;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IConvertible"/>
/// </summary>
public sealed class ConvertibleConverter : ConvertibleConverterBase, IJsonConverter
{
	/// <inheritdoc cref="ConvertibleConverter"/>
	public ConvertibleConverter() : this(null) { }

	/// <inheritdoc cref="ConvertibleConverter"/>
	public ConvertibleConverter(CultureInfo? formatProvider) : base(formatProvider) { }

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

		if (context.PropertyType == typeof(string)) return HandleStringData(in data);

		return ConvertToNullableDataType(data.Value, context.PropertyType);
	}

	/// <remarks>
	/// The <see cref="ConvertibleConverterBase.ConvertToNullableDataType"/> can probably handle string very well.
	/// However, because of how the quotes work in JSON, we'd like some more control when it comes to strings.
	/// </remarks>
	private static string? HandleStringData(in IDataValue data)
	{
		if (data.Value is null || data.Value.Length == 0) return default;
		if (data.Value?.Length >= 2) return data.Value[1..^1];

		throw new DataException("A string type cannot be smaller than 2 since it should be surrounded in quotes");
	}
}
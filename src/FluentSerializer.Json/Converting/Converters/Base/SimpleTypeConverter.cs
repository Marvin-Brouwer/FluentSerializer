using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters.Base;

/// <summary>
/// This class contains methods that are used when converting simple types
/// </summary>
public abstract class SimpleTypeConverter<TObject> : IJsonConverter
{
	/// <inheritdoc />
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public virtual bool CanConvert(Type targetType) => typeof(TObject).IsAssignableFrom(targetType);

	/// <summary>
	/// Abstract placeholder for converting to string logic
	/// </summary>
	protected abstract string ConvertToString(TObject value);
	/// <summary>
	/// Abstract placeholder for converting to object logic
	/// </summary>
	protected abstract TObject ConvertToDataType(string currentValue);

	/// <summary>
	/// Wrapper around <see cref="ConvertToDataType(string)"/> to support nullable values
	/// </summary>
	protected virtual TObject? ConvertToNullableDataType(string? currentValue)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return ConvertToDataType(currentValue);
	}

	/// <inheritdoc />
	public object? Deserialize(IJsonNode objectToDeserialize, ISerializerContext context)
	{
		if (objectToDeserialize is not IDataValue dataValue) return default;

		var stringValue = dataValue.Value;
		return ConvertToNullableDataType(stringValue);
	}

	/// <inheritdoc />
	public IJsonNode? Serialize(object objectToSerialize, ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		var stringValue = ConvertToString(value);

		return Value(stringValue);
	}
}
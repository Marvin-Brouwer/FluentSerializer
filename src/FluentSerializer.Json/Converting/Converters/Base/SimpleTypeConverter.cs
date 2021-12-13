using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters.Base;

public abstract class SimpleTypeConverter<TObject> : IJsonConverter
{
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
	public virtual bool CanConvert(Type targetType) => typeof(TObject).IsAssignableFrom(targetType);

	protected abstract string ConvertToString(TObject value);
	protected abstract TObject ConvertToDataType(string currentValue);

	protected virtual TObject? ConvertToNullableDataType(string? currentValue)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return ConvertToDataType(currentValue);
	}
        
	public object? Deserialize(IJsonNode objectToDeserialize, ISerializerContext context)
	{
		if (objectToDeserialize is not IDataValue dataValue) return default;

		var stringValue = dataValue.Value;
		return ConvertToNullableDataType(stringValue);
	}

	public IJsonNode? Serialize(object objectToSerialize, ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		var stringValue = ConvertToString(value);

		return Value(stringValue);
	}
}
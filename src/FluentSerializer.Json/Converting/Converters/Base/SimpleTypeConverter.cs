using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters.Base;

/// <summary>
/// This class contains methods that are used when converting simple types
/// </summary>
public abstract class SimpleTypeConverter<TObject> : SimpleTypeConverterBase<TObject>, IJsonConverter
{
	/// <inheritdoc />
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IDataValue dataValue) return default;

		var stringValue = dataValue.Value;
		return ConvertToNullableDataType(in stringValue);
	}

	/// <inheritdoc />
	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var value = (TObject)objectToSerialize;
		var stringValue = ConvertToString(in value);

		return Value(in stringValue);
	}
}
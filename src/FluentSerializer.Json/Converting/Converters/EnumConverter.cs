#if NETSTANDARD2_0
using FluentSerializer.Core.Dirty.BackwardsCompatibility.NetFramework;
#endif
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters;

/// <inheritdoc cref="EnumConverter(in EnumFormats, in bool)" />
public sealed class EnumConverter : EnumConverterBase, IJsonConverter
{
	private readonly bool _writeNumbersAsString;

	/// <summary>
	/// Converter for <c>enum</c>s with specialized settings
	/// </summary>
	/// <paramref name="enumFormat">The format to use when reading and writing serialized <c>enum</c> values</paramref>
	/// <paramref name="writeNumbersAsString">Configure whether to wrap numbers in quotes ("")</paramref>
	public EnumConverter(in EnumFormats enumFormat, in bool writeNumbersAsString) : base(enumFormat, null)
	{
		_writeNumbersAsString = writeNumbersAsString;
	}

	/// <inheritdoc />
	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		var convertedValue = ConvertToString(objectToSerialize, context.PropertyType);

		if (convertedValue is null) return null;
		var (stringValue, isNumeric) = convertedValue.Value;

		if (!_writeNumbersAsString && EnumFormat.HasFlag(EnumFormats.UseNumberValue) && isNumeric)
			return Value(in stringValue);

		return Value($"\"{stringValue}\"");
	}

	/// <inheritdoc />
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IDataValue data) return default;
		if (string.IsNullOrWhiteSpace(data.Value)) return default;

		var value = data.Value!.StartsWith(JsonCharacterConstants.PropertyWrapCharacter)
			? data.Value![1..^1]
			: data.Value;
		return ConvertToEnum(value, context.PropertyType);
	}
}
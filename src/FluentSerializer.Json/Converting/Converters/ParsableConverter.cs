#if NET7_0_OR_GREATER
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.DataNodes;

using System;
using System.Data;
using System.Globalization;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IParsable{TSelf}"/>
/// </summary>
public sealed class ParsableConverter : ParsableConverterBase, IJsonConverter
{
	/// <inheritdoc cref="ParsableConverter"/>
	private ParsableConverter(in CultureInfo? formatProvider, in bool tryParse) : base(in formatProvider, in tryParse) { }

	/// <inheritdoc cref="ParsableConverter"/>
	public static ParsableConverter Parse(in CultureInfo? formatProvider = null) => new (in formatProvider, false);

	/// <inheritdoc cref="ParsableConverter"/>
	public static ParsableConverter TryParse(in CultureInfo? formatProvider = null) => new (in formatProvider, true);

	/// <inheritdoc />
	public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
		throw new NotSupportedException("This is a Deserialize only converter.");

	/// <inheritdoc />
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context)
	{
		if (objectToDeserialize is not IDataValue data) return default;

		if (context.PropertyType == typeof(string)) return HandleStringData(in data);

		return ConvertToNullableDataType(data.Value, context.PropertyType);
	}

	/// <summary>
	/// The <see cref="ParsableConverterBase.ConvertToNullableDataType"/> can probably handle string very well.
	/// However, because of how the quotes work in JSON, we'd like some more control when it comes to strings.
	/// </summary>
	private static string? HandleStringData(in IDataValue data)
	{
		if (string.IsNullOrEmpty(data.Value)) return default;
		if (data.Value?.Length >= 2) return data.Value[1..^1];

		throw new DataException("A string type cannot be smaller than 2 since it should be surrounded in quotes");
	}
}
#endif
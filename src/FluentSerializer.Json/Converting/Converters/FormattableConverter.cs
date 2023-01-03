using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.DataNodes;

using System;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IFormattable"/>
/// </summary>
public sealed class FormattableConverter : FormattableConverterBase, IJsonConverter
{
	/// <inheritdoc cref="FormattableConverter"/>
	public FormattableConverter(in string? format, in IFormatProvider? formatProvider) : base(in format, in formatProvider) { }

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
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context) =>
		throw new NotSupportedException("This is a Serialize only converter.");
}

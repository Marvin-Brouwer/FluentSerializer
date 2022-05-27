using System;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IConvertible"/>
/// </summary>
public abstract class ConvertibleConverterBase : IConverter
{
	/// <inheritdoc cref="IConverter.Direction" />
	public SerializerDirection Direction { get; } = SerializerDirection.Both;

	/// <inheritdoc cref="IConverter.CanConvert(in Type)" />
	public bool CanConvert(in Type targetType) => typeof(IConvertible).IsAssignableFrom(targetType);

	/// <inheritdoc />
	public int ConverterHashCode { get; } = typeof(IConvertible).GetHashCode();

	/// <summary>
	/// Wrapper around <see cref="Convert.ToString(bool)"/>
	/// </summary>
	protected static string? ConvertToString(in object value) => Convert.ToString(value);

	/// <summary>
	/// Wrapper around <see cref="Convert.ChangeType(object?, Type)"/> to support nullable values
	/// </summary>
	protected static object? ConvertToNullableDataType(in string? currentValue, in Type targetType)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return Convert.ChangeType(currentValue, targetType);
	}
}
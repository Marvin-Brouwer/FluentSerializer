using FluentSerializer.Core.Configuration;

using System;
using System.Globalization;

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

	private readonly CultureInfo? _formatProvider;
	private CultureInfo FormatProvider => _formatProvider ?? CultureInfo.CurrentCulture;

	/// <inheritdoc cref="ConvertibleConverterBase"/>
	protected ConvertibleConverterBase(CultureInfo? formatProvider)
	{
		_formatProvider = formatProvider;
	}

	/// <summary>
	/// Wrapper around <see cref="Convert.ToString(bool)"/>
	/// </summary>
	protected string? ConvertToString(in object value) => Convert.ToString(value, FormatProvider);

	/// <summary>
	/// Wrapper around <see cref="Convert.ChangeType(object?, Type)"/> to support nullable values
	/// </summary>
	protected object? ConvertToNullableDataType(in string? currentValue, in Type targetType)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return Convert.ChangeType(currentValue, targetType, FormatProvider);
	}
}
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;

using System;
using System.Globalization;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IFormattable"/>
/// </summary>
public abstract class FormattableConverterBase : IConverter
{
	/// <inheritdoc cref="IConverter.Direction" />
	public SerializerDirection Direction { get; } = SerializerDirection.Serialize;

	/// <inheritdoc cref="IConverter.CanConvert(in Type)" />
	public bool CanConvert(in Type targetType) => targetType.Implements(typeof(IFormattable));

	/// <inheritdoc />
	public Guid ConverterId { get; } = typeof(IFormattable).GUID;

	private readonly IFormatProvider? _formatProvider;
	private readonly string? _format;

	private IFormatProvider FormatProvider => _formatProvider ?? CultureInfo.CurrentCulture;

	/// <inheritdoc cref="ConvertibleConverterBase"/>
	protected FormattableConverterBase(in IFormatProvider? formatProvider, in string? format = null)
	{
		_formatProvider = formatProvider;
		_format = format;
	}

	/// <summary>
	/// Wrapper around <see cref="IFormattable"/>
	/// </summary>
	protected string? ConvertToString(in object value)
	{
		if (value is not IFormattable formattableValue)
			throw new InvalidCastException($"The type '{value.GetType().FullName}' does not implement {nameof(IFormattable)} interface");

		return formattableValue.ToString(_format, FormatProvider);
	}
}

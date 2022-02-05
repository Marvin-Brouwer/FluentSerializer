using System;
using System.Globalization;
using Ardalis.GuardClauses;
using FluentSerializer.Json.Converting.Converters.Base;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts dates based on the format provided
/// </summary>
public class DateByFormatConverter : SimpleTypeConverter<DateTime>
{
	private readonly string _format;
	private readonly CultureInfo _cultureInfo;
	private readonly DateTimeStyles _dateTimeStyle;

	/// <summary>
	/// Converts dates based on the <paramref name="format"/> provided
	/// </summary>
	public DateByFormatConverter(in string format, in CultureInfo cultureInfo, in DateTimeStyles dateTimeStyle)
	{
		Guard.Against.NullOrWhiteSpace(format, nameof(format));
		Guard.Against.Null(cultureInfo, nameof(cultureInfo));
		Guard.Against.Null(dateTimeStyle, nameof(dateTimeStyle));

		_format = format;
		_cultureInfo = cultureInfo;
		_dateTimeStyle = dateTimeStyle;
	}

	/// <inheritdoc />
	protected override DateTime ConvertToDataType(in string currentValue) => DateTime.ParseExact(currentValue, _format, _cultureInfo, _dateTimeStyle);

	/// <inheritdoc />
	protected override string ConvertToString(in DateTime value) => value.ToString(_format, _cultureInfo);
}
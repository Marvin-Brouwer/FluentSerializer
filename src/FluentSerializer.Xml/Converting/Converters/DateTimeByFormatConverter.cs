using Ardalis.GuardClauses;

using FluentSerializer.Xml.Converting.Converters.Base;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts dates based on the format provided
/// </summary>
public class DateTimeByFormatConverter : SimpleTypeConverter<DateTime>
{
	private readonly string _format;
	private readonly CultureInfo _cultureInfo;
	private readonly DateTimeStyles _dateTimeStyle;

	/// <summary>
	/// Converts dates based on the <paramref name="format"/> provided
	/// </summary>
	public DateTimeByFormatConverter(in string format, in CultureInfo cultureInfo, in DateTimeStyles dateTimeStyle)
	{
		Guard.Against.NullOrWhiteSpace(format
#if NETSTANDARD
			, nameof(format)
#endif
		);
		Guard.Against.Null(cultureInfo
#if NETSTANDARD
			, nameof(cultureInfo)
#endif
		);

		_format = format;
		_cultureInfo = cultureInfo;
		_dateTimeStyle = dateTimeStyle;
	}

	/// <inheritdoc />
	protected override DateTime ConvertToDataType(in string currentValue) => DateTime.ParseExact(currentValue, _format, _cultureInfo, _dateTimeStyle);

	/// <inheritdoc />
	protected override string ConvertToString(in DateTime value) => value.ToString(_format, _cultureInfo);
}
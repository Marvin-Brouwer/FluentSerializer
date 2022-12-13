#if NET5_0_OR_GREATER
using Ardalis.GuardClauses;

using FluentSerializer.Xml.Converting.Converters.Base;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts dates based on the format provided
/// </summary>
public class TimeOnlyByFormatConverter : SimpleTypeConverter<TimeOnly>
{
	private readonly string _format;
	private readonly CultureInfo _cultureInfo;
	private readonly DateTimeStyles _dateTimeStyle;

	/// <summary>
	/// Converts dates based on the <paramref name="format"/> provided
	/// </summary>
	public TimeOnlyByFormatConverter(in string format, in CultureInfo cultureInfo, in DateTimeStyles dateTimeStyle)
	{
		Guard.Against.NullOrWhiteSpace(format);
		Guard.Against.Null(cultureInfo);

		_format = format;
		_cultureInfo = cultureInfo;
		_dateTimeStyle = dateTimeStyle;
	}

	/// <inheritdoc />
	protected override TimeOnly ConvertToDataType(in string currentValue) => TimeOnly.ParseExact(currentValue, _format, _cultureInfo, _dateTimeStyle);

	/// <inheritdoc />
	protected override string ConvertToString(in TimeOnly value) => value.ToString(_format, _cultureInfo);
}
#endif
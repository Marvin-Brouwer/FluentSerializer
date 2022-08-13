using Ardalis.GuardClauses;

using FluentSerializer.Xml.Converting.Converters.Base;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts <see cref="TimeSpan"/>s based on the format provided
/// </summary>
public class TimeSpanByFormatConverter : SimpleTypeConverter<TimeSpan>
{
	private readonly string _format;
	private readonly CultureInfo _cultureInfo;
	private readonly TimeSpanStyles _timeSpanStyles;

	/// <summary>
	/// Converts dates based on the <paramref name="format"/> provided
	/// </summary>
	public TimeSpanByFormatConverter(in string format, in CultureInfo cultureInfo, in TimeSpanStyles timeSpanStyles)
	{
		Guard.Against.NullOrWhiteSpace(format, nameof(format));
		Guard.Against.Null(cultureInfo, nameof(cultureInfo));
		Guard.Against.Null(timeSpanStyles, nameof(timeSpanStyles));

		_format = format;
		_cultureInfo = cultureInfo;
		_timeSpanStyles = timeSpanStyles;
	}

	/// <inheritdoc />
	protected override TimeSpan ConvertToDataType(in string currentValue) => TimeSpan.ParseExact(currentValue, _format, _cultureInfo, _timeSpanStyles);

	/// <inheritdoc />
	protected override string ConvertToString(in TimeSpan value) => value.ToString(_format, _cultureInfo);

	/// <inheritdoc />
	public override int GetHashCode() => TimeSpan.MinValue.GetHashCode();
}

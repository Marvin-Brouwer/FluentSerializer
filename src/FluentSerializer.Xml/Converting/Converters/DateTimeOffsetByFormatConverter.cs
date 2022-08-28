using Ardalis.GuardClauses;

using FluentSerializer.Xml.Converting.Converters.Base;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts dates based on the format provided
/// </summary>
public class DateTimeOffsetByFormatConverter : SimpleTypeConverter<DateTimeOffset>
{
	private readonly string _format;
	private readonly CultureInfo _cultureInfo;
	private readonly DateTimeStyles _dateTimeStyle;

	/// <summary>
	/// Converts dates based on the <paramref name="format"/> provided
	/// </summary>
	public DateTimeOffsetByFormatConverter(in string format, in CultureInfo cultureInfo, in DateTimeStyles dateTimeStyle)
	{
		Guard.Against.NullOrWhiteSpace(format
#if NETSTANDARD2_1
			, nameof(format)
#endif
		);
		Guard.Against.Null(cultureInfo
#if NETSTANDARD2_1
			, nameof(cultureInfo)
#endif
		);
		Guard.Against.Null(dateTimeStyle
#if NETSTANDARD2_1
			, nameof(dateTimeStyle)
#endif
		);

		_format = format;
		_cultureInfo = cultureInfo;
		_dateTimeStyle = dateTimeStyle;
	}

	/// <inheritdoc />
	protected override DateTimeOffset ConvertToDataType(in string currentValue) => DateTimeOffset.ParseExact(currentValue, _format, _cultureInfo, _dateTimeStyle);

	/// <inheritdoc />
	protected override string ConvertToString(in DateTimeOffset value) => value.ToString(_format, _cultureInfo);

	/// <inheritdoc />
	public override int GetHashCode() => DateTimeOffset.MinValue.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => GetHashCode() == (obj?.GetHashCode() ?? 0);
}
#if NET5_0_OR_GREATER
using Ardalis.GuardClauses;

using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts dates based on the format provided
/// </summary>
public class DateOnlyByFormatConverter : SimpleTypeConverter<DateOnly>
{
	private readonly string _format;
	private readonly CultureInfo _cultureInfo;
	private readonly DateTimeStyles _dateTimeStyle;

	/// <summary>
	/// Converts dates based on the <paramref name="format"/> provided
	/// </summary>
	public DateOnlyByFormatConverter(in string format, in CultureInfo cultureInfo, in DateTimeStyles dateTimeStyle)
	{
		Guard.Against.NullOrWhiteSpace(format);
		Guard.Against.Null(cultureInfo);

		_format = format;
		_cultureInfo = cultureInfo;
		_dateTimeStyle = dateTimeStyle;
	}

	/// <inheritdoc />
	protected override DateOnly ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length >= 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter)
			? currentValue[1..^1]
			: currentValue;

		return DateOnly.ParseExact(dateValue, _format, _cultureInfo, _dateTimeStyle);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in DateOnly value) =>
		JsonCharacterConstants.PropertyWrapCharacter +
		value.ToString(_format, _cultureInfo) +
		JsonCharacterConstants.PropertyWrapCharacter;
}
#endif
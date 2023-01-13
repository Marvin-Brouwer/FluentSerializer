using Ardalis.GuardClauses;

#if NETSTANDARD2_0
using FluentSerializer.Core.Dirty.BackwardsCompatibility.NetFramework;
#endif
using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting.Converters;

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
		_timeSpanStyles = timeSpanStyles;
	}

	/// <inheritdoc />
	protected override TimeSpan ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length >= 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter)
			? currentValue[1..^1]
			: currentValue;

		return TimeSpan.ParseExact(dateValue, _format, _cultureInfo, _timeSpanStyles);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in TimeSpan value) =>
		JsonCharacterConstants.PropertyWrapCharacter +
		value.ToString(_format, _cultureInfo) +
		JsonCharacterConstants.PropertyWrapCharacter;
}

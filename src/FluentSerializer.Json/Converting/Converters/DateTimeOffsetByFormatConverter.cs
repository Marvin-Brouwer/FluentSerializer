using Ardalis.GuardClauses;

using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting.Converters;

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

		_format = format;
		_cultureInfo = cultureInfo;
		_dateTimeStyle = dateTimeStyle;
	}

	/// <inheritdoc />
	protected override DateTimeOffset ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length >= 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter)
			? currentValue[1..^1]
			: currentValue;

		return DateTimeOffset.ParseExact(dateValue, _format, _cultureInfo, _dateTimeStyle);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in DateTimeOffset value) => 
		JsonCharacterConstants.PropertyWrapCharacter + 
		value.ToString(_format, _cultureInfo) +
		JsonCharacterConstants.PropertyWrapCharacter;

	/// <inheritdoc />
	public override int GetHashCode() => DateTimeOffset.MinValue.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => GetHashCode() == (obj?.GetHashCode() ?? 0);
}
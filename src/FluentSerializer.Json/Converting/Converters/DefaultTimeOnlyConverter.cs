#if NET5_0_OR_GREATER
using FluentSerializer.Core.Constants;
using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts times <br/>
/// Using <see cref="TimeOnly.Parse(string, IFormatProvider?, DateTimeStyles)"/>
/// with <see cref="CultureInfo.CurrentCulture"/>
/// and <see cref="DateTimeStyles.AllowWhiteSpaces"/> for deserializing <br/>
/// Using <c>TimeOnly.ToString(IsoTimeFormat, CultureInfo.CurrentCulture)</c> for serializing
/// with a format like HH:mm:ssK
/// </summary>
public sealed class DefaultTimeOnlyConverter : SimpleTypeConverter<TimeOnly>
{
	/// <inheritdoc />
	protected override TimeOnly ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length >= 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter)
			? currentValue[1..^1]
			: currentValue;
		return TimeOnly.Parse(dateValue, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in TimeOnly value) =>
		JsonCharacterConstants.PropertyWrapCharacter +
		value.ToString(DateTimeConstants.IsoTimeFormat, CultureInfo.CurrentCulture) +
		JsonCharacterConstants.PropertyWrapCharacter;
}
#endif
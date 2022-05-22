#if NET5_0_OR_GREATER
using System;
using System.Globalization;
using FluentSerializer.Core.Constants;
using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts times <br/>
/// Using <see cref="TimeOnly.Parse(string, IFormatProvider?)"/> with <see cref="CultureInfo.CurrentCulture"/> for deserializing <br/>
/// Using <c>TimeOnly.ToString(IsoTimeFormat, CultureInfo.CurrentCulture)</c> for serializing
/// with a format like HH:mm:ssK
/// </summary>
public sealed class DefaultTimeOnlyConverter : SimpleTypeConverter<TimeOnly>
{
	/// <inheritdoc />
	protected override TimeOnly ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length > 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter) 
			? currentValue[1..^1]
			: currentValue;
		return TimeOnly.Parse(dateValue, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in TimeOnly value) =>
		JsonCharacterConstants.PropertyWrapCharacter + 
		value.ToString(DateTimeConstants.IsoTimeFormat, CultureInfo.CurrentCulture) +
		JsonCharacterConstants.PropertyWrapCharacter;

	/// <inheritdoc />
	public override int GetHashCode() => TimeOnly.MinValue.GetHashCode();
}
#endif
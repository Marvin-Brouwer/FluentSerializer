#if NET5_0_OR_GREATER
using System;
using System.Globalization;
using FluentSerializer.Core.Constants;
using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts dates <br/>
/// Using <see cref="DateOnly.Parse(string, IFormatProvider?, DateTimeStyles)"/> 
/// with <see cref="CultureInfo.CurrentCulture"/>
/// and <see cref="DateTimeStyles.NoCurrentDateDefault"/> for deserializing <br/>
/// Using <c>DateOnly.ToString(IsoDateFormat, CultureInfo.CurrentCulture)</c> for serializing
/// with a format like yyyy-MM-dd
/// </summary>
public sealed class DefaultDateOnlyConverter : SimpleTypeConverter<DateOnly>
{
	/// <inheritdoc />
	protected override DateOnly ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length > 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter) 
			? currentValue[1..^1]
			: currentValue;
		return DateOnly.Parse(dateValue, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in DateOnly value) =>
		JsonCharacterConstants.PropertyWrapCharacter + 
		value.ToString(DateTimeConstants.IsoDateFormat, CultureInfo.CurrentCulture) +
		JsonCharacterConstants.PropertyWrapCharacter;

	/// <inheritdoc />
	public override int GetHashCode() => DateOnly.MinValue.GetHashCode();
}
#endif
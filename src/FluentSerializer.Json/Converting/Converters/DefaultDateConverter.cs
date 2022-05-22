using System;
using System.Globalization;
using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts dates <br/>
/// Using <see cref="DateTime.Parse(string, IFormatProvider?)"/> with <see cref="CultureInfo.CurrentCulture"/> for deserializing <br/>
/// Using <c>DateTime.ToUniversalTime().ToString(IsoDateFormat, CultureInfo.CurrentCulture)</c> for serializing
/// with a format like yyyy-MM-ddTHH:mm:ssK
/// </summary>
public sealed class DefaultDateTimeConverter : SimpleTypeConverter<DateTime>
{
	private const string IsoDateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

	/// <inheritdoc />
	protected override DateTime ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length > 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter) 
			? currentValue[1..^1]
			: currentValue;
		return DateTime.Parse(dateValue, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in DateTime value) =>
		JsonCharacterConstants.PropertyWrapCharacter + 
		value.ToUniversalTime().ToString(IsoDateFormat, CultureInfo.CurrentCulture) +
		JsonCharacterConstants.PropertyWrapCharacter;

	/// <inheritdoc />
	public override int GetHashCode() => DateTime.MinValue.GetHashCode();
}
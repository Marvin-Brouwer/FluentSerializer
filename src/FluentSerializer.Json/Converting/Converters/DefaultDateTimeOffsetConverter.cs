using FluentSerializer.Core.Constants;
using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts dates <br/>
/// Using <see cref="DateTimeOffset.Parse(string, IFormatProvider?)"/> 
/// with <see cref="CultureInfo.CurrentCulture"/>
/// and <see cref="DateTimeStyles.AdjustToUniversal"/> for deserializing <br/>
/// Using <c>DateTimeOffset.ToUniversalTime().ToString(IsoDateTimeFormat, CultureInfo.CurrentCulture)</c> for serializing
/// with a format like yyyy-MM-ddTHH:mm:ssK
/// </summary>
public sealed class DefaultDateTimeOffsetConverter : SimpleTypeConverter<DateTimeOffset>
{
	/// <inheritdoc />
	protected override DateTimeOffset ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length >= 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter) 
			? currentValue[1..^1]
			: currentValue;
		return DateTimeOffset.Parse(dateValue, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in DateTimeOffset value) =>
		JsonCharacterConstants.PropertyWrapCharacter + 
		value.ToUniversalTime().ToString(DateTimeConstants.IsoDateTimeFormat, CultureInfo.CurrentCulture) +
		JsonCharacterConstants.PropertyWrapCharacter;

	/// <inheritdoc />
	public override int GetHashCode() => DateTimeOffset.MinValue.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => GetHashCode() == (obj?.GetHashCode() ?? 0);
}
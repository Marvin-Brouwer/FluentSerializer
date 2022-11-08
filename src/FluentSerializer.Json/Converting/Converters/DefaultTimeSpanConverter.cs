using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts times <br/>
/// Using <see cref="TimeSpan.Parse(string, IFormatProvider?)"/>
/// with <see cref="CultureInfo.CurrentCulture"/> for deserializing <br/>
/// Using <c>TimeSpan.ToString(null, CultureInfo.CurrentCulture)</c> for serializing
/// </summary>
public sealed class DefaultTimeSpanConverter : SimpleTypeConverter<TimeSpan>
{
	/// <inheritdoc />
	protected override TimeSpan ConvertToDataType(in string currentValue)
	{
		var dateValue = currentValue.Length >= 2 && currentValue.StartsWith(JsonCharacterConstants.PropertyWrapCharacter) 
			? currentValue[1..^1]
			: currentValue;
		return TimeSpan.Parse(dateValue, CultureInfo.CurrentCulture);
	}

	/// <inheritdoc />
	protected override string ConvertToString(in TimeSpan value) =>
		JsonCharacterConstants.PropertyWrapCharacter + 
		value.ToString(null, CultureInfo.CurrentCulture) +
		JsonCharacterConstants.PropertyWrapCharacter;

	/// <inheritdoc />
	public override int GetHashCode() => TimeSpan.MinValue.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => GetHashCode() == (obj?.GetHashCode() ?? 0);
}
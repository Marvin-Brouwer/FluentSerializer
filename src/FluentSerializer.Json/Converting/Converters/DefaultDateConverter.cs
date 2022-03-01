using System;
using System.Globalization;
using FluentSerializer.Json.Converting.Converters.Base;
using FluentSerializer.Json.DataNodes;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts dates based on DotNet's default, using <see cref="CultureInfo.CurrentCulture"/>
/// </summary>
public sealed class DefaultDateConverter : SimpleTypeConverter<DateTime>
{
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
		value.ToString(CultureInfo.CurrentCulture) +
		JsonCharacterConstants.PropertyWrapCharacter;

	/// <inheritdoc />
	public override int GetHashCode() => DateTime.MinValue.GetHashCode();
}
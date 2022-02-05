using System;
using System.Globalization;
using FluentSerializer.Json.Converting.Converters.Base;

namespace FluentSerializer.Json.Converting.Converters;

/// <summary>
/// Converts dates based on DotNet's default, using <see cref="CultureInfo.CurrentCulture"/>
/// </summary>
public class DefaultDateConverter : SimpleTypeConverter<DateTime>
{
	/// <inheritdoc />
	protected override DateTime ConvertToDataType(string currentValue) => DateTime.Parse(currentValue, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);

	/// <inheritdoc />
	protected override string ConvertToString(DateTime value) => value.ToString(CultureInfo.CurrentCulture);
}
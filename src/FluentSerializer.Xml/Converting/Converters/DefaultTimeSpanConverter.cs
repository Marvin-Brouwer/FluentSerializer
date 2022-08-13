using FluentSerializer.Xml.Converting.Converters.Base;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts times <br/>
/// Using <see cref="TimeSpan.Parse(string, IFormatProvider?)"/>
/// with <see cref="CultureInfo.CurrentCulture"/> for deserializing <br/>
/// Using <c>TimeSpan.ToString(null, CultureInfo.CurrentCulture)</c> for serializing
/// </summary>
public sealed class DefaultTimeSpanConverter : SimpleTypeConverter<TimeSpan>
{
	/// <inheritdoc />
	protected override TimeSpan ConvertToDataType(in string currentValue) => TimeSpan.Parse(currentValue, CultureInfo.CurrentCulture);

	/// <inheritdoc />
	protected override string ConvertToString(in TimeSpan value) => value.ToString(null, CultureInfo.CurrentCulture);

	/// <inheritdoc />
	public override int GetHashCode() => TimeSpan.MinValue.GetHashCode();
}
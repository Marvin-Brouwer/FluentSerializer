#if NET5_0_OR_GREATER
using System;
using System.Globalization;
using FluentSerializer.Core.Constants;
using FluentSerializer.Xml.Converting.Converters.Base;

namespace FluentSerializer.Xml.Converting.Converters;

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
	protected override TimeOnly ConvertToDataType(in string currentValue) => TimeOnly.Parse(currentValue, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces);

	/// <inheritdoc />
	protected override string ConvertToString(in TimeOnly value) => value.ToString(DateTimeConstants.IsoTimeFormat, CultureInfo.CurrentCulture);

	/// <inheritdoc />
	public override int GetHashCode() => TimeOnly.MinValue.GetHashCode();
}
#endif
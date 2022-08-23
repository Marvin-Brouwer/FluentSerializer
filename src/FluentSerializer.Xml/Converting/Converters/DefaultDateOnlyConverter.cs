#if NET5_0_OR_GREATER
using FluentSerializer.Core.Constants;
using FluentSerializer.Xml.Converting.Converters.Base;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts dates <br/>
/// Using <see cref="DateOnly.Parse(string, IFormatProvider?, DateTimeStyles)"/> 
/// with <see cref="CultureInfo.CurrentCulture"/>
/// and <see cref="DateTimeStyles.AllowWhiteSpaces"/> for deserializing <br/>
/// Using <c>DateOnly.ToString(IsoDateFormat, CultureInfo.CurrentCulture)</c> for serializing
/// with a format like yyyy-MM-dd
/// </summary>
public sealed class DefaultDateOnlyConverter : SimpleTypeConverter<DateOnly>
{
	/// <inheritdoc />
	protected override DateOnly ConvertToDataType(in string currentValue) => DateOnly.Parse(currentValue, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces);

	/// <inheritdoc />
	protected override string ConvertToString(in DateOnly value) => value.ToString(DateTimeConstants.IsoDateFormat, CultureInfo.CurrentCulture);

	/// <inheritdoc />
	public override int GetHashCode() => DateOnly.MinValue.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object? obj) => GetHashCode() == (obj?.GetHashCode() ?? 0);
}
#endif
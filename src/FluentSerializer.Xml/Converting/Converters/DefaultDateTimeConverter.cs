using FluentSerializer.Core.Constants;
using FluentSerializer.Xml.Converting.Converters.Base;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts dates <br/>
/// Using <see cref="DateTime.Parse(string, IFormatProvider?)"/>
/// with <see cref="CultureInfo.CurrentCulture"/>
/// and <see cref="DateTimeStyles.AdjustToUniversal"/> for deserializing <br/>
/// Using <c>DateTime.ToUniversalTime().ToString(IsoDateTimeFormat, CultureInfo.CurrentCulture)</c> for serializing
/// with a format like yyyy-MM-ddTHH:mm:ssK
/// </summary>
public sealed class DefaultDateTimeConverter : SimpleTypeConverter<DateTime>
{
	/// <inheritdoc />
	protected override DateTime ConvertToDataType(in string currentValue) => DateTime.Parse(currentValue, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);

	/// <inheritdoc />
	protected override string ConvertToString(in DateTime value) => value.ToString(DateTimeConstants.IsoDateTimeFormat, CultureInfo.CurrentCulture);
}
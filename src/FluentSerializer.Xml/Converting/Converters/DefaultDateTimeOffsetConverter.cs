using System;
using System.Globalization;
using FluentSerializer.Core.Constants;
using FluentSerializer.Xml.Converting.Converters.Base;

namespace FluentSerializer.Xml.Converting.Converters;

/// <summary>
/// Converts dates <br/>
/// Using <see cref="DateTimeOffset.Parse(string, IFormatProvider?)"/> with <see cref="CultureInfo.CurrentCulture"/> for deserializing <br/>
/// Using <c>DateTimeOffset.ToUniversalTime().ToString(IsoDateTimeFormat, CultureInfo.CurrentCulture)</c> for serializing
/// with a format like yyyy-MM-ddTHH:mm:ssK
/// </summary>
public sealed class DefaultDateTimeOffsetConverter : SimpleTypeConverter<DateTimeOffset>
{
	/// <inheritdoc />
	protected override DateTimeOffset ConvertToDataType(in string currentValue) => DateTimeOffset.Parse(currentValue, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);

	/// <inheritdoc />
	protected override string ConvertToString(in DateTimeOffset value) => value.ToUniversalTime().ToString(DateTimeConstants.IsoDateTimeFormat, CultureInfo.CurrentCulture);

	/// <inheritdoc />
	public override int GetHashCode() => DateTimeOffset.MinValue.GetHashCode();
}
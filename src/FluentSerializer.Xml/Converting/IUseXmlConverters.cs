using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters.Base;
using FluentSerializer.Xml.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting;

/// <summary>
/// Use an <see cref="IXmlConverter"/> for this mapping
/// </summary>
/// <remarks>
/// For using a custom <see cref="IXmlConverter"/> create an extension method on <see cref="IUseXmlConverters"/>
/// </remarks>
public interface IUseXmlConverters
{
	/// <inheritdoc cref="DefaultDateTimeConverter" />
	SimpleTypeConverter<DateTime> DateTime();

	/// <inheritdoc cref="DateTimeByFormatConverter" />
	Func<SimpleTypeConverter<DateTime>> DateTime(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AdjustToUniversal);

	/// <inheritdoc cref="DefaultDateTimeOffsetConverter" />
	SimpleTypeConverter<DateTimeOffset> DateTimeOffset();

	/// <inheritdoc cref="DateTimeOffsetByFormatConverter" />
	Func<SimpleTypeConverter<DateTimeOffset>> DateTimeOffset(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AdjustToUniversal);

#if NET5_0_OR_GREATER
	/// <inheritdoc cref="DefaultDateOnlyConverter" />
	SimpleTypeConverter<DateOnly> DateOnly();

	/// <inheritdoc cref="DateOnlyByFormatConverter" />
	Func<SimpleTypeConverter<DateOnly>> DateOnly(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces);

	/// <inheritdoc cref="DefaultTimeOnlyConverter" />
	SimpleTypeConverter<TimeOnly> TimeOnly();

	/// <inheritdoc cref="TimeOnlyByFormatConverter" />
	Func<SimpleTypeConverter<TimeOnly>> TimeOnly(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces);
#endif

	/// <inheritdoc cref="DefaultTimeSpanConverter" />
	SimpleTypeConverter<TimeSpan> TimeSpan();

	/// <inheritdoc cref="TimeSpanByFormatConverter" />
	Func<SimpleTypeConverter<TimeSpan>> TimeSpan(string format, CultureInfo? culture = null, TimeSpanStyles style = TimeSpanStyles.None);

	/// <summary>
	/// Converts most DotNet collections
	/// </summary>
	/// <param name="wrapCollection">When true, wraps the collection in a tag of the property name</param>
	Func<IXmlConverter<IXmlElement>> Collection(in bool wrapCollection = true);

	/// <inheritdoc cref="EnumConverter" />
	IXmlConverter Enum();
	/// <inheritdoc cref="EnumConverter(in EnumFormats)" />
	Func<IXmlConverter> Enum(EnumFormats format);

#if NET7_0_OR_GREATER

	/// <inheritdoc cref="ParsableConverter" />
	IXmlConverter Parse();

	/// <inheritdoc cref="ParsableConverter.Parse(in CultureInfo?)" />
	Func<IXmlConverter> Parse(CultureInfo formatProvider);

	/// <inheritdoc cref="ParsableConverter" />
	IXmlConverter TryParse();

	/// <inheritdoc cref="ParsableConverter.TryParse(in CultureInfo?)" />
	Func<IXmlConverter> TryParse(CultureInfo formatProvider);

#endif
}
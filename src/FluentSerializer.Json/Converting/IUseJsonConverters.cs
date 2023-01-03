using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting.Converters;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting;

/// <summary>
/// Use an <see cref="IJsonConverter"/> for this mapping
/// </summary>
/// <remarks>
/// For using a custom <see cref="IJsonConverter"/> create an extension method on <see cref="IUseJsonConverters"/>
/// </remarks>
public interface IUseJsonConverters
{
	/// <inheritdoc cref="DefaultDateTimeConverter" />
	IJsonConverter DateTime();

	/// <inheritdoc cref="DateTimeByFormatConverter" />
	Func<IJsonConverter> DateTime(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AdjustToUniversal);

	/// <inheritdoc cref="DefaultDateTimeOffsetConverter" />
	IJsonConverter DateTimeOffset();

	/// <inheritdoc cref="DateTimeOffsetByFormatConverter" />
	Func<IJsonConverter> DateTimeOffset(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AdjustToUniversal);

#if NET5_0_OR_GREATER
	/// <inheritdoc cref="DefaultDateOnlyConverter" />
	IJsonConverter DateOnly();

	/// <inheritdoc cref="DateOnlyByFormatConverter" />
	Func<IJsonConverter> DateOnly(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces);

	/// <inheritdoc cref="DefaultTimeOnlyConverter" />
	IJsonConverter TimeOnly();

	/// <inheritdoc cref="TimeOnlyByFormatConverter" />
	Func<IJsonConverter> TimeOnly(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces);
#endif

	/// <inheritdoc cref="DefaultTimeSpanConverter" />
	IJsonConverter TimeSpan();

	/// <inheritdoc cref="TimeSpanByFormatConverter" />
	Func<IJsonConverter> TimeSpan(string format, CultureInfo? culture = null, TimeSpanStyles style = TimeSpanStyles.None);

	/// <inheritdoc cref="CollectionConverter" />
	IJsonConverter Collection();

	/// <inheritdoc cref="EnumConverter" />
	IJsonConverter Enum();
	/// <inheritdoc cref="EnumConverter(in EnumFormats, in bool)" />
	Func<IJsonConverter> Enum(EnumFormats format, bool writeNumbersAsString = false);

#if NET7_0_OR_GREATER

	/// <inheritdoc cref="ParsableConverter" />
	IJsonConverter Parsable();

	/// <inheritdoc cref="ParsableConverter" />
	Func<IJsonConverter> Parsable(IFormatProvider formatProvider);

	/// <inheritdoc cref="ParsableConverter" />
	Func<IJsonConverter> Parsable(bool tryParse);

	/// <inheritdoc cref="ParsableConverter" />
	Func<IJsonConverter> Parsable(bool tryParse, IFormatProvider formatProvider);

#endif
}
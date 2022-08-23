using Ardalis.GuardClauses;

using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting.Converters;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting;

/// <inheritdoc />
public sealed class UseJsonConverters : IUseJsonConverters
{
	internal static readonly IJsonConverter DefaultDateTimeConverter = new DefaultDateTimeConverter();
	internal static readonly IJsonConverter DefaultDateTimeOffsetConverter = new DefaultDateTimeOffsetConverter();
#if NET5_0_OR_GREATER
	internal static readonly IJsonConverter DefaultDateOnlyConverter = new DefaultDateOnlyConverter();
	internal static readonly IJsonConverter DefaultTimeOnlyConverter = new DefaultTimeOnlyConverter();
#endif
	internal static readonly IJsonConverter DefaultTimeSpanConverter = new DefaultTimeSpanConverter();
	internal static readonly IJsonConverter CollectionConverter = new CollectionConverter();
	internal static readonly IJsonConverter ConvertibleConverter = new ConvertibleConverter();
	internal static readonly IJsonConverter DefaultEnumConverter = new EnumConverter(EnumFormats.Default, false);

	/// <inheritdoc />
	public IJsonConverter DateTime() => DefaultDateTimeConverter;

	/// <inheritdoc />
	public Func<IJsonConverter> DateTime(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AdjustToUniversal)
	{
		Guard.Against.NullOrWhiteSpace(format
#if NETSTANDARD2_1
			, nameof(format)
#endif
		);

		return () => new DateTimeByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc />
	public IJsonConverter DateTimeOffset() => DefaultDateTimeOffsetConverter;

	/// <inheritdoc />
	public Func<IJsonConverter> DateTimeOffset(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AdjustToUniversal)
	{
		Guard.Against.NullOrWhiteSpace(format
#if NETSTANDARD2_1
			, nameof(format)
#endif
		);

		return () => new DateTimeOffsetByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

#if NET5_0_OR_GREATER
	/// <inheritdoc />
	public IJsonConverter DateOnly() => DefaultDateOnlyConverter;

	/// <inheritdoc />
	public Func<IJsonConverter> DateOnly(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces)
	{
		Guard.Against.NullOrWhiteSpace(format);
		return () => new DateOnlyByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc />
	public IJsonConverter TimeOnly() => DefaultTimeOnlyConverter;

	/// <inheritdoc />
	public Func<IJsonConverter> TimeOnly(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces)
	{
		Guard.Against.NullOrWhiteSpace(format);
		return () => new TimeOnlyByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}
#endif

	/// <inheritdoc />
	public IJsonConverter TimeSpan() => DefaultTimeSpanConverter;

	/// <inheritdoc />
	public Func<IJsonConverter> TimeSpan(string format, CultureInfo? culture = null, TimeSpanStyles style = TimeSpanStyles.None)
	{
		Guard.Against.NullOrWhiteSpace(format
#if NETSTANDARD2_1
			, nameof(format)
#endif
		);

		return () => new TimeSpanByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc />
	public IJsonConverter Collection() => CollectionConverter;

	/// <inheritdoc />
	public IJsonConverter Enum() => DefaultEnumConverter;
	/// <inheritdoc />
	public Func<IJsonConverter> Enum(EnumFormats format, bool writeNumbersAsString = false)
	{
		return () => new EnumConverter(format, writeNumbersAsString);
	}
}
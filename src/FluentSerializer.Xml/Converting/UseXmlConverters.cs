using Ardalis.GuardClauses;

using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters.Base;
using FluentSerializer.Xml.DataNodes;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converting;

/// <inheritdoc/>
public sealed class UseXmlConverters : IUseXmlConverters
{
	internal static readonly SimpleTypeConverter<DateTime> DefaultDateTimeConverter = new DefaultDateTimeConverter();
	internal static readonly SimpleTypeConverter<DateTimeOffset> DefaultDateTimeOffsetConverter = new DefaultDateTimeOffsetConverter();
#if NET5_0_OR_GREATER
	internal static readonly SimpleTypeConverter<DateOnly> DefaultDateOnlyConverter = new DefaultDateOnlyConverter();
	internal static readonly SimpleTypeConverter<TimeOnly> DefaultTimeOnlyConverter = new DefaultTimeOnlyConverter();
#endif
	internal static readonly SimpleTypeConverter<TimeSpan> DefaultTimeSpanConverter = new DefaultTimeSpanConverter();
	internal static readonly IXmlConverter<IXmlElement> WrappedCollectionConverter = new WrappedCollectionConverter();
	private static readonly IXmlConverter<IXmlElement> NonWrappedCollectionConverter = new NonWrappedCollectionConverter();
	internal static readonly IXmlConverter ConvertibleConverter = new ConvertibleConverter();
	internal static readonly IXmlConverter DefaultEnumConverter = new EnumConverter(EnumFormats.Default);

	/// <inheritdoc/>
	public SimpleTypeConverter<DateTime> DateTime() => DefaultDateTimeConverter;

	/// <inheritdoc/>
	public Func<SimpleTypeConverter<DateTime>> DateTime(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AdjustToUniversal)
	{
		Guard.Against.NullOrWhiteSpace(format
#if NETSTANDARD2_1
			, nameof(format)
#endif
		);

		return () => new DateTimeByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc />
	public SimpleTypeConverter<DateTimeOffset> DateTimeOffset() => DefaultDateTimeOffsetConverter;

	/// <inheritdoc />
	public Func<SimpleTypeConverter<DateTimeOffset>> DateTimeOffset(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AdjustToUniversal)
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
	public SimpleTypeConverter<DateOnly> DateOnly() => DefaultDateOnlyConverter;

	/// <inheritdoc />
	public Func<SimpleTypeConverter<DateOnly>> DateOnly(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces)
	{
		Guard.Against.NullOrWhiteSpace(format);
		return () => new DateOnlyByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc />
	public SimpleTypeConverter<TimeOnly> TimeOnly() => DefaultTimeOnlyConverter;

	/// <inheritdoc />
	public Func<SimpleTypeConverter<TimeOnly>> TimeOnly(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces)
	{
		Guard.Against.NullOrWhiteSpace(format);
		return () => new TimeOnlyByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}
#endif

	/// <inheritdoc />
	public SimpleTypeConverter<TimeSpan> TimeSpan() => DefaultTimeSpanConverter;

	/// <inheritdoc />
	public Func<SimpleTypeConverter<TimeSpan>> TimeSpan(string format, CultureInfo? culture = null, TimeSpanStyles style = TimeSpanStyles.None)
	{
		Guard.Against.NullOrWhiteSpace(format
#if NETSTANDARD2_1
			, nameof(format)
#endif
		);

		return () => new TimeSpanByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc/>
	public Func<IXmlConverter<IXmlElement>> Collection(in bool wrapCollection = true)
	{
		if (wrapCollection) return () => WrappedCollectionConverter;
		return () => NonWrappedCollectionConverter;
	}

	/// <inheritdoc />
	public IXmlConverter Enum() => DefaultEnumConverter;
	/// <inheritdoc />
	public Func<IXmlConverter> Enum(EnumFormats format)
	{
		return () => new EnumConverter(format);
	}
}
using System;
using System.Globalization;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting.Converters;

namespace FluentSerializer.Json.Converting;

/// <inheritdoc />
public sealed class UseJsonConverters : IUseJsonConverters
{
	internal static readonly IJsonConverter DefaultDateConverter = new DefaultDateConverter();
	internal static readonly IJsonConverter CollectionConverter = new CollectionConverter();
	internal static readonly IJsonConverter ConvertibleConverter = new ConvertibleConverter();
	internal static readonly IJsonConverter DefaultEnumConverter = new EnumConverter(EnumFormat.Default, true);

	/// <inheritdoc />
	public IJsonConverter DateTime() => DefaultDateConverter;

	/// <inheritdoc />
	public Func<IJsonConverter> DateTime(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None)
	{
		Guard.Against.NullOrWhiteSpace(format, nameof(format));
		return () => new DateByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc />
	public IJsonConverter Collection() => CollectionConverter;

	/// <inheritdoc />
	public IJsonConverter Enum() => DefaultEnumConverter;
	/// <inheritdoc />
	public Func<IJsonConverter> Enum(EnumFormat format, bool writeNumbersAsString = false)
	{
		return () => new EnumConverter(format, writeNumbersAsString);
	}
}
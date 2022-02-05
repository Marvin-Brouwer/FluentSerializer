using System;
using System.Globalization;
using Ardalis.GuardClauses;
using FluentSerializer.Json.Converting.Converters;

namespace FluentSerializer.Json.Converting;

/// <inheritdoc />
public sealed class UseJsonConverters : IUseJsonConverters
{
	internal static readonly IJsonConverter DefaultDateConverter = new DefaultDateConverter();
	internal static readonly IJsonConverter CollectionConverter = new CollectionConverter();
	internal static readonly IJsonConverter ConvertibleConverter = new ConvertibleConverter();

	/// <inheritdoc />
	public IJsonConverter Dates() => DefaultDateConverter;

	/// <inheritdoc />
	public Func<IJsonConverter> Dates(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None)
	{
		Guard.Against.NullOrWhiteSpace(format, nameof(format));
		return () => new DateByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc />
	public IJsonConverter Collection() => CollectionConverter;
}
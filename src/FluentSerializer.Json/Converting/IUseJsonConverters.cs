using FluentSerializer.Json.Converting.Converters;
using System;
using System.Globalization;
using FluentSerializer.Core.Converting.Converters;

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
	Func<IJsonConverter> DateTime(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None);

	/// <inheritdoc cref="CollectionConverter" />
	IJsonConverter Collection();

	/// <inheritdoc cref="EnumConverter" />
	IJsonConverter Enum();
	/// <inheritdoc cref="EnumConverter(in EnumFormat, in bool)" />
	Func<IJsonConverter> Enum(EnumFormat format, bool writeNumbersAsString = true);
}
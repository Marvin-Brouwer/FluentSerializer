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
	/// <inheritdoc cref="DefaultDateConverter" />
	IJsonConverter DateTime();

	/// <inheritdoc cref="DateByFormatConverter" />
	Func<IJsonConverter> DateTime(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None);

	/// <inheritdoc cref="CollectionConverter" />
	IJsonConverter Collection();
}
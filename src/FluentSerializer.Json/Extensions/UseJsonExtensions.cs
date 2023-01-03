using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting;

using System;
using System.Globalization;

namespace FluentSerializer.Json.Extensions;

/// <summary>
/// Additional extensions to configure default converters.
/// </summary>
public static class UseJsonExtensions
{
	/// <inheritdoc cref="IUseJsonConverters.Enum(EnumFormats, bool)"/>
	public static IConfigurationStack<IConverter> UseEnum(this IConfigurationStack<IConverter> converters, in EnumFormats format, in bool writeNumbersAsString = false)
	{
		return converters.Use(Converter.For.Enum(format, writeNumbersAsString));
	}

#if NET7_0_OR_GREATER

	/// <inheritdoc cref="IUseJsonConverters.Parsable()"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters)
	{
		return converters.Use(UseJsonConverters.DefaultParseConverter);
	}

	/// <inheritdoc cref="IUseJsonConverters.Parsable(IFormatProvider)"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters, CultureInfo formatProvder)
	{
		return converters.Use(Converter.For.Parsable(false, formatProvder));
	}

	/// <inheritdoc cref="IUseJsonConverters.Parsable(bool)"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters, in bool tryParse)
	{
		return converters.Use(
#pragma warning disable CA1305 // Specify CultureInfo
			Converter.For.Parsable(tryParse)
#pragma warning restore CA1305 // Specify CultureInfo
		);
	}

	/// <inheritdoc cref="IUseJsonConverters.Parsable(bool, IFormatProvider)"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters, in bool tryParse, CultureInfo formatProvder)
	{
		return converters.Use(Converter.For.Parsable(tryParse, formatProvder));
	}

#endif
}

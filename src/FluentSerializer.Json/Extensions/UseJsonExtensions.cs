using Ardalis.GuardClauses;

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

	/// <inheritdoc cref="IUseJsonConverters.Formattable()"/>
	public static IConfigurationStack<IConverter> UseFormattable(this IConfigurationStack<IConverter> converters)
	{
		return converters.Use(UseJsonConverters.DefaultFormattableConverter);
	}

	/// <inheritdoc cref="IUseJsonConverters.Formattable(IFormatProvider)"/>
	public static IConfigurationStack<IConverter> UseFormattable(this IConfigurationStack<IConverter> converters, CultureInfo formatProvder)
	{
		return converters.Use(Converter.For.Formattable(formatProvder));
	}

	/// <inheritdoc cref="IUseJsonConverters.Formattable(string)"/>
	public static IConfigurationStack<IConverter> UseFormattable(this IConfigurationStack<IConverter> converters, in string formatString)
	{
		Guard.Against.NullOrWhiteSpace(formatString
#if NETSTANDARD2_1
			, nameof(formatString)
#endif
		);

		return converters.Use(
#pragma warning disable CA1305 // Specify CultureInfo
			Converter.For.Formattable(formatString)
#pragma warning restore CA1305 // Specify CultureInfo
		);
	}

	/// <inheritdoc cref="IUseJsonConverters.Formattable(string, IFormatProvider)"/>
	public static IConfigurationStack<IConverter> UseFormattable(this IConfigurationStack<IConverter> converters, in string formatString, CultureInfo formatProvder)
	{
		Guard.Against.NullOrWhiteSpace(formatString
#if NETSTANDARD2_1
			, nameof(formatString)
#endif
		);

		return converters.Use(Converter.For.Formattable(formatString, formatProvder));
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

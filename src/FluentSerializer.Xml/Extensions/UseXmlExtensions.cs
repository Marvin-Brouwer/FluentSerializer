using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting;

using System;
using System.Globalization;

namespace FluentSerializer.Xml.Extensions;

/// <summary>
/// Additional extensions to configure default converters.
/// </summary>
public static class UseXmlExtensions
{
	/// <inheritdoc cref="IUseXmlConverters.Enum(EnumFormats)"/>
	public static IConfigurationStack<IConverter> UseEnum(this IConfigurationStack<IConverter> converters, in EnumFormats format)
	{
		return converters.Use(Converter.For.Enum(format));
	}

	/// <inheritdoc cref="IUseXmlConverters.Formattable()"/>
	public static IConfigurationStack<IConverter> UseFormattable(this IConfigurationStack<IConverter> converters)
	{
		return converters.Use(UseXmlConverters.DefaultFormattableConverter);
	}

	/// <inheritdoc cref="IUseXmlConverters.Formattable(IFormatProvider)"/>
	public static IConfigurationStack<IConverter> UseFormattable(this IConfigurationStack<IConverter> converters, CultureInfo formatProvder)
	{
		return converters.Use(Converter.For.Formattable(formatProvder));
	}

	/// <inheritdoc cref="IUseXmlConverters.Formattable(string)"/>
	public static IConfigurationStack<IConverter> UseFormattable(this IConfigurationStack<IConverter> converters, in string formatString)
	{
		Guard.Against.NullOrWhiteSpace(formatString
#if NETSTANDARD
			, nameof(formatString)
#endif
		);

		return converters.Use(
#pragma warning disable CA1305 // Specify CultureInfo
			Converter.For.Formattable(formatString)
#pragma warning restore CA1305 // Specify CultureInfo
		);
	}

	/// <inheritdoc cref="IUseXmlConverters.Formattable(string, IFormatProvider)"/>
	public static IConfigurationStack<IConverter> UseFormattable(this IConfigurationStack<IConverter> converters, in string formatString, CultureInfo formatProvder)
	{
		Guard.Against.NullOrWhiteSpace(formatString
#if NETSTANDARD
			, nameof(formatString)
#endif
		);

		return converters.Use(Converter.For.Formattable(formatString, formatProvder));
	}

#if NET7_0_OR_GREATER

	/// <inheritdoc cref="IUseXmlConverters.Parsable()"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters)
	{
		return converters.Use(UseXmlConverters.DefaultParseConverter);
	}

	/// <inheritdoc cref="IUseXmlConverters.Parsable(IFormatProvider)"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters, IFormatProvider formatProvder)
	{
		return converters.Use(Converter.For.Parsable(false, formatProvder));
	}

	/// <inheritdoc cref="IUseXmlConverters.Parsable(bool)"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters, in bool tryParse)
	{
		return converters.Use(
#pragma warning disable CA1305 // Specify CultureInfo
			Converter.For.Parsable(tryParse)
#pragma warning restore CA1305 // Specify CultureInfo
		);
	}

	/// <inheritdoc cref="IUseXmlConverters.Parsable(bool, IFormatProvider)"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters, in bool tryParse, IFormatProvider formatProvder)
	{
		return converters.Use(Converter.For.Parsable(tryParse, formatProvder));
	}

#endif
}

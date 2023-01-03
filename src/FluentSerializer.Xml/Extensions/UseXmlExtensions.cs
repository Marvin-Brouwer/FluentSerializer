using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting;

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

#if NET7_0_OR_GREATER

	/// <inheritdoc cref="IUseXmlConverters.Parsable()"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters)
	{
		return converters.Use(UseXmlConverters.DefaultParseConverter);
	}

	/// <inheritdoc cref="IUseXmlConverters.Parsable(CultureInfo, bool)"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters, CultureInfo formatProvder, in bool tryParse = false)
	{
		return converters.Use(Converter.For.Parsable(formatProvder, tryParse));
	}

	/// <inheritdoc cref="IUseXmlConverters.Parsable(bool)"/>
	public static IConfigurationStack<IConverter> UseParsable(this IConfigurationStack<IConverter> converters, in bool tryParse)
	{
		return converters.Use(
#pragma warning disable CA1304 // Specify CultureInfo
			Converter.For.Parsable(tryParse)
#pragma warning restore CA1304 // Specify CultureInfo
		);
	}

#endif
}

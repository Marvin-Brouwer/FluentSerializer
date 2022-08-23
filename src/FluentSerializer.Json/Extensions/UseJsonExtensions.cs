using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.Json.Extensions;

/// <summary>
/// Additional extensions to configure default converters.
/// </summary>
public static class UseJsonExtensions
{
	/// <inheritdoc cref="IUseJsonConverters.Enum(EnumFormats, bool)"/>
	public static IConfigurationStack<IConverter> UseEnum(this IConfigurationStack<IConverter> converters, in EnumFormats format, in bool writeNumbersAsString = false)
	{
		return converters.Use(Converter.For.Enum(format, writeNumbersAsString)());
	}
}

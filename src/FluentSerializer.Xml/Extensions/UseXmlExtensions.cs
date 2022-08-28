using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting;

namespace FluentSerializer.Xml.Extensions;

/// <summary>
/// Additional extensions to configure default converters.
/// </summary>
public static class UseXmlExtensions
{
	/// <inheritdoc cref="IUseXmlConverters.Enum(EnumFormats)"/>
	public static IConfigurationStack<IConverter> UseEnum(this IConfigurationStack<IConverter> converters, in EnumFormats format)
	{
		return converters.Use(Converter.For.Enum(format)());
	}
}

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
	/// <inheritdoc cref="IUseXmlConverters.Enum(EnumFormat)"/>
	public static IConfigurationStack<IConverter> UseEnum(this IConfigurationStack<IConverter> converters, in EnumFormat format)
	{
		return converters.Use(Converter.For.Enum(format)());
	}
}

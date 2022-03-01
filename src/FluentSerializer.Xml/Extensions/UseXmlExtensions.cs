using System.Collections.Generic;
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
	public static List<IConverter> ReplaceEnumConverter(this List<IConverter> converters, in EnumFormat format)
	{
		var index = converters.IndexOf(UseXmlConverters.DefaultEnumConverter);
		converters[index] = Converter.For.Enum(format)();

		return converters;
	}
}

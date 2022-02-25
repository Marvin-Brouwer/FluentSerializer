using System.Collections.Generic;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.Json.Extensions;

/// <summary>
/// Additional extensions to configure default converters.
/// </summary>
public static class UseJsonExtensions
{
	/// <inheritdoc cref="IUseJsonConverters.Enum(EnumFormat, bool)"/>
	public static List<IConverter> ReplaceEnumConverter(this List<IConverter> converters, in EnumFormat format, in bool writeNumbersAsString = false)
	{
		var index = converters.IndexOf(UseJsonConverters.DefaultEnumConverter);
		converters[index] = Converter.For.Enum(format, writeNumbersAsString)();

		return converters;
	}
}

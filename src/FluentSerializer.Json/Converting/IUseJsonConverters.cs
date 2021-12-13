using System;
using System.Globalization;

namespace FluentSerializer.Json.Converting;

public interface IUseJsonConverters
{
	Func<IJsonConverter> Dates(string? format = null, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None);
	IJsonConverter Collection();
}
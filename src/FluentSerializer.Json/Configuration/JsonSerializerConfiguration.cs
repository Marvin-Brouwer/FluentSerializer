using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;

using System;
using System.Text;

namespace FluentSerializer.Json.Configuration;

/// <summary>
/// Configuration for the JSON serializer
/// </summary>
public sealed class JsonSerializerConfiguration : SerializerConfiguration
{
	/// <summary>
	/// A static reference to the default JSON Configuration
	/// </summary>
	public static readonly JsonSerializerConfiguration Default = new();

	/// <summary>
	/// The default naming strategy for all JSON properties
	/// </summary>
	public Func<INamingStrategy> DefaultNamingStrategy { get; set; }

	/// <inheritdoc cref="JsonSerializerConfiguration"/>
	public JsonSerializerConfiguration()
	{
		Encoding = Encoding.UTF8;
		FormatOutput = true;
		WriteNull = false;
		DefaultNamingStrategy = Names.Use.CamelCase;
		DefaultConverters = new ConfigurationStack<IConverter>
		{
			// Built-in converters
			UseJsonConverters.ConvertibleConverter,
			UseJsonConverters.DefaultEnumConverter,
			UseJsonConverters.DefaultDateTimeConverter,
			UseJsonConverters.DefaultDateTimeOffsetConverter,
#if NET5_0_OR_GREATER
			UseJsonConverters.DefaultDateOnlyConverter,
			UseJsonConverters.DefaultTimeOnlyConverter,
#endif
			UseJsonConverters.DefaultTimeSpanConverter,
			// Collection converters
			UseJsonConverters.CollectionConverter
		};
	}
}
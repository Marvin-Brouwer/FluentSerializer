using System;
using System.Collections.Generic;
using System.Text;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.Json.Configuration;

/// <summary>
/// Configuration for the JSON serializer
/// </summary>
public sealed class JsonSerializerConfiguration : SerializerConfiguration
{
	/// <summary>
	/// A static reference to the default JSON Configuration
	/// </summary>
	public static JsonSerializerConfiguration Default { get; } = new();

	/// <summary>
	/// The default naming strategy for all JSON properties
	/// </summary>
	public Func<INamingStrategy> DefaultNamingStrategy { get; set; }

	private JsonSerializerConfiguration()
	{
		Encoding = Encoding.UTF8;
		FormatOutput = true;
		WriteNull = false;
		DefaultNamingStrategy = Names.Use.CamelCase;
		DefaultConverters = new List<IConverter>
		{
			UseJsonConverters.DefaultEnumConverter,
			UseJsonConverters.DefaultDateConverter,
			UseJsonConverters.ConvertibleConverter,

			// Collection converters
			UseJsonConverters.CollectionConverter
		};
	}
}
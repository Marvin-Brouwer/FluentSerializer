using System;
using System.Collections.Generic;
using System.Text;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Xml.Converting;

namespace FluentSerializer.Xml.Configuration;

/// <summary>
/// Configuration for the XML serializer
/// </summary>
public sealed class XmlSerializerConfiguration : SerializerConfiguration
{
	/// <summary>
	/// A static reference to the default XML Configuration
	/// </summary>
	public static XmlSerializerConfiguration Default { get; } = new();

	/// <summary>
	/// The default naming strategy for all XML elements
	/// </summary>
	public Func<INamingStrategy> DefaultClassNamingStrategy { get; set; }
	/// <summary>
	/// The default naming strategy for all XML attributes
	/// </summary>
	public Func<INamingStrategy> DefaultPropertyNamingStrategy { get; set; }

	private XmlSerializerConfiguration()
	{
		Encoding = Encoding.Unicode;
		FormatOutput = true;
		WriteNull = false;
		DefaultClassNamingStrategy = Names.Use.PascalCase;
		DefaultPropertyNamingStrategy = Names.Use.CamelCase;
		DefaultConverters = new List<IConverter>
		{
			UseXmlConverters.DefaultEnumConverter,
			UseXmlConverters.DefaultDateConverter,
			UseXmlConverters.ConvertibleConverter,

			// Collection converters
			UseXmlConverters.WrappedCollectionConverter
		};
	}
}
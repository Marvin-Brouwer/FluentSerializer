using System;
using System.Collections.Generic;
using System.Text;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Xml.Converting;

namespace FluentSerializer.Xml.Configuration;

public sealed class XmlSerializerConfiguration : SerializerConfiguration
{
	public static XmlSerializerConfiguration Default { get; } = new();

	public Func<INamingStrategy> DefaultClassNamingStrategy { get; set; }
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
			UseXmlConverters.DefaultDateConverter,
			UseXmlConverters.ConvertibleConverter,

			// Collection converters
			UseXmlConverters.WrappedCollectionConverter
		};
	}
}
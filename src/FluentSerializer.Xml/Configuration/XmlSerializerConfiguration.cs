using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Xml.Converting;

using System;
using System.Text;

namespace FluentSerializer.Xml.Configuration;

/// <summary>
/// Configuration for the XML serializer
/// </summary>
public sealed class XmlSerializerConfiguration : SerializerConfiguration
{
	/// <summary>
	/// A static reference to the default XML Configuration
	/// </summary>
	public static readonly XmlSerializerConfiguration Default = new();

	/// <summary>
	/// The default naming strategy for all XML elements
	/// </summary>
	public Func<INamingStrategy> DefaultClassNamingStrategy { get; set; }
	/// <summary>
	/// The default naming strategy for all XML attributes
	/// </summary>
	public Func<INamingStrategy> DefaultPropertyNamingStrategy { get; set; }

	/// <inheritdoc cref="XmlSerializerConfiguration"/>
	public XmlSerializerConfiguration()
	{
		Encoding = Encoding.Unicode;
		FormatOutput = true;
		WriteNull = false;
		DefaultClassNamingStrategy = Names.Use.PascalCase;
		DefaultPropertyNamingStrategy = Names.Use.CamelCase;
		DefaultConverters = new ConfigurationStack<IConverter>(ConverterComparer.Default,
			// Built-in converters
			UseXmlConverters.ConvertibleConverter,
			UseXmlConverters.DefaultFormattableConverter,
#if NET7_0_OR_GREATER
			UseXmlConverters.DefaultParseConverter,
#endif
			UseXmlConverters.DefaultEnumConverter,
			UseXmlConverters.DefaultDateTimeConverter,
			UseXmlConverters.DefaultDateTimeOffsetConverter,
#if NET5_0_OR_GREATER
			UseXmlConverters.DefaultDateOnlyConverter,
			UseXmlConverters.DefaultTimeOnlyConverter,
#endif
			UseXmlConverters.DefaultTimeSpanConverter,
			// Collection converters
			UseXmlConverters.WrappedCollectionConverter
		);
	}
}
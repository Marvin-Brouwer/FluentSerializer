using FluentSerializer.Core.Factories;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Factory;
using Microsoft.Extensions.ObjectPool;
using System;

namespace FluentSerializer.Xml.Extensions;

/// <summary>
/// Extensions for gaining access to the <see cref="IXmlSerializerFactory"/>
/// </summary>
public static class SerializerFactoryExtensions
{
	private static IXmlSerializerFactory New() => new XmlSerializerFactory();

	/// <inheritdoc cref="IXmlSerializerFactory.WithDefaultConfiguration(in ObjectPoolProvider?)"/>
	public static IConfiguredXmlSerializerFactory Xml(this SerializerFactory _, in ObjectPoolProvider? poolProvider = null) => New()
		.WithDefaultConfiguration(in poolProvider);

	/// <inheritdoc cref="IXmlSerializerFactory.WithConfiguration(in XmlSerializerConfiguration, in ObjectPoolProvider?)"/>
	public static IConfiguredXmlSerializerFactory Xml(this SerializerFactory _, in XmlSerializerConfiguration configuration, in ObjectPoolProvider? poolProvider = null) =>
		New().WithConfiguration(in configuration, in poolProvider);

	/// <inheritdoc cref="IXmlSerializerFactory.WithConfiguration(in Action{XmlSerializerConfiguration}, in ObjectPoolProvider?)"/>
	public static IConfiguredXmlSerializerFactory Xml(this SerializerFactory _, in Action<XmlSerializerConfiguration> configurationSetup) =>
		New().WithConfiguration(configurationSetup);
	/// <inheritdoc cref="IXmlSerializerFactory.WithConfiguration(in Action{XmlSerializerConfiguration}, in ObjectPoolProvider?)"/>
	public static IConfiguredXmlSerializerFactory Xml(this SerializerFactory _, in ObjectPoolProvider poolProvider, in Action<XmlSerializerConfiguration> configurationSetup) =>
		New().WithConfiguration(in configurationSetup, in poolProvider);
}

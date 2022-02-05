using FluentSerializer.DependencyInjection.NetCoreDefault.Extensions;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FluentSerializer.DependencyInjection.Xml.NetCoreDefault.Extensions;

public static class DependencyInjectionExtensions
{
	private static readonly ServiceDescriptor RuntimeSerializerDescriptor = new(typeof(RuntimeXmlSerializer), typeof(RuntimeXmlSerializer), ServiceLifetime.Transient);

	public static IServiceCollection AddFluentXmlSerializer<TAssemblyMarker>(
		this IServiceCollection serviceCollection, in Action<XmlSerializerConfiguration> configurator) =>
		serviceCollection.AddFluentXmlSerializer(typeof(TAssemblyMarker).Assembly, configurator);
	public static IServiceCollection AddFluentXmlSerializer(
		this IServiceCollection serviceCollection, in Assembly assembly, in Action<XmlSerializerConfiguration> configurator)
	{
		var configuration = XmlSerializerConfiguration.Default;
		configurator(configuration);
		return serviceCollection.AddFluentXmlSerializer(assembly, configuration);
	}

	public static IServiceCollection AddFluentXmlSerializer<TAssemblyMarker>(
		this IServiceCollection serviceCollection, XmlSerializerConfiguration? configuration = null) =>
		serviceCollection.AddFluentXmlSerializer(typeof(TAssemblyMarker).Assembly, configuration);

	public static IServiceCollection AddFluentXmlSerializer(
		this IServiceCollection serviceCollection, in Assembly assembly, XmlSerializerConfiguration? configuration = null)
	{
		configuration ??= XmlSerializerConfiguration.Default;

		return serviceCollection
			.AddFluentSerializerServices(configuration)
			.AddFluentSerializerProfiles<XmlSerializerProfile, XmlSerializerConfiguration>(assembly, configuration)
			.AddRuntimeXmlSerializer();
	}

	public static IServiceCollection AddRuntimeXmlSerializer(this IServiceCollection serviceCollection)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.Add(RuntimeSerializerDescriptor);
		return serviceCollection
			.AddTransient<IAdvancedXmlSerializer, RuntimeXmlSerializer>(resolver => resolver.GetService<RuntimeXmlSerializer>()!)
			.AddTransient<IXmlSerializer, RuntimeXmlSerializer>(resolver => resolver.GetService<RuntimeXmlSerializer>()!);
	}
}

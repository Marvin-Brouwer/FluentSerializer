using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Factories;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Extensions;
using FluentSerializer.Xml.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Reflection;

namespace FluentSerializer.Xml.DependencyInjection.NetCoreDefault.Extensions;

/// <summary>
/// Extension class for registering the FluentSerializer for Xml
/// </summary>
public static class DependencyInjectionExtensions
{
	private static readonly ServiceDescriptor RuntimeSerializerDescriptor = new(typeof(IXmlSerializer), typeof(RuntimeXmlSerializer), ServiceLifetime.Transient);

	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using <see cref="XmlSerializerConfiguration.Default"/>
	/// And use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentXmlSerializer<TAssemblyMarker>(this IServiceCollection serviceCollection)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Xml(objectPoolProvider)
					.UseProfilesFromAssembly<TAssemblyMarker>();
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using <see cref="XmlSerializerConfiguration.Default"/>
	/// And use all the profiles found in the <paramref name="assembly"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentXmlSerializer(this IServiceCollection serviceCollection,
		Assembly assembly)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Xml(objectPoolProvider)
					.UseProfilesFromAssembly(assembly);
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using the provided <paramref name="configuration"/>
	/// And use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentXmlSerializer<TAssemblyMarker>(this IServiceCollection serviceCollection,
		XmlSerializerConfiguration configuration)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Xml(configuration, objectPoolProvider)
					.UseProfilesFromAssembly<TAssemblyMarker>();
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using the provided <paramref name="configuration"/>
	/// And use all the profiles found in the <paramref name="assembly"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentXmlSerializer(this IServiceCollection serviceCollection,
		Assembly assembly, XmlSerializerConfiguration configuration)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Xml(configuration, objectPoolProvider)
					.UseProfilesFromAssembly(assembly);
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using the provided <paramref name="configurationSetup"/>
	/// And use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentXmlSerializer<TAssemblyMarker>(this IServiceCollection serviceCollection,
		Action<XmlSerializerConfiguration> configurationSetup)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Xml(objectPoolProvider, configurationSetup)
					.UseProfilesFromAssembly<TAssemblyMarker>();
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using the provided <paramref name="configurationSetup"/>
	/// And use all the profiles found in the <paramref name="assembly"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentXmlSerializer(this IServiceCollection serviceCollection,
		Assembly assembly, Action<XmlSerializerConfiguration> configurationSetup)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Xml(objectPoolProvider, configurationSetup)
					.UseProfilesFromAssembly(assembly);
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	private static IServiceCollection AddInterFaceRegistrations(this IServiceCollection serviceCollection)
	{
		return serviceCollection
			.AddTransient(static resolver => (IAdvancedXmlSerializer)resolver.GetService<IXmlSerializer>()!);
	}
}

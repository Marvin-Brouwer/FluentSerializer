using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Factories;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Extensions;
using FluentSerializer.Json.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

using System;
using System.Reflection;

namespace FluentSerializer.Json.DependencyInjection.NetCoreDefault.Extensions;

/// <summary>
/// Extension class for registering the FluentSerializer for JSON
/// </summary>
public static class DependencyInjectionExtensions
{
	private static readonly ServiceDescriptor RuntimeSerializerDescriptor = new(typeof(IJsonSerializer), typeof(RuntimeJsonSerializer), ServiceLifetime.Transient);

	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using <see cref="JsonSerializerConfiguration.Default"/>
	/// And use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentJsonSerializer<TAssemblyMarker>(this IServiceCollection serviceCollection)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Json(objectPoolProvider)
					.UseProfilesFromAssembly<TAssemblyMarker>();
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using <see cref="JsonSerializerConfiguration.Default"/>
	/// And use all the profiles found in the <paramref name="assembly"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentJsonSerializer(this IServiceCollection serviceCollection,
		Assembly assembly)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Json(objectPoolProvider)
					.UseProfilesFromAssembly(assembly);
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using the provided <paramref name="configuration"/>
	/// And use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentJsonSerializer<TAssemblyMarker>(this IServiceCollection serviceCollection,
		JsonSerializerConfiguration configuration)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Json(configuration, objectPoolProvider)
					.UseProfilesFromAssembly<TAssemblyMarker>();
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using the provided <paramref name="configuration"/>
	/// And use all the profiles found in the <paramref name="assembly"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentJsonSerializer(this IServiceCollection serviceCollection,
		Assembly assembly, JsonSerializerConfiguration configuration)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Json(configuration, objectPoolProvider)
					.UseProfilesFromAssembly(assembly);
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using the provided <paramref name="configurationSetup"/>
	/// And use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentJsonSerializer<TAssemblyMarker>(this IServiceCollection serviceCollection,
		Action<JsonSerializerConfiguration> configurationSetup)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Json(objectPoolProvider, configurationSetup)
					.UseProfilesFromAssembly<TAssemblyMarker>();
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using the provided <paramref name="configurationSetup"/>
	/// And use all the profiles found in the <paramref name="assembly"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	public static IServiceCollection AddFluentJsonSerializer(this IServiceCollection serviceCollection,
		Assembly assembly, Action<JsonSerializerConfiguration> configurationSetup)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.AddTransient(s =>
			{
				var objectPoolProvider = s.GetService<ObjectPoolProvider>() ?? FactoryConstants.DefaultObjectPoolProvider;

				return SerializerFactory.For
					.Json(objectPoolProvider, configurationSetup)
					.UseProfilesFromAssembly(assembly);
			});

		return serviceCollection
			.AddInterFaceRegistrations();
	}

	private static IServiceCollection AddInterFaceRegistrations(this IServiceCollection serviceCollection)
	{
		return serviceCollection
			.AddTransient(static resolver => (IAdvancedJsonSerializer)resolver.GetService<IJsonSerializer>()!);
	}
}

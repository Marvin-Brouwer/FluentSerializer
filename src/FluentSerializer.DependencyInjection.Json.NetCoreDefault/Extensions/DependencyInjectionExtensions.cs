using FluentSerializer.DependencyInjection.NetCoreDefault.Extensions;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FluentSerializer.DependencyInjection.Json.NetCoreDefault.Extensions;

public static class DependencyInjectionExtensions
{
	private static readonly ServiceDescriptor RuntimeSerializerDescriptor = new(typeof(RuntimeJsonSerializer), typeof(RuntimeJsonSerializer), ServiceLifetime.Transient);

	public static IServiceCollection AddFluentJsonSerializer<TAssemblyMarker>(
		this IServiceCollection serviceCollection, in Action<JsonSerializerConfiguration> configurator) =>
		serviceCollection.AddFluentJsonSerializer(typeof(TAssemblyMarker).Assembly, configurator);
	public static IServiceCollection AddFluentJsonSerializer(
		this IServiceCollection serviceCollection, in Assembly assembly, in Action<JsonSerializerConfiguration> configurator)
	{
		var configuration = JsonSerializerConfiguration.Default;
		configurator(configuration);
		return serviceCollection.AddFluentJsonSerializer(assembly, configuration);
	}

	public static IServiceCollection AddFluentJsonSerializer<TAssemblyMarker>(
		this IServiceCollection serviceCollection, JsonSerializerConfiguration? configuration = null) =>
		serviceCollection.AddFluentJsonSerializer(typeof(TAssemblyMarker).Assembly, configuration);

	public static IServiceCollection AddFluentJsonSerializer(
		this IServiceCollection serviceCollection, in Assembly assembly, JsonSerializerConfiguration? configuration = null)
	{
		configuration ??= JsonSerializerConfiguration.Default;

		return serviceCollection
			.AddFluentSerializerServices(configuration)
			.AddFluentSerializerProfiles<JsonSerializerProfile, JsonSerializerConfiguration>(assembly, configuration)
			.AddRuntimeJsonSerializer();
	}

	public static IServiceCollection AddRuntimeJsonSerializer(this IServiceCollection serviceCollection)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.Add(RuntimeSerializerDescriptor);
		return serviceCollection
			.AddTransient<IAdvancedJsonSerializer, RuntimeJsonSerializer>(resolver => resolver.GetService<RuntimeJsonSerializer>()!)
			.AddTransient<IJsonSerializer, RuntimeJsonSerializer>(resolver => resolver.GetService<RuntimeJsonSerializer>()!);
	}
}

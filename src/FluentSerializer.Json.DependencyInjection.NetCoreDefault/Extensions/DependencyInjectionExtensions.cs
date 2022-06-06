using FluentSerializer.Core.DependencyInjection.NetCoreDefault.Extensions;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FluentSerializer.Json.DependencyInjection.NetCoreDefault.Extensions;

/// <summary>
/// Extension class for registering the FluentSerializer for JSON
/// </summary>
public static class DependencyInjectionExtensions
{
	private static readonly ServiceDescriptor RuntimeSerializerDescriptor = new(typeof(RuntimeJsonSerializer), typeof(RuntimeJsonSerializer), ServiceLifetime.Transient);

	/// <typeparam name="TAssemblyMarker">The assembly to scan for <see cref="JsonSerializerProfile"/></typeparam>
	/// <param name="serviceCollection"></param>
	/// <param name="configurator">A configuration lambda to configure this serializer</param>
	/// <returns></returns>
	/// <summary>
	/// Register the FluentSerializer for JSON
	/// </summary>
	public static IServiceCollection AddFluentJsonSerializer<TAssemblyMarker>(
		this IServiceCollection serviceCollection, in Action<JsonSerializerConfiguration> configurator) =>
		serviceCollection.AddFluentJsonSerializer(typeof(TAssemblyMarker).Assembly, in configurator);

	/// <param name="assembly">The assembly to scan for <see cref="JsonSerializerProfile"/></param>
	/// <param name="serviceCollection"></param>
	/// <param name="configurator">A configuration lambda to configure this serializer</param>
	/// <returns></returns>
	/// <inheritdoc cref="AddFluentJsonSerializer{TAssemblyMarker}(IServiceCollection, in Action{JsonSerializerConfiguration})"/>
	public static IServiceCollection AddFluentJsonSerializer(
		this IServiceCollection serviceCollection, in Assembly assembly, in Action<JsonSerializerConfiguration> configurator)
	{
		var configuration = new JsonSerializerConfiguration();
		configurator(configuration);
		return serviceCollection.AddFluentJsonSerializer(in assembly, configuration);
	}

	/// <typeparam name="TAssemblyMarker">The assembly to scan for <see cref="JsonSerializerProfile"/></typeparam>
	/// <param name="serviceCollection"></param>
	/// <param name="configuration">A configuration override for this serializer</param>
	/// <returns></returns>
	/// <inheritdoc cref="AddFluentJsonSerializer{TAssemblyMarker}(IServiceCollection, in Action{JsonSerializerConfiguration})"/>
	public static IServiceCollection AddFluentJsonSerializer<TAssemblyMarker>(
		this IServiceCollection serviceCollection, JsonSerializerConfiguration? configuration = null) =>
		serviceCollection.AddFluentJsonSerializer(typeof(TAssemblyMarker).Assembly, configuration);

	/// <param name="assembly">The assembly to scan for <see cref="JsonSerializerProfile"/></param>
	/// <param name="serviceCollection"></param>
	/// <param name="configuration">A configuration override for this serializer</param>
	/// <returns></returns>
	/// <inheritdoc cref="AddFluentJsonSerializer{TAssemblyMarker}(IServiceCollection, in Action{JsonSerializerConfiguration})"/>
	public static IServiceCollection AddFluentJsonSerializer(
		this IServiceCollection serviceCollection, in Assembly assembly, JsonSerializerConfiguration? configuration = null)
	{
		configuration ??= JsonSerializerConfiguration.Default;

		return serviceCollection
			.AddFluentSerializerServices(configuration)
			//.AddFluentSerializerProfiles<JsonSerializerProfile, JsonSerializerConfiguration>(in assembly, in configuration)
			.AddRuntimeJsonSerializer();
	}

	private static IServiceCollection AddRuntimeJsonSerializer(this IServiceCollection serviceCollection)
	{
		if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

		serviceCollection
			.Add(RuntimeSerializerDescriptor);
		return serviceCollection
			.AddTransient<IAdvancedJsonSerializer, RuntimeJsonSerializer>(static resolver => resolver.GetService<RuntimeJsonSerializer>()!)
			.AddTransient<IJsonSerializer, RuntimeJsonSerializer>(static resolver => resolver.GetService<RuntimeJsonSerializer>()!);
	}
}

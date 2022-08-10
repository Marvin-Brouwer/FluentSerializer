using FluentSerializer.Core.Factories;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Factory;

using Microsoft.Extensions.ObjectPool;

using System;

namespace FluentSerializer.Json.Extensions;

/// <summary>
/// Extensions for gaining access to the <see cref="IJsonSerializerFactory"/>
/// </summary>
public static class SerializerFactoryExtensions
{
	private static IJsonSerializerFactory New() => new JsonSerializerFactory();

	/// <inheritdoc cref="IJsonSerializerFactory.WithDefaultConfiguration(in ObjectPoolProvider?)"/>
	public static IConfiguredJsonSerializerFactory Json(this SerializerFactory _, in ObjectPoolProvider? poolProvider = null) => New()
		.WithDefaultConfiguration(in poolProvider);

	/// <inheritdoc cref="IJsonSerializerFactory.WithConfiguration(in JsonSerializerConfiguration, in ObjectPoolProvider?)"/>
	public static IConfiguredJsonSerializerFactory Json(this SerializerFactory _, in JsonSerializerConfiguration configuration, in ObjectPoolProvider? poolProvider = null) =>
		New().WithConfiguration(in configuration, in poolProvider);

	/// <inheritdoc cref="IJsonSerializerFactory.WithConfiguration(in Action{JsonSerializerConfiguration}, in ObjectPoolProvider?)"/>
	public static IConfiguredJsonSerializerFactory Json(this SerializerFactory _, in Action<JsonSerializerConfiguration> configurationSetup) =>
		New().WithConfiguration(configurationSetup);
	/// <inheritdoc cref="IJsonSerializerFactory.WithConfiguration(in Action{JsonSerializerConfiguration}, in ObjectPoolProvider?)"/>
	public static IConfiguredJsonSerializerFactory Json(this SerializerFactory _, in ObjectPoolProvider poolProvider, in Action<JsonSerializerConfiguration> configurationSetup) =>
		New().WithConfiguration(in configurationSetup, in poolProvider);
}

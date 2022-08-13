using FluentSerializer.Core.Factories;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Services;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Json.Factory;

/// <inheritdoc cref="ISerializerFactory{TSerializer, TConfiguration, TSerializerProfile}"/>
public interface IJsonSerializerFactory :
	ISerializerFactory<IJsonSerializer, JsonSerializerConfiguration, JsonSerializerProfile>
{
	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using <see cref="JsonSerializerConfiguration.Default"/>
	/// </summary>
	new IConfiguredJsonSerializerFactory WithDefaultConfiguration(in ObjectPoolProvider? poolProvider = null);
	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using the provided <paramref name="configuration"/>
	/// </summary>
	new IConfiguredJsonSerializerFactory WithConfiguration(in JsonSerializerConfiguration configuration, in ObjectPoolProvider? poolProvider = null);
	/// <summary>
	/// Create a new <see cref="IJsonSerializer"/> using the provided <paramref name="configurationSetup"/>
	/// </summary>
	new IConfiguredJsonSerializerFactory WithConfiguration(in Action<JsonSerializerConfiguration> configurationSetup, in ObjectPoolProvider? poolProvider = null);

}

/// <inheritdoc cref="IConfiguredSerializerFactory{TSerializer, TConfiguration, TSerializerProfile}"/>
public interface IConfiguredJsonSerializerFactory :
	IConfiguredSerializerFactory<IJsonSerializer, JsonSerializerConfiguration, JsonSerializerProfile>
{
	/// <summary>
	/// Use the <paramref name="profile"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	new IJsonSerializer UseProfile(in JsonSerializerProfile profile);
	/// <summary>
	/// Use the <paramref name="profiles"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	new IJsonSerializer UseProfiles(in IReadOnlyCollection<JsonSerializerProfile> profiles);
	/// <summary>
	/// Use all the profiles found in the <paramref name="assembly"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	new IJsonSerializer UseProfilesFromAssembly(in Assembly assembly);
	/// <summary>
	/// Use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <see cref="IJsonSerializer"/>
	/// </summary>
	new IJsonSerializer UseProfilesFromAssembly<TAssemblyMarker>();
}

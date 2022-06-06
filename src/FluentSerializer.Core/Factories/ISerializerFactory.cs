using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.Services;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Factories;

/// <summary>
/// Interface responsible for creating a new <see cref="ISerializer"/>
/// </summary>
public interface ISerializerFactory<TSerializer, TConfiguration, TSerializerProfile>
	where TSerializer : ISerializer
	where TConfiguration : ISerializerConfiguration, new()
	where TSerializerProfile : ISerializerProfile<TConfiguration>
{
	/// <summary>
	/// Create a new <typeparamref name="TSerializer"/> using the default configuration setup
	/// </summary>
	IConfiguredSerializerFactory<TSerializer, TConfiguration, TSerializerProfile> WithDefaultConfiguration(in ObjectPoolProvider? poolProvider = null);

	/// <summary>
	/// Create a new <typeparamref name="TSerializer"/> using the provided configuration
	/// </summary>
	IConfiguredSerializerFactory<TSerializer, TConfiguration, TSerializerProfile> WithConfiguration(in TConfiguration configuration, in ObjectPoolProvider? poolProvider = null);

	/// <summary>
	/// Create a new <typeparamref name="TSerializer"/> using the provided configuration, it's recommended to use a static delategate
	/// </summary>
	IConfiguredSerializerFactory<TSerializer, TConfiguration, TSerializerProfile> WithConfiguration(in Action<TConfiguration> configurationSetup, in ObjectPoolProvider? poolProvider = null);
}

/// <summary>
/// Interface for configuring the serializer with (additional) Profiles
/// </summary>
public interface IConfiguredSerializerFactory<TSerializer, TConfiguration, TSerializerProfile>
	where TSerializer : ISerializer
	where TConfiguration : ISerializerConfiguration, new()
	where TSerializerProfile : ISerializerProfile<TConfiguration>
{
	/// <summary>
	/// Use the <paramref name="profile"/> to configure the <typeparamref name="TSerializer"/>
	/// </summary>
	TSerializer UseProfile(in TSerializerProfile profile);
	/// <summary>
	/// Use the <paramref name="profiles"/> to configure the <typeparamref name="TSerializer"/>
	/// </summary>
	TSerializer UseProfiles(in IReadOnlyCollection<TSerializerProfile> profiles);
	/// <summary>
	/// Use all the profiles found in the <paramref name="assembly"/> to configure the <typeparamref name="TSerializer"/>
	/// </summary>
	TSerializer UseProfilesFromAssembly(in Assembly assembly);
	/// <summary>
	/// Use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <typeparamref name="TSerializer"/>
	/// </summary>
	TSerializer UseProfilesFromAssembly<TAssemblyMarker>();
}

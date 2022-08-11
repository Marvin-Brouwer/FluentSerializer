using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.Services;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Factories;

/// <inheritdoc cref="ISerializerFactory{TSerializer, TConfiguration, TSerializerProfile}"/>
public abstract class BaseSerializerFactory<TSerializer, TConfiguration, TSerializerProfile> :
	ISerializerFactory<TSerializer, TConfiguration, TSerializerProfile>,
	IConfiguredSerializerFactory<TSerializer, TConfiguration, TSerializerProfile>
	where TSerializer : ISerializer
	where TConfiguration : ISerializerConfiguration, new()
	where TSerializerProfile : ISerializerProfile<TConfiguration>
{
	private TConfiguration? _currentConfiguration;

	/// <summary>
	/// The <typeparamref name="TConfiguration"/> currently configured, defaults to the <see cref="DefaultConfiguration"/>
	/// </summary>
	protected TConfiguration CurrentConfiguration {
		get => _currentConfiguration ?? DefaultConfiguration;
		set => _currentConfiguration = value;
	}

	/// <summary>
	/// The <see cref="ObjectPoolProvider"/> currently configured, defaults to <see cref="FactoryConstants.DefaultObjectPoolProvider"/>
	/// </summary>
	protected ObjectPoolProvider CurrentObjectPoolProvider { get; set; } = FactoryConstants.DefaultObjectPoolProvider;

	/// <summary>
	/// The <typeparamref name="TConfiguration"/> to default to
	/// </summary>
	protected abstract TConfiguration DefaultConfiguration { get; }

	/// <summary>
	/// The eventual method that will be called once the factory calls the last configuration method.
	/// </summary>
	protected abstract TSerializer CreateSerializer(in TConfiguration configuration, in ObjectPoolProvider poolProvider,
		in IReadOnlyCollection<IClassMap> mappings);

	/// <inheritdoc/>
	public IConfiguredSerializerFactory<TSerializer, TConfiguration, TSerializerProfile> WithConfiguration(in TConfiguration configuration, in ObjectPoolProvider? poolProvider = null)
	{
		Guard.Against.Null(configuration, nameof(configuration));

		CurrentConfiguration = configuration;
		CurrentObjectPoolProvider = poolProvider ?? FactoryConstants.DefaultObjectPoolProvider;

		return this;
	}

	/// <inheritdoc/>
	public IConfiguredSerializerFactory<TSerializer, TConfiguration, TSerializerProfile> WithConfiguration(in Action<TConfiguration> configurationSetup, in ObjectPoolProvider? poolProvider = null)
	{
		Guard.Against.Null(configurationSetup, nameof(configurationSetup));

		var configuration = new TConfiguration();
		configurationSetup(configuration);

		return WithConfiguration(configuration, in poolProvider);
	}

	/// <inheritdoc/>
	public IConfiguredSerializerFactory<TSerializer, TConfiguration, TSerializerProfile> WithDefaultConfiguration(in ObjectPoolProvider? poolProvider = null)
	{
		return WithConfiguration(DefaultConfiguration, in poolProvider);
	}

	/// <inheritdoc/>
	public TSerializer UseProfilesFromAssembly(in Assembly assembly)
	{
		var profiles = ProfileScanner.ScanAssembly<TSerializerProfile, TConfiguration>(assembly);

		return UseProfiles(profiles.ToList());
	}

	/// <inheritdoc/>
	public TSerializer UseProfilesFromAssembly<TAssemblyMarker>()
	{
		return UseProfilesFromAssembly(typeof(TAssemblyMarker).Assembly);
	}

	/// <inheritdoc/>
	public TSerializer UseProfile(in TSerializerProfile profile)
	{
		return UseProfiles(new[] { profile });
	}

	/// <inheritdoc/>
	public TSerializer UseProfiles(in IReadOnlyCollection<TSerializerProfile> profiles)
	{
		var classMaps = ProfileScanner
			.FindClassMapsInProfiles(profiles, CurrentConfiguration)
			.ToArray();

		return CreateSerializer(CurrentConfiguration, CurrentObjectPoolProvider, classMaps);
	}
}

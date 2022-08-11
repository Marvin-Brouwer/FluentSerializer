using FluentSerializer.Core.Factories;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Services;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Json.Factory;

internal sealed class JsonSerializerFactory : BaseSerializerFactory<IJsonSerializer, JsonSerializerConfiguration, JsonSerializerProfile>,
	IJsonSerializerFactory,
	IConfiguredJsonSerializerFactory
{
	protected override JsonSerializerConfiguration DefaultConfiguration => JsonSerializerConfiguration.Default;

	protected override IJsonSerializer CreateSerializer(in JsonSerializerConfiguration configuration, in ObjectPoolProvider poolProvider, in IReadOnlyCollection<IClassMap> mappings)
	{
		var classMapCollection = new ClassMapCollection(in mappings);
		return new RuntimeJsonSerializer(in configuration, in poolProvider, classMapCollection);
	}

	IJsonSerializer IConfiguredJsonSerializerFactory.UseProfilesFromAssembly(in Assembly assembly)
	{
		return UseProfilesFromAssembly(in assembly);
	}

	IJsonSerializer IConfiguredJsonSerializerFactory.UseProfilesFromAssembly<TAssemblyMarker>()
	{
		return UseProfilesFromAssembly<TAssemblyMarker>();
	}

	IJsonSerializer IConfiguredJsonSerializerFactory.UseProfile(in JsonSerializerProfile profile)
	{
		return UseProfile(in profile);
	}

	IJsonSerializer IConfiguredJsonSerializerFactory.UseProfiles(in IReadOnlyCollection<JsonSerializerProfile> profiles)
	{
		return UseProfiles(in profiles);
	}

	IConfiguredJsonSerializerFactory IJsonSerializerFactory.WithConfiguration(in JsonSerializerConfiguration configuration, in ObjectPoolProvider? poolProvider)
	{
		return (IConfiguredJsonSerializerFactory)WithConfiguration(in configuration, in poolProvider);
	}

	IConfiguredJsonSerializerFactory IJsonSerializerFactory.WithConfiguration(in Action<JsonSerializerConfiguration> configurationSetup, in ObjectPoolProvider? poolProvider)
	{
		return (IConfiguredJsonSerializerFactory)WithConfiguration(in configurationSetup, in poolProvider);
	}

	IConfiguredJsonSerializerFactory IJsonSerializerFactory.WithDefaultConfiguration(in ObjectPoolProvider? poolProvider)
	{
		return (IConfiguredJsonSerializerFactory)WithDefaultConfiguration(in poolProvider);
	}
}

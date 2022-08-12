using FluentSerializer.Core.Factories;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Services;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Xml.Factory;

internal sealed class XmlSerializerFactory : BaseSerializerFactory<IXmlSerializer, XmlSerializerConfiguration, XmlSerializerProfile>,
	IXmlSerializerFactory,
	IConfiguredXmlSerializerFactory
{
	protected override XmlSerializerConfiguration DefaultConfiguration => XmlSerializerConfiguration.Default;

	protected override IXmlSerializer CreateSerializer(in XmlSerializerConfiguration configuration, in ObjectPoolProvider poolProvider, in IReadOnlyCollection<IClassMap> mappings)
	{
		var classMapCollection = new ClassMapCollection(in mappings);
		return new RuntimeXmlSerializer(in configuration, in poolProvider, classMapCollection);
	}

	IXmlSerializer IConfiguredXmlSerializerFactory.UseProfilesFromAssembly(in Assembly assembly)
	{
		return UseProfilesFromAssembly(in assembly);
	}

	IXmlSerializer IConfiguredXmlSerializerFactory.UseProfilesFromAssembly<TAssemblyMarker>()
	{
		return UseProfilesFromAssembly<TAssemblyMarker>();
	}

	IXmlSerializer IConfiguredXmlSerializerFactory.UseProfile(in XmlSerializerProfile profile)
	{
		return UseProfile(in profile);
	}

	IXmlSerializer IConfiguredXmlSerializerFactory.UseProfiles(in IReadOnlyCollection<XmlSerializerProfile> profiles)
	{
		return UseProfiles(in profiles);
	}

	IConfiguredXmlSerializerFactory IXmlSerializerFactory.WithConfiguration(in XmlSerializerConfiguration configuration, in ObjectPoolProvider? poolProvider)
	{
		return (IConfiguredXmlSerializerFactory)WithConfiguration(in configuration, in poolProvider);
	}

	IConfiguredXmlSerializerFactory IXmlSerializerFactory.WithConfiguration(in Action<XmlSerializerConfiguration> configurationSetup, in ObjectPoolProvider? poolProvider)
	{
		return (IConfiguredXmlSerializerFactory)WithConfiguration(in configurationSetup, in poolProvider);
	}

	IConfiguredXmlSerializerFactory IXmlSerializerFactory.WithDefaultConfiguration(in ObjectPoolProvider? poolProvider)
	{
		return (IConfiguredXmlSerializerFactory)WithDefaultConfiguration(in poolProvider);
	}
}

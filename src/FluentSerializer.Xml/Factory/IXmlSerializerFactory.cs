using FluentSerializer.Core.Factories;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Services;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Xml.Factory;

/// <inheritdoc cref="ISerializerFactory{TSerializer, TConfiguration, TSerializerProfile}"/>
public interface IXmlSerializerFactory :
	ISerializerFactory<IXmlSerializer, XmlSerializerConfiguration, XmlSerializerProfile>
{
	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using <see cref="XmlSerializerConfiguration.Default"/>
	/// </summary>
	new IConfiguredXmlSerializerFactory WithDefaultConfiguration(in ObjectPoolProvider? poolProvider = null);
	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using the provided <paramref name="configuration"/>
	/// </summary>
	new IConfiguredXmlSerializerFactory WithConfiguration(in XmlSerializerConfiguration configuration, in ObjectPoolProvider? poolProvider = null);
	/// <summary>
	/// Create a new <see cref="IXmlSerializer"/> using the provided <paramref name="configurationSetup"/>
	/// </summary>
	new IConfiguredXmlSerializerFactory WithConfiguration(in Action<XmlSerializerConfiguration> configurationSetup, in ObjectPoolProvider? poolProvider = null);

}

/// <inheritdoc cref="IConfiguredSerializerFactory{TSerializer, TConfiguration, TSerializerProfile}"/>
public interface IConfiguredXmlSerializerFactory :
	IConfiguredSerializerFactory<IXmlSerializer, XmlSerializerConfiguration, XmlSerializerProfile>
{
	/// <summary>
	/// Use the <paramref name="profile"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	new IXmlSerializer UseProfile(in XmlSerializerProfile profile);
	/// <summary>
	/// Use the <paramref name="profiles"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	new IXmlSerializer UseProfiles(in IReadOnlyCollection<XmlSerializerProfile> profiles);
	/// <summary>
	/// Use all the profiles found in the <paramref name="assembly"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	new IXmlSerializer UseProfilesFromAssembly(in Assembly assembly);
	/// <summary>
	/// Use all the profiles found in the <typeparamref name="TAssemblyMarker"/> to configure the <see cref="IXmlSerializer"/>
	/// </summary>
	new IXmlSerializer UseProfilesFromAssembly<TAssemblyMarker>();
}

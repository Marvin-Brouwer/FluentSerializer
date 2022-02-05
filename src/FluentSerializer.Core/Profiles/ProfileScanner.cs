using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;

namespace FluentSerializer.Core.Profiles;

/// <summary>
/// Helper class to find <see cref="ISerializerProfile"/>s
/// </summary>
public static class ProfileScanner
{
	private static IEnumerable<ISerializerProfile> ScanAssembly<TSerializerProfile>(Assembly assembly) where TSerializerProfile : ISerializerProfile =>
		assembly.GetTypes()
			.Where(type => typeof(TSerializerProfile).IsAssignableFrom(type))
			.Where(type => !type.IsAbstract)
			.Select(type => (ISerializerProfile)Activator.CreateInstance(type)!);

	/// <summary>
	/// Find all profiles of type <typeparamref name="TSerializerProfile"/> in the given <paramref name="assembly"/>,
	/// generate the profile definitions and push into a <see cref="ClassMapScanList"/>
	/// </summary>
	public static ClassMapScanList FindClassMapsInAssembly<TSerializerProfile>(
		Assembly assembly, SerializerConfiguration configuration)
		where TSerializerProfile : ISerializerProfile
	{
		Guard.Against.Null(assembly, nameof(assembly));

		var profiles = ScanAssembly<TSerializerProfile>(assembly);

		return new ClassMapScanList(profiles.SelectMany(profile => profile.Configure(configuration)).ToList().AsReadOnly());
	}
}
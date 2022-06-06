using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;

namespace FluentSerializer.Core.Profiles;

/// <summary>
/// Helper class to find <see cref="ISerializerProfile{TConfiguration}"/>s
/// </summary>
public static class ProfileScanner
{
	/// <summary>
	/// Scan the assembly for all profiles of type <typeparamref name="TSerializerProfile"/>
	/// </summary>
	public static IEnumerable<TSerializerProfile> ScanAssembly<TSerializerProfile, TConfiguration>(Assembly assembly)
		where TSerializerProfile : ISerializerProfile<TConfiguration>
		where TConfiguration : ISerializerConfiguration
	{
		foreach (var type in assembly.GetTypes())
		{
			if (type.IsAbstract) continue;
			if (!typeof(TSerializerProfile).IsAssignableFrom(type)) continue;

			yield return (TSerializerProfile)Activator.CreateInstance(type)!;
		}
	}

	/// <summary>
	/// Find all profiles of type <typeparamref name="TSerializerProfile"/> in the given <paramref name="assembly"/>,
	/// generate the profile definitions and push into an <see cref="IClassMapScanList{TSerializer, TConfiguration}"/>
	/// </summary>
	[Obsolete("Obsolete", false)]
	public static IClassMapScanList<TSerializerProfile, TConfiguration> FindClassMapsInAssembly<TSerializerProfile, TConfiguration>(
		in Assembly assembly, TConfiguration configuration)
		where TSerializerProfile : ISerializerProfile<TConfiguration>
		where TConfiguration : ISerializerConfiguration
	{
		Guard.Against.Null(assembly, nameof(assembly));
		Guard.Against.Null(configuration, nameof(configuration));

		var profiles = ScanAssembly<TSerializerProfile, TConfiguration>(assembly);
		var classMaps = FindClassMapsInProfiles(profiles, configuration).ToList();

		return new ClassMapScanList<TSerializerProfile, TConfiguration>(classMaps);
	}

	/// <summary>
	/// Get all <see cref="IClassMap"/>s from a list of <typeparamref name="TSerializerProfile"/>s
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static IEnumerable<IClassMap> FindClassMapsInProfiles<TSerializerProfile, TConfiguration>(
		IEnumerable<TSerializerProfile> profiles, TConfiguration configuration)
		where TSerializerProfile : ISerializerProfile<TConfiguration>
		where TConfiguration : ISerializerConfiguration
	{
		foreach (var profile in profiles)
		{
			var classMaps = FindClassMapsInProfile(in profile, in configuration);
			foreach (var classMap in classMaps) yield return classMap;
		}
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static IEnumerable<IClassMap> FindClassMapsInProfile<TSerializerProfile, TConfiguration>(
		in TSerializerProfile profile, in TConfiguration configuration)
		where TSerializerProfile : ISerializerProfile<TConfiguration>
		where TConfiguration : ISerializerConfiguration
	{
		return profile.Configure(configuration);
	}
}
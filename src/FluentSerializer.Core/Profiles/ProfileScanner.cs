using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
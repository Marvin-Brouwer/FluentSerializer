using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.DependencyInjection.NetCoreDefault.Extensions;

/// <summary>
/// Extension class for registering FluentSerializers required services.
/// </summary>
public static class DependencyInjectionExtensions
{
	/// <summary>
	/// Add the services required for the FluentSerializer to work <br />
	/// Registers the following: <br />
	/// - The <see cref="DefaultObjectPoolProvider"/> as singleton, given there is no registration for <see cref="ObjectPoolProvider"/><br />
	/// - The <typeparamref name="TConfiguration"/> as singleton, given it has not been registered yet.
	/// </summary>
	public static IServiceCollection AddFluentSerializerServices<TConfiguration>(
		this IServiceCollection serviceCollection, TConfiguration configuration)
		where TConfiguration : SerializerConfiguration
	{
		// We don't care about what provider to use, as long as there is one
		if (!serviceCollection.ContainsServiceRegistrationFor<ObjectPoolProvider>())
			serviceCollection.AddSingleton<ObjectPoolProvider>(static _ => new DefaultObjectPoolProvider());

		if (!serviceCollection.ContainsServiceRegistrationFor<TConfiguration>())
			serviceCollection.AddSingleton(_ => configuration);

		return serviceCollection;
	}

	/// <summary>
	/// Register <typeparamref name="TSerializerProfile"/>s found in <paramref name="assembly"/>. <br />
	/// If there already are <typeparamref name="TSerializerProfile"/>s present, they will be appended to.
	/// </summary>
	public static IServiceCollection AddFluentSerializerProfiles<TSerializerProfile, TConfiguration>(
		this IServiceCollection serviceCollection, in Assembly assembly, in TConfiguration configuration)
		where TSerializerProfile : class, ISerializerProfile<TConfiguration>
		where TConfiguration : SerializerConfiguration
	{
		var existingMappings = serviceCollection.FindRegistrationFor<IClassMapScanList<TSerializerProfile, TConfiguration>>();
		var mappings = ProfileScanner.FindClassMapsInAssembly<TSerializerProfile, TConfiguration>(in assembly, configuration);

		if (existingMappings is null)
			serviceCollection.AddScoped(_ => mappings);
		else
			serviceCollection.AddScoped<IClassMapScanList<TSerializerProfile, TConfiguration>>(_ =>
				((ClassMapScanList<TSerializerProfile, TConfiguration>)existingMappings).Append(mappings));

		return serviceCollection;
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static bool ContainsServiceRegistrationFor<TServiceType>(this IServiceCollection serviceCollection)
	{
		foreach (var service in serviceCollection)
			if (service.ServiceType.IsAssignableFrom(typeof(TServiceType))) return true;

		return false;
	}

#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	private static TServiceType? FindRegistrationFor<TServiceType>(this IServiceCollection serviceCollection)
		where TServiceType : class
	{
		foreach (var service in serviceCollection)
			if (service.ServiceType.IsAssignableFrom(typeof(TServiceType))) return service.ImplementationInstance as TServiceType;

		return default;
	}
}


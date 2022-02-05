using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.DependencyInjection.NetCoreDefault.Extensions;

public static class DependencyInjectionExtensions
{
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

	public static IServiceCollection AddFluentSerializerProfiles<TSerializerProfile, TConfiguration>(
		this IServiceCollection serviceCollection, in Assembly assembly, TConfiguration configuration)
		where TSerializerProfile : class, ISerializerProfile
		where TConfiguration : SerializerConfiguration
	{
		var existingMappings = serviceCollection.FindRegistrationFor<ClassMapScanList>();
		var mappings = ProfileScanner.FindClassMapsInAssembly<TSerializerProfile>(assembly, configuration);

		if (existingMappings is null)
			serviceCollection.AddScoped(_ => mappings);
		else
			serviceCollection.AddScoped(_ => existingMappings.Append(mappings));

		return serviceCollection;
	}

	private static bool ContainsServiceRegistrationFor<TServiceType>(this IServiceCollection serviceCollection)
	{
		foreach (var service in serviceCollection)
			if (service.ServiceType.IsAssignableFrom(typeof(TServiceType))) return true;

		return false;
	}
	private static TServiceType? FindRegistrationFor<TServiceType>(this IServiceCollection serviceCollection)
		where TServiceType : class
	{
		foreach (var service in serviceCollection)
			if (service.ServiceType.IsAssignableFrom(typeof(TServiceType))) return service.ImplementationInstance as TServiceType;

		return default;
	}
}


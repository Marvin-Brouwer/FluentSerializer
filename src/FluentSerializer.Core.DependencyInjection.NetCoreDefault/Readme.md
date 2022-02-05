# FluentSerializer.Core.DependencyInjection.NetCoreDefault
[DependencyInjectionNuget]: (https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)

This library contains some basic logic to help register the specific serializer using
[Microsoft.Extensions.DependencyInjection.Abstractions][DependencyInjectionNuget].

## Usage

To setup the FluentSerializer you need to do the following three things:
  
**Firstly** you need to register the required services.
```cs
serviceCollection.AddFluentSerializerServices(configuration);
```
This will register the configuration required bij several parts of the serializer,
plus it will ensure there is an objectpool provider available.
This method can be called multiple times and will only do something when services aren't registered yet.
So in short it will only do something once.

**Next** you need to register an assembly to scan for profiles _at least once_.
This method can be called for multiple assemblies and will append to previously registered results.
```cs
serviceCollection.AddFluentSerializerProfiles<TSerializerProfile, TSerializerConfiguration>(assembly, configuration);
```

**Finally** it's up to your library's method to register itself.
Please make sure you only register the services once - you can use XML or JSON as a reference - so that
your registration extension method can be called multiple times for multiple assemblies without side effects.
```cs
serviceCollection.AddRuntimeTSerializer();
```
```cs
private static readonly ServiceDescriptor RuntimeSerializerDescriptor = new(typeof(RuntimeTSerializer), typeof(RuntimeTSerializer), ServiceLifetime.Transient);

public static IServiceCollection AddRuntimeTSerializer(this IServiceCollection serviceCollection)
{
	if (serviceCollection.Contains(RuntimeSerializerDescriptor)) return serviceCollection;

	serviceCollection
		.Add(RuntimeSerializerDescriptor);
	return serviceCollection
		.AddTransient<IAdvancedTSerializer, RuntimeTSerializer>(resolver => resolver.GetService<RuntimeTSerializer>()!)
		.AddTransient<ITSerializer, RuntimeTSerializer>(resolver => resolver.GetService<RuntimeTSerializer>()!);
}
```
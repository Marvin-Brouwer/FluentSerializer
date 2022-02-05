# FluentSerializer.Json.DependencyInjection.NetCoreDefault
[DependencyInjectionNuget]: (https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)

This library is responsible for registering the FluentSerializer for JSON using
[Microsoft.Extensions.DependencyInjection.Abstractions][DependencyInjectionNuget].

## Usage

For basic usage you can use this:  
```cs
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>();
```
This will use the `JsonSerializerConfiguration.Default` as the applied config.
The type parameter of `TAssemblyMarker` will be used to scan that assembly for the profiles associated with this serializer.
You can call this registration multiple times with different assemblies for additional profiles.
Alternatively there are overloads that accept a `System.Reflection.Assembly` variable.  
  
There are multiple overloads, for changing configuration the lambda approach is recomended:  
```cs
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	// Change configuration values
	configuration.NewLine = LineEndings.LineFeed;
});
```
This will use the `JsonSerializerConfiguration.Default` as the applied config and allows you to change some properties.

## Default Configuration

- **Encoding** = `Encoding.UTF8`
- **FormatOutput** = true
- **WriteNull** = false
- **DefaultNamingStrategy** = camelCase
- **DefaultConverters**
  - Converter that can handle DateTime objects (JSON spec compliant)
  - Converter that can handle IConvertable types
  - Converter to handle collection types
[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.json.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/src/FluentSerializer.Json.DependencyInjection.NetCoreDefault/Readme.md#readme">
		FluentSerializer.Json.DependencyInjection.NetCoreDefault
	</a>
</h1>

[//]: # (Body)
[DependencyInjectionNuget]: (https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)

This library is responsible for registering the FluentSerializer for JSON using
[Microsoft.Extensions.DependencyInjection.Abstractions][DependencyInjectionNuget].

## Usage
[configuration-doc]: https://github.com/Marvin-Brouwer/FluentSerializer/src/FluentSerializer.Json/Readme.md#configuration

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
This will use the `JsonSerializerConfiguration.Default` as the applied config and allows you to [change some properties][configuration-doc].
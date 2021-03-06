[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.xml.optimized.svg" />
	<a href="/src/FluentSerializer.Xml.DependencyInjection.NetCoreDefault#readme">
		FluentSerializer.Xml.DependencyInjection.NetCoreDefault
	</a>
</h1>

[//]: # (Body)
[DependencyInjectionNuget]: (https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)

The `FluentSerializer.Xml.DependencyInjection.NetCoreDefault` library is responsible for registering the FluentSerializer for XML using
[Microsoft.Extensions.DependencyInjection.Abstractions][DependencyInjectionNuget].

## Usage

[configuration-doc]: /src/FluentSerializer.Xml/Readme.md#configuration

For basic usage you can use this:

```csharp
serviceCollection.AddFluentXmlSerializer<TAssemblyMarker>();
```

This will use the `XmlSerializerConfiguration.Default` as the applied config.
The type parameter of `TAssemblyMarker` will be used to scan that assembly for the profiles associated with this serializer.
Alternatively there are overloads that accept a `System.Reflection.Assembly` variable.

There are multiple overloads, for changing configuration the lambda approach is recommended:

```csharp
serviceCollection.AddFluentXmlSerializer<TAssemblyMarker>(static configuration =>
{
	// Change configuration values
	configuration.NewLine = LineEndings.LineFeed;
});
```

This will use a new instance of `XmlSerializerConfiguration` as the applied config and allows you to change some properties.
[See the default values][configuration-doc].

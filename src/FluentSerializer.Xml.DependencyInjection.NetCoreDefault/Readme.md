# FluentSerializer.Xml.DependencyInjection.NetCoreDefault
[DependencyInjectionNuget]: (https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/)

This library is responsible for registering the FluentSerializer for XML using
[Microsoft.Extensions.DependencyInjection.Abstractions][DependencyInjectionNuget].

## Usage

For basic usage you can use this:  
```cs
serviceCollection.AddFluentXmlSerializer<TAssemblyMarker>();
```
This will use the `XmlSerializerConfiguration.Default` as the applied config.
The type parameter of `TAssemblyMarker` will be used to scan that assembly for the profiles associated with this serializer.
You can call this registration multiple times with different assemblies for additional profiles.
Alternatively there are overloads that accept a `System.Reflection.Assembly` variable.  
  
There are multiple overloads, for changing configuration the lambda approach is recomended:  
```cs
serviceCollection.AddFluentXmlSerializer<TAssemblyMarker>(static configuration =>
{
	// Change configuration values
	configuration.NewLine = LineEndings.LineFeed;
});
```
This will use the `XmlSerializerConfiguration.Default` as the applied config and allows you to change some properties.

## Default Configuration

- **Encoding** = `Encoding.Unicode` (utf-16)
- **FormatOutput** = true
- **WriteNull** = false
- **DefaultClassNamingStrategy** = PascalCase
- **DefaultPropertyNamingStrategy** = camelCase
- **DefaultConverters**
  - Converter that can handle DateTime objects (XML spec compliant)
  - Converter that can handle IConvertable types
  - Converter to handle collection types (wrapped XML collection)